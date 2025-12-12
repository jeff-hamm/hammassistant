using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace Hammlet.Extensions.Cookies;


public class ChromeLogin(ILogger<ChromeLogin> logger)
{
    public async Task Login(string email, string password)
    {
        // Set up the PuppeteerExtra instance
        var extra = new PuppeteerExtra();

        // Use the Stealth, Anonymize UA, and Adblocker plugins
        extra.Use(new StealthPlugin());
        extra.Use(new PuppeteerExtraSharp.Plugins.AnonymizeUa.AnonymizeUaPlugin());

        // Launch the browser
        var browser = await extra.LaunchAsync(new LaunchOptions
        {
            Headless = true,  // Set to false for debugging
            DefaultViewport = null,
            IgnoreDefaultArgs = true,
            DumpIO = true,
            Args = new[] { "--start-maximized", "--no-sandbox", "--disable-setuid-sandbox", "--disable-extensions" },
            ExecutablePath = "/Applications/Chromium.app/Contents/MacOS/Chromium"  // Set path to your Chromium installation
        });

        // Open a new page
        var page = await browser.NewPageAsync();

        // Navigate to OpenAI usage page
        await page.GoToAsync("https://platform.openai.com/usage", WaitUntilNavigation.Networkidle2);

        // Wait for and click the first button
        await page.WaitForSelectorAsync("button");
        await page.ClickAsync("button");

        // Wait for social buttons
        await page.WaitForSelectorAsync(".social-btn");

        try
        {
            // Try to find and click Google login
            await page.EvaluateExpressionAsync(@"(document.querySelectorAll('.social-btn') || []).forEach(element => {
                if (element.innerHTML.includes('Google')) {
                    element.click();
                }
            })");
        }
        catch (Exception)
        {
            // Fallback if Google login is not found
            await page.ClickAsync("form[data-provider] button");
        }

        // Wait for Google login page to load
        await page.WaitForNavigationAsync();
        await page.WaitForSelectorAsync("input[type='email']");

        // Fill out the email field
        await page.TypeAsync("input[type='email']", email, new TypeOptions { Delay = 30 });
        await page.Keyboard.PressAsync("Enter");

        // Wait for the password field and type the password
        await page.WaitForSelectorAsync("input[type='password']", new WaitForSelectorOptions { Visible = true });
        await page.TypeAsync("input[type='password']", password);
        await page.Keyboard.PressAsync("Enter");

        // Wait for page to finish loading after login
        await page.WaitForNavigationAsync();

        // Extract cookies from the page after the login process
        var cookies = await page.GetCookiesAsync();

        // Save cookies to a text file
        var cookiesFilePath = "cookies.txt";
        await SaveCookiesToFile(cookies, cookiesFilePath);

        // Close the browser when done
        await browser.CloseAsync();

        logger.LogInformation($"Cookies have been saved to {cookiesFilePath}");
    }

    // Method to save cookies to a file
    private static async Task SaveCookiesToFile(CookieParam[] cookies, string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        {
            foreach (var cookie in cookies)
            {
                // Write each cookie as a line in the cookies.txt file
                var cookieLine = $"{cookie.Name}={cookie.Value}; domain={cookie.Domain}; path={cookie.Path}; expires={cookie.Expires}";
                await writer.WriteLineAsync(cookieLine);
            }
        }
    }
}


public class ChromiumCookieReader(ILogger<ChromiumCookieReader> logger) : IDisposable
{

