using System.Reactive.Disposables;
using System.Reactive.Linq;
using Hammlet.Extensions;
using Microsoft.Extensions.Logging;
using NetDaemon.AppModel;
using NetDaemon.Client;
using NetDaemon.Client.HomeAssistant.Model;
using NetDaemon.HassModel;

namespace NetDaemon.Extensions.Hammlet.Services;
using Event=HassEvent;
public class AsyncServiceCaller(IHomeAssistantRunner haRunner, ILogger<AsyncServiceCaller> logger, IHaContext context) : IAsyncInitializable, IDisposable
{
    private CancellationTokenSource? _cancelAll;
    private readonly CompositeDisposable _toDispose = new CompositeDisposable();
    private CancellationToken cancelToken = default;
    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        localConnection = haRunner.CurrentConnection ?? throw new InvalidOperationException("Home Assistant connection is not initialized");
        _cancelAll = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cancelToken = _cancelAll.Token;
        _toDispose.Add(_cancelAll);
        _toDispose.Add(haRunner.OnDisconnect.Subscribe(OnDisconnect));
        _toDispose.Add(haRunner.OnConnect.Subscribe(OnConnected));
        return Task.CompletedTask;
    }

    private async Task<IObservable<Event>> GetEventStream(IHomeAssistantConnection connection) => //context.Events;
        await connection.SubscribeToHomeAssistantEventsAsync("state_change", cancelToken)
            .ConfigureAwait(false);

    private IHomeAssistantConnection? localConnection;
    protected bool IsConnected => localConnection != null;
    private async Task<IHomeAssistantConnection> GetConnectionAsync()
    {
        while (localConnection == null)
        {
            if (cancelToken.IsCancellationRequested)
                throw new OperationCanceledException();
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(AsyncServiceCaller));
            var timeout = ServiceTimeout.Default;
            var waitTimeout = timeout.NextTimeout();
            logger.LogInformation("Client connection failed, retrying in {Seconds} seconds...", waitTimeout.TotalSeconds);
            await Task.Delay(waitTimeout, cancelToken).ConfigureAwait(false);

        }
        return localConnection;
    }



    private void OnConnected(IHomeAssistantConnection newConnection)
    {
        localConnection = newConnection;
    }

    private void OnDisconnect(DisconnectReason disconnectReason)
    {
        localConnection = null;
    }

    public async Task<bool> CallServiceAndWaitForChangeAsync(ServiceCall call, EntityStateEventMonitor monitor, TimeSpan timeout, CancellationToken token = default)
    {
        var connection = await GetConnectionAsync();
        //var events = (await GetEventStream(connection))
            
        //    .Where(e =>
        //    {
        //        if (e.EventType != monitor.EventType)
        //        {
        //            logger.LogDebug("Ignoring event {@EventType}", e.EventType);
        //            return false;
        //        }

        //        logger.LogDebug("Checking event {@EventType}, {@Data}", e, e.DataElement?.ToString());
        //            return monitor.Filter(e);
        //    })
        //    .Select(e => new EntityChangedEvent(e));
        var timeoutSource = new CancellationTokenSource(timeout);
        using var lts = CancellationTokenSource.CreateLinkedTokenSource(cancelToken, token, timeoutSource.Token);
        bool found = false;
        try
        {
            //using var responses = events.SubscribeAsyncConcurrent(e =>
            //{
            //    if (!monitor.Found(e)) return Task.CompletedTask;
            //    logger.LogInformation("Found entity state change: {EntityId}", e.EntityId);
            //    found = true;
            //    if (!timeoutSource.IsCancellationRequested)
            //        timeoutSource.Cancel();
            //    return Task.CompletedTask;
            //},10,logger);
            logger.LogTrace("Sending command {@Command}", call);
            logger.LogDebug("Calling service {domain}.{service} on {entity}",call.Domain,call.Service, 
                call.Target?.EntityIds?.StringJoin().PrefixNotNull("EntityIds: ") + 
                call.Target?.DeviceIds.StringJoin().PrefixNotNull("DeviceIds: ") +
                call.Target?.AreaIds.StringJoin().PrefixNotNull("AreaIds: ") + 
                call.Target?.LabelIds.StringJoin().PrefixNotNull("LabelIds: ") + 
                call.Target?.FloorIds.StringJoin().PrefixNotNull("FloorIds: ")
            );
            var response = await connection.SendCommandAndReturnResponseRawAsync(new CallServiceCommand()
            {
                Domain = call.Domain,
                Service = call.Service,
                ServiceData = call.Data,
                Target = call.Target
            }, lts.Token);
            logger.LogTrace($"Got Response {response}");
//            await Task.Delay(Timeout.Infinite, lts.Token);
        }
        catch (TaskCanceledException ex)
        {
        }
        finally
        {
            timeoutSource.Dispose();
        }
//        if(!found)
//            logger.LogWarning("Entity state change not found within timeout");
        return found;

    }


    private bool IsDisposed { get; set; }

    public void Dispose()
    {
        IsDisposed = true;
        _toDispose.Dispose();
    }
}