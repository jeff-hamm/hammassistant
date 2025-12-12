using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetDaemon.Client.HomeAssistant.Model;

namespace Hammlet.Extensions;

public static class LogExtensions
{
    public static string StringJoin(this IEnumerable<string>? @this, char seperator = ',') => @this == null ? "" : String.Join(seperator,@this);
    public static string StringJoin(this IEnumerable<string>? @this, string seperator) => @this == null ? "" :String.Join(seperator,@this);
    public static string StringJoin<TValue>(this IEnumerable<TValue>? @this, Func<TValue,string> project, char seperator = ',') => @this == null ? "" :String.Join(seperator,@this.Select(project));

    public static string? PrefixNotNull(this string? prefix, string? message)
    {
        if (string.IsNullOrEmpty(message))
            return message;
        return $"{prefix}{message}";
    }
    public static string? SuffixNotNull(this string? prefix, string? message)
    {
        if (string.IsNullOrEmpty(message))
            return message;
        return $"{prefix}{message}";
    } 
}

public static class LogUtil
{
    public static ILoggerFactory LoggerFactory { get; set;  } = new LoggerFactory();
    public static ILogger<T> Logger<T>() => LoggerFactory.CreateLogger<T>();
}

public static class Log<TContext>
{
    private static readonly ILogger<TContext> logger = LogUtil.Logger<TContext>();


    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogDebug(0, "Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogDebug(
        EventId eventId,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Debug, eventId, message, args);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogDebug(exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogDebug(

        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Debug, exception, message, args);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogDebug("Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogDebug(string? message, params object?[] args)
    {
        logger.Log(LogLevel.Debug, message, args);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogTrace(0, exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogTrace(

        EventId eventId,
        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Trace, eventId, exception, message, args);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogTrace(0, "Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogTrace(

        EventId eventId,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Trace, eventId, message, args);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogTrace(exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogTrace(

        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Trace, exception, message, args);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogTrace("Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogTrace(string? message, params object?[] args)
    {
        logger.Log(LogLevel.Trace, message, args);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogInformation(0, exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogInformation(

        EventId eventId,
        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Information, eventId, exception, message, args);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogInformation(0, "Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogInformation(

        EventId eventId,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Information, eventId, message, args);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogInformation(exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogInformation(

        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Information, exception, message, args);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogInformation("Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogInformation(string? message, params object?[] args)
    {
        logger.Log(LogLevel.Information, message, args);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogWarning(0, exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogWarning(

        EventId eventId,
        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Warning, eventId, exception, message, args);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogWarning(0, "Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogWarning(

        EventId eventId,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Warning, eventId, message, args);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogWarning(exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogWarning(

        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Warning, exception, message, args);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogWarning("Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogWarning(string? message, params object?[] args)
    {
        logger.Log(LogLevel.Warning, message, args);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogError(0, exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogError(

        EventId eventId,
        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Error, eventId, exception, message, args);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogError(0, "Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogError(

        EventId eventId,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Error, eventId, message, args);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogError(exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogError(

        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Error, exception, message, args);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogError("Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogError(string? message, params object?[] args)
    {
        logger.Log(LogLevel.Error, message, args);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogCritical(0, exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogCritical(

        EventId eventId,
        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Critical, eventId, exception, message, args);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogCritical(0, "Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogCritical(

        EventId eventId,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Critical, eventId, message, args);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogCritical(exception, "Error while processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogCritical(

        Exception? exception,
        string? message,
        params object?[] args)
    {
        logger.Log(LogLevel.Critical, exception, message, args);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c>.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>
    /// <code language="csharp">
    /// logger.LogCritical("Processing request from {Address}", address)
    /// </code>
    /// </example>
    public static void LogCritical(string? message, params object?[] args)
    {
        logger.Log(LogLevel.Critical, message, args);
    }

}


