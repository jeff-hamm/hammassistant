using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NetDaemon.Client;
using NetDaemon.Client.HomeAssistant.Model;
using NetDaemon.Client.Internal.HomeAssistant.Commands;

namespace NetDaemon.Extensions.Hammlet.Client;


internal class ProtocolLoggingConnection : IHomeAssistantConnection
{

    internal IHomeAssistantConnection InternalConnection { get; }
    public static readonly EventId RequestCorrelationEventId = new(1000, "RequestCorrelation");
    public static readonly EventId RequestJsonEventId = new(1001, "RequestJson");
    public static readonly EventId ResponseCorrelationEventId = new(2000, "Response");
    public static readonly EventId ResponseJsonEventId = new(2001, "ResponseJson");
    public static readonly EventId RequestAndResponse = new(3000, "ResponseJson");
    private ConcurrentDictionary<IComparable,
        (DateTimeOffset Added,WeakReference<object>)> requestCorrelation = new();

    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<ProtocolLoggingConnection> _logger;

    public ProtocolLoggingConnection(IHomeAssistantConnection _internalConnection,
        JsonSerializerOptions jsonOptions, 
        ILogger<ProtocolLoggingConnection> logger)
    {
        _jsonOptions = jsonOptions;
        _logger = logger;
        InternalConnection = _internalConnection;
    }

    protected void LogRequest<T>(T message, IComparable? requestKey=null) where T:notnull
    {
        if(message is ICorrelated correlatedMessage)
            requestKey = correlatedMessage.Key;
        else if(message is string msgKey)
            requestKey = msgKey;
        else
            requestKey = message.GetHashCode();
        _logger.LogTrace("{CorrelationKey}: {@RequestMessage}",requestKey,message);

        requestCorrelation.AddOrUpdate(requestKey, (_) => (DateTimeOffset.UtcNow, new WeakReference<object>(message)),
        (key, previousReference) =>
        {
            if (!previousReference.Item2.TryGetTarget(out var previousTarget) || previousTarget is not { } previous)
                _logger.LogTrace("Replacing existing request for {CorrrelationKey} the previous request had already been garbage collected",key);
            else
                _logger.LogTrace("Replacing existing request for {CorrrelationKey}, replacing previous {@PreviousRequestMessage}", key, previous);
            return 
                (DateTimeOffset.UtcNow, new WeakReference<object>(message));
        });
        _logger.LogTrace(RequestJsonEventId, "{RequestMessage}", JsonSerializer.Serialize(message, _jsonOptions));
    }

    protected T LogResponse<T>(T response,IComparable requestKey) 
    {
 //       logger.LogTrace(ResponseJsonEventId, "{ResponseMessageJson}", message);
        if(typeof(ICorrelatedResponse).IsAssignableFrom(typeof(T)))
        {
            var responseDictionary = new Dictionary<IComparable, List<(object? request, object? response)>>();
            if (response is ICorrelatedResponse item)
                requestKey = item.Key;

            object? requestMsg = null;
                if (!requestCorrelation.TryRemove(requestKey, out var requestVal))
                {
                    _logger.LogTrace(ResponseCorrelationEventId, "No Request found for {CorrelationKey}", requestKey);
                }
                else if (!requestVal.Item2.TryGetTarget(out var requestTarget) || requestTarget is not {} correlatedRequest)
                {
                    _logger.LogTrace(ResponseCorrelationEventId, "Found request for {CorrelationKey}, but the request had already been garbage collected", requestKey);
                }
                else {
                    requestMsg = correlatedRequest;
                }
                if(!responseDictionary.TryGetValue(requestKey, out var responseList))
                {
                    responseList = new List<(object? request, object? response)>();
                    responseDictionary.Add(requestKey, responseList);
                }
                responseList.Add(((requestMsg, response)));
            foreach(var kvp in responseDictionary)
            {
                foreach (var (request,responseVal) in kvp.Value)
                {
                    _logger.LogDebug(ResponseCorrelationEventId, "key={CorrelationKey},request={@request},response={@response}", kvp.Key, request,responseVal);
                }
            }
        }
        else
        {
            _logger.LogTrace("key={CorrelationKey},response={@response}",requestKey,response);
        }
        return response;
    }

    public async Task<T?> GetApiCallAsync<T>(string apiPath, CancellationToken cancelToken)
    {
        var key = "GET:" + apiPath;
        LogRequest(key);
        return await LogResponse(InternalConnection.GetApiCallAsync<T>(apiPath, cancelToken),key);
    }

    public async Task<T?> PostApiCallAsync<T>(string apiPath, CancellationToken cancelToken, object? data = null)
    {
        var key = "POST:" + apiPath;
        LogRequest(key);
        return LogResponse(await InternalConnection.PostApiCallAsync<T>(apiPath, cancelToken, data),key);
    }

    public ValueTask DisposeAsync() => InternalConnection.DisposeAsync();

    public async Task<IObservable<HassEvent>> SubscribeToHomeAssistantEventsAsync(string? eventType, CancellationToken cancelToken)
    {
        var key = "Subscribe: " + eventType;
        LogRequest(key);
        return (await InternalConnection.SubscribeToHomeAssistantEventsAsync(eventType, cancelToken).ConfigureAwait(false)).Do(e => LogResponse(e,key));
    }

    public async Task SendCommandAsync<T>(T command, CancellationToken cancelToken) where T : CommandMessage
    {
        LogRequest(command, command.GetHashCode());
        await InternalConnection.SendCommandAsync(command, cancelToken);
        LogResponse("SendCommandAsyncResponse", command.GetHashCode());
    }

    public async Task<TResult?> SendCommandAndReturnResponseAsync<T, TResult>(T command, CancellationToken cancelToken) where T : CommandMessage
    {
        LogRequest(command);
        return LogResponse(await InternalConnection.SendCommandAndReturnResponseAsync<T, TResult>(command, cancelToken),command.GetHashCode());
    }

    public async Task<JsonElement?> SendCommandAndReturnResponseRawAsync<T>(T command, CancellationToken cancelToken) where T : CommandMessage
    {
        LogRequest(command);
        return LogResponse(await InternalConnection.SendCommandAndReturnResponseRawAsync(command, cancelToken),command.GetHashCode());
    }

    public async Task<HassMessage?> SendCommandAndReturnHassMessageResponseAsync<T>(T command, CancellationToken cancelToken) where T : CommandMessage
    {
        LogRequest(command);
        return LogResponse(await InternalConnection.SendCommandAndReturnHassMessageResponseAsync(command, cancelToken),command.GetHashCode());
    }

    public Task WaitForConnectionToCloseAsync(CancellationToken cancelToken)
    {
        _logger.LogTrace("Waiting for connection to close");
        return InternalConnection.WaitForConnectionToCloseAsync(cancelToken);
    }
}