    /// <summary>
    /// Reading and decrypting Chrome cookies.
    /// <see cref="https://stackoverflow.com/questions/68643057/decrypt-google-cookies-in-c-sharp-net-framework"/>
    /// </summary>
    /// <param name="hostname"></param>
    /// <param name="folder"></param>
    /// <returns></returns>
    public List<Cookie> GetCookies(string hostname, string profileFolderPath = null)
    {

        string ChromeCookiePath = profileFolderPath != null
                                        ? System.IO.Path.Combine(profileFolderPath, @"Default\Network\Cookies")
                                        : Environment.ExpandEnvironmentVariables(@"%localappdata%\Google\Chrome\User Data\Default\Network\Cookies");


        if (!File.Exists(ChromeCookiePath))
        {
            logger.LogError($"Cookie database does not exist @ '{ChromeCookiePath}'");
            return new List<Cookie>();
        }

        List<Cookie> data = new List<Cookie>();
        try
        {
            logger.LogDebug($"Reading Cookie database @ '{ChromeCookiePath}'");
            var connBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ChromeCookiePath,
                Mode = SqliteOpenMode.ReadOnly
            };
            using (var conn = new SqliteConnection(connBuilder.ConnectionString))
            {
                conn.Open();
                //construct the host_key clause for use in the Cookies table query.
                var subDomains = GetSubDomainList(hostname);
                var hostClause = (subDomains.Count > 0)
                                    ? $"host_key in ('{hostname}', {string.Join(",", subDomains.Select(sub => $"'.{sub}'"))})"
                                    : $"host_key = '{hostname}'";

                using (var cmd = conn.CreateCommand())
                {
                    long expireTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    cmd.CommandText =
                        $@"select   creation_utc,
                                    host_key,
                                    top_frame_site_key,
                                    name,
                                    value,
                                    encrypted_value,
                                    path,
                                    expires_utc,
                                    is_secure,
                                    is_httponly,
                                    last_access_utc,
                                    has_expires,
                                    is_persistent,
                                    priority,
                                    samesite,
                                    source_scheme,
                                    source_port,
                                    is_same_party
                            from cookies
                            WHERE
                            [is_persistent] =1 AND
                            {hostClause}
                            ";

                    byte[] key = AesGcm256.GetKey(profileFolderPath);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["name"].ToString();
                            string path = reader["path"].ToString();
                            string domain = reader["host_key"].ToString();
                            long expiration = long.Parse(reader["expires_utc"].ToString());
                            byte[] encrypted_value = (byte[])reader["encrypted_value"];
                            string value = DecryptCookieValue(key, encrypted_value);
                            Cookie cookie = new Cookie(name, value, path, domain)
                            {
                                Expires = DateTimeOffset.FromUnixTimeSeconds(expiration.ChromiumDbTimeToUnixTime()).DateTime
                            };
                            data.Add(cookie);
                        }
                    }
                    conn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failure reading cookies from '{ChromeCookiePath}'");
        }

        logger.LogDebug($"Cookie count found in database for hostname='{hostname}': {data.Count}");
        return data;
    }

    /// <summary>
    /// Break the host name down into subdomain list.
    /// </summary>
    /// <param name="hostname"></param>
    /// <returns></returns>
    private List<string> GetSubDomainList(string host, bool addHostToList = false)
    {
        if (string.IsNullOrEmpty(host))
            return new List<string>();

        List<string> subDomainList = new List<string>();
        var splits = host.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        if (splits.Length <= 2)
            return new List<string>();

        //ex: www.beta.microsoft.co.uk     
        if (addHostToList)
            subDomainList.Add(host);

        string sub = host;
        for (int i = 0; i < (splits.Length - 2); i++)
        {
            //sub = twosplit(sub);
            sub = sub.Split(new char[] { '.' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            subDomainList.Add(sub);
        }

        return subDomainList;
    }
    private AesGcm256 encryption = new AesGcm256(logger);
    /// <summary>
    /// Decrypt Cookie Value
    /// <see href="https://stackoverflow.com/questions/71718371/decrypt-cookies-encrypted-value-from-chrome-chromium-80-in-c-sharp-issue-wi"/>
    /// <seealso href="https://stackoverflow.com/questions/68643057/decrypt-google-cookies-in-c-sharp-net-framework" />
    /// <seealso href="https://gist.github.com/creachadair/937179894a24571ce9860e2475a2d2ec"/>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="encryptedCookieValue"></param>
    /// <returns></returns>
    private string DecryptCookieValue(byte[] key, byte[] encryptedCookieValue)
    {
        try
        {
            byte[] nonce, ciphertextTag;
            encryption.prepare(encryptedCookieValue, out nonce, out ciphertextTag);
            string value = encryption.decrypt(ciphertextTag, key, nonce);
            return value;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error encountered decrypting cookie value");
        }

        return null;
    }

    #region Dispose

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    protected virtual void Dispose(bool disposable)
    {
        if (disposable)
        { //Dispose of Native
        }
    }

    #endregion

    /// <summary>
    /// AES GCM Decryption Class
    /// </summary>
    class AesGcm256(ILogger logger)
    {
        /// <summary>
        /// Get the cookie key value.
        /// </summary>
        /// <param name="profileFolderPath"> Path to profile folder. If null, uses the default Chrome folder</param>
        /// <returns></returns>
        public static byte[] GetKey(string profileFolderPath = null)
        {
            string sR = string.Empty;
            string path = profileFolderPath != null
                               ? System.IO.Path.Combine(profileFolderPath, @"Local State")
                               : Environment.ExpandEnvironmentVariables(@"%localappdata%\Google\Chrome\User Data\Local State");


            string v = File.ReadAllText(path);

            var json = JsonSerializer.Deserialize<JsonObject>(v);
            string key = json["os_crypt"]["encrypted_key"].GetValue<string>();

            byte[] src = Convert.FromBase64String(key);
            byte[] encryptedKey = src.Skip(5).ToArray();

            byte[] decryptedKey = ProtectedData.Unprotect(encryptedKey, null, DataProtectionScope.CurrentUser);

            return decryptedKey;
        }

        public string decrypt(byte[] encryptedBytes, byte[] key, byte[] iv)
        {
            string sR = String.Empty;
            try
            {
                GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
                AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, iv, null);

                cipher.Init(false, parameters);
                byte[] plainBytes = new byte[cipher.GetOutputSize(encryptedBytes.Length)];
                Int32 retLen = cipher.ProcessBytes(encryptedBytes, 0, encryptedBytes.Length, plainBytes, 0);
                cipher.DoFinal(plainBytes, retLen);

                sR = Encoding.UTF8.GetString(plainBytes).TrimEnd("\r\n\0".ToCharArray());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error encountered decrypting cookie value");
            }

            return sR;
        }

        public void prepare(byte[] encryptedData, out byte[] nonce, out byte[] ciphertextTag)
        {
            nonce = new byte[12];
            ciphertextTag = new byte[encryptedData.Length - 3 - nonce.Length];

            System.Array.Copy(encryptedData, 3, nonce, 0, nonce.Length);
            System.Array.Copy(encryptedData, 3 + nonce.Length, ciphertextTag, 0, ciphertextTag.Length);
        }
    }
}

