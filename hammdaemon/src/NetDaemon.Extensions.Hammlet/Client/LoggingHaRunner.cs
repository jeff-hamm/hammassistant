using NetDaemon.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NetDaemon.Extensions.Hammlet.Client;
public class LoggingHaRunner(IHomeAssistantRunner baseRunner, JsonSerializerOptions options, ILoggerFactory loggerFactory) : IHomeAssistantRunner
{
    public ValueTask DisposeAsync() => baseRunner.DisposeAsync();

    public Task RunAsync(string host, int port, bool ssl, string token, TimeSpan timeout, CancellationToken cancelToken) => baseRunner.RunAsync(host, port, ssl, token, timeout, cancelToken);

    public Task RunAsync(string host, int port, bool ssl, string token, string websocketPath, TimeSpan timeout,
        CancellationToken cancelToken) =>
        baseRunner.RunAsync(host, port, ssl, token, websocketPath, timeout, cancelToken);


    private ProtocolLoggingConnection? _knownConnection;

    public IObservable<IHomeAssistantConnection> OnConnect =>
            baseRunner.OnConnect.Select(c =>
                _knownConnection = new ProtocolLoggingConnection(c, options, loggerFactory.CreateLogger<ProtocolLoggingConnection>()));

    public IObservable<DisconnectReason> OnDisconnect => baseRunner.OnDisconnect;

    public IHomeAssistantConnection? CurrentConnection
    {
        get
        {
            if(baseRunner.CurrentConnection == null)
                return null;
            if (_knownConnection?.InternalConnection == baseRunner.CurrentConnection)
                return _knownConnection;
            return _knownConnection = new ProtocolLoggingConnection(baseRunner.CurrentConnection, options, loggerFactory.CreateLogger<ProtocolLoggingConnection>());
        }
    }
}
