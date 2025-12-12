//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.WebSockets;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using NetDaemon.Client.Internal.Net;
//using NetDaemon.HassModel;

//namespace NetDaemon.Extensions.Hammlet.Client;
//internal class WebSocketClientTransportPipelineFactory : IWebSocketClientTransportPipelineFactory
//{
//    public IWebSocketClientTransportPipeline New(IWebSocketClient webSocketClient)
//    {
//        if (webSocketClient.State != WebSocketState.Open)
//            throw new ApplicationException("Unexpected state of WebSocketClient, should be 'Open'");

//        return new ProtocolLoggingWebSocketClientTransportPipeline(webSocketClient,HassJsonContext.DefaultOptions);
//    }
//}

//internal class ProtocolLoggingWebSocketClientTransportPipeline(IWebSocketClient clientWebSocket,
//    JsonSerializerOptions serializerOptions, 
//    ILogger<ProtocolLoggingWebSocketClientTransportPipeline> logger) :
//    WebSocketClientTransportPipeline(clientWebSocket,serializerOptions)
//{
//    public static readonly EventId RequestCorrelationEventId = new(1000, "RequestCorrelation");
//    public static readonly EventId RequestJsonEventId = new(1001, "RequestJson");
//    public static readonly EventId ResponseCorrelationEventId = new(2000, "Response");
//    public static readonly EventId ResponseJsonEventId = new(2001, "ResponseJson");
//    public static readonly EventId RequestAndResponse = new(3000, "ResponseJson");
//    private ConcurrentDictionary<IComparable,
//        WeakReference<ICorrelated>> requestCorrelation = new();
//    protected override byte[] SerializeRequestMessage<T>(T message)
//    {
//        if(message is ICorrelated correlatedMessage)
//        {
//            logger.LogTrace("{CorrelationKey}: {@RequestMessage}",correlatedMessage.Key,correlatedMessage);
//            if (message is ICorrelatedRequest)
//            {
//                requestCorrelation.AddOrUpdate(correlatedMessage.Key, (key) => new WeakReference<ICorrelated>(correlatedMessage),
//                    (key, previousReference) =>
//                    {
//                        if (!previousReference.TryGetTarget(out var previousTarget) || previousTarget is not { } previous)
//                            logger.LogTrace("Replacing existing request for {CorrrelationKey} the previous request had already been garbage collected",key);
//                        else
//                            logger.LogTrace("Replacing existing request for {CorrrelationKey}, replacing previous {@PreviousRequestMessage}", key, previous);
//                        return new WeakReference<ICorrelated>(correlatedMessage);
//                    });
//            }
//        }
//        var requestBytes = base.SerializeRequestMessage(message);
//        logger.LogTrace(RequestJsonEventId, "{RequestMessageBytes}", requestBytes);
//        return requestBytes;
//    }

//    protected override T[] DeserializeResponseElement<T>(JsonElement message)
//    {
//        logger.LogTrace(ResponseJsonEventId, "{ResponseMessageJson}", message);
//        var response = base.DeserializeResponseElement<T>(message);
//        if(typeof(ICorrelatedResponse).IsAssignableFrom(typeof(T)))
//        {
//            var responseDictionary = new Dictionary<IComparable, List<(object? request, object response)>>();
//            foreach (var item in response.OfType<ICorrelatedResponse>())
//            {
//                object? request = null;
//                if (!requestCorrelation.TryRemove(item.Key, out var requestVal))
//                {
//                    logger.LogTrace(ResponseCorrelationEventId, "No Request found for {CorrelationKey}", item.Key);
//                }
//                else if (!requestVal.TryGetTarget(out var requestTarget) || requestTarget is not {} correlatedRequest)
//                {
//                    logger.LogTrace(ResponseCorrelationEventId, "Found request for {CorrelationKey}, but the request had already been garbage collected", item.Key);
//                }
//                else {
//                    request = correlatedRequest;
//                }
//                if(!responseDictionary.TryGetValue(item.Key, out var responseList))
//                {
//                    responseList = new List<(object? request, object response)>();
//                    responseDictionary.Add(item.Key, responseList);
//                }
//                responseList.Add((request, item));
//            }
//            foreach(var kvp in responseDictionary)
//            {
//                foreach (var item in kvp.Value)
//                {
//                    logger.LogDebug(ResponseCorrelationEventId, "key={CorrelationKey},request={@request},response={@response}", kvp.Key, item.request,item.response);
//                }
//            }
//        }
//        return response;
//    }

//}