public static class ChromiumDbDateTimeExtensions
{
    //1601-01-01T00:00:00Z
    private static DateTimeOffset ProlepticGregorianEpoch = new DateTimeOffset(1601, 1, 1, 0, 0, 0, TimeSpan.Zero);

    /// <summary>
    /// Convert a Chromium DB Proleptic Gregorian Time Stamp to Unix Time        
    /// <para>
    /// Timestamps
    //     The expires_utc and creation_utc fields contain timestamps given as integer numbers of microseconds elapsed since midnight 01-Jan-1601 UTC in the proleptic calendar.
    //     The Unix epoch is 11644473600 seconds after this moment.
    /// </para>
    /// <see cref="https://gist.github.com/creachadair/937179894a24571ce9860e2475a2d2ec#timestamps"/>
    /// </summary>
    /// <param name="prolepticGregorianBasedTimeInMicroseconds "></param>
    /// <returns></returns>
    public static long ChromiumDbTimeToUnixTime(this long prolepticGregorianEpochBasedTimeInMicroseconds)
    {
        var unixTimeSeconds = (prolepticGregorianEpochBasedTimeInMicroseconds / 1000000) + ProlepticGregorianEpoch.ToUnixTimeSeconds();
        return unixTimeSeconds;
    }
}



public class ChatGptExtractor(ILogger<ChatGptExtractor> logger)
{
    public void Extract()
    {
        string url = "https://music.youtube.com";
        string dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"Google\Chrome\User Data\Default\Cookies");
        string cookieFilePath = "/config/cookies.txt";

        if (!File.Exists(dbPath))
        {
            logger.LogInformation("Chrome cookie database not found!");
            return;
        }

        bool isLoggedIn = CheckCookies(dbPath);

        if (!isLoggedIn)
        {
            logger.LogInformation("You are not logged in. Opening Chrome...");
            Process.Start("cmd", $"/c start {url}");

            logger.LogInformation("Please log in to YouTube Music, then press Enter to continue...");
            Console.ReadLine();

            logger.LogInformation("Re-checking cookies...");
            Thread.Sleep(5000);  // Wait for a few seconds before rechecking

            isLoggedIn = CheckCookies(dbPath);
            if (!isLoggedIn)
            {
                logger.LogInformation("Login not detected. Please try again.");
                return;
            }
        }

        SaveCookies(dbPath, cookieFilePath);
        logger.LogInformation($"Cookies saved successfully to {cookieFilePath}");
    }

    static bool CheckCookies(string dbPath)
    {
        string connectionString = $"Data Source={dbPath};Version=3;";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM cookies WHERE host_key LIKE '%music.youtube.com%'";

            using (var command = new SqliteCommand(query, connection))
            {
                var count = (long?)command.ExecuteScalar();
                return count > 0; // If we have cookies, assume logged in
            }
        }
    }

    static void SaveCookies(string dbPath, string cookieFilePath)
    {
        string connectionString = $"Data Source={dbPath};Version=3;";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT name, encrypted_value FROM cookies WHERE host_key LIKE '%music.youtube.com%'";

            using (var command = new SqliteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                using (StreamWriter writer = new StreamWriter(cookieFilePath))
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        byte[] encryptedValue = (byte[])reader["encrypted_value"];
                        string decryptedValue = DecryptCookie(encryptedValue);

                        writer.WriteLine($"{name}={decryptedValue}");
                    }
                }
            }
        }
    }

    static string DecryptCookie(byte[] encryptedData)
    {
        try
        {
            byte[] decrypted = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decrypted);
        }
        catch
        {
            return "Decryption failed.";
        }
    }
}

