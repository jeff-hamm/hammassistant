using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hammlet.Extensions.Observables;
internal interface ISink<in TTarget>
{
    void ForwardOnNext(TTarget value);

    void ForwardOnCompleted();

    void ForwardOnError(Exception error);
}

public abstract class Sink<TTarget> : ISink<TTarget>, IDisposable
{
    private SingleAssignmentDisposableValue _upstream;
    private volatile IObserver<TTarget> _observer;

    protected Sink(IObserver<TTarget> observer) => this._observer = observer;

    public void Dispose()
    {
        if (Interlocked.Exchange<IObserver<TTarget>>(ref this._observer, NopObserver<TTarget>.Instance) == NopObserver<TTarget>.Instance)
            return;
        this.Dispose(true);
    }

    /// <summary>
    /// Override this method to dispose additional resources.
    /// The method is guaranteed to be called at most once.
    /// </summary>
    /// <param name="disposing">If true, the method was called from <see cref="M:System.Reactive.Sink`1.Dispose" />.</param>
    protected virtual void Dispose(bool disposing) => this._upstream.Dispose();

    public void ForwardOnNext(TTarget value) => this._observer.OnNext(value);

    public void ForwardOnCompleted()
    {
        this._observer.OnCompleted();
        this.Dispose();
    }

    public void ForwardOnError(Exception error)
    {
        this._observer.OnError(error);
        this.Dispose();
    }

    protected void SetUpstream(IDisposable upstream) => this._upstream.Disposable = upstream;

    protected void DisposeUpstream() => this._upstream.Dispose();
}
public abstract class Sink<TSource, TTarget> : Sink<TTarget>, IObserver<TSource>
{
    protected Sink(IObserver<TTarget> observer)
        : base(observer)
    {
    }

    public virtual void Run(IObservable<TSource> source)
    {
        this.SetUpstream(source.SubscribeSafe<TSource>((IObserver<TSource>) this));
    }

    public abstract void OnNext(TSource value);

    public virtual void OnError(Exception error) => this.ForwardOnError(error);

    public virtual void OnCompleted() => this.ForwardOnCompleted();

    public IObserver<TTarget> GetForwarder()
    {
        return (IObserver<TTarget>) new Sink<TSource, TTarget>._(this);
    }

    private sealed class _ : IObserver<TTarget>
    {
        private readonly Sink<TSource, TTarget> _forward;

        public _(Sink<TSource, TTarget> forward) => this._forward = forward;

        public void OnNext(TTarget value) => this._forward.ForwardOnNext(value);

        public void OnError(Exception error) => this._forward.ForwardOnError(error);

        public void OnCompleted() => this._forward.ForwardOnCompleted();
    }
}

internal sealed class NopObserver<T> : IObserver<T>
{
    public static readonly IObserver<T> Instance = (IObserver<T>) new NopObserver<T>();

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(T value)
    {
    }
}


internal abstract class Producer<TTarget, TSink> : IProducer<TTarget>, IObservable<TTarget> where TSink : IDisposable
{
    /// <summary>Publicly visible Subscribe method.</summary>
    /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
    /// <returns>IDisposable to cancel the subscription. This causes the underlying sink to be notified of unsubscription, causing it to prevent further messages from being sent to the observer.</returns>
    public IDisposable Subscribe(IObserver<TTarget> observer)
    {
        return observer != null ? this.SubscribeRaw(observer, true) : throw new ArgumentNullException(nameof (observer));
    }

    public IDisposable SubscribeRaw(IObserver<TTarget> observer, bool enableSafeguard)
    {
        ISafeObserver<TTarget>? safeObserver = null;
        if (enableSafeguard)
            observer = (IObserver<TTarget>) (safeObserver = SafeObserver<TTarget>.Wrap(observer));
        TSink sink = this.CreateSink(observer);
        safeObserver?.SetResource((IDisposable) sink);
        if (CurrentThreadScheduler.IsScheduleRequired)
            CurrentThreadScheduler.Instance.ScheduleAction<(Producer<TTarget, TSink>, TSink)>((this, sink), (Action<(Producer<TTarget, TSink>, TSink)>) (tuple => tuple.Item1.Run(tuple.Item2)));
        else
            this.Run(sink);
        return (IDisposable) sink;
    }

    /// <summary>
    /// Core implementation of the query operator, called upon a new subscription to the producer object.
    /// </summary>
    /// <param name="sink">The sink object.</param>
    protected abstract void Run(TSink sink);

    protected abstract TSink CreateSink(IObserver<TTarget> observer);
}


  /// <summary>
  /// Base class for implementation of query operators, providing performance benefits over the use of Observable.Create.
  /// </summary>
  /// <typeparam name="TSource">Type of the resulting sequence's elements.</typeparam>
  internal abstract class BasicProducer<TSource> : IProducer<TSource>, IObservable<TSource>
  {
    /// <summary>Publicly visible Subscribe method.</summary>
    /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
    /// <returns>IDisposable to cancel the subscription. This causes the underlying sink to be notified of unsubscription, causing it to prevent further messages from being sent to the observer.</returns>
    public IDisposable Subscribe(IObserver<TSource> observer)
    {
      return observer != null ? this.SubscribeRaw(observer, true) : throw new ArgumentNullException(nameof (observer));
    }

    public IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard)
    {
      ISafeObserver<TSource>? safeObserver = null;
      if (enableSafeguard)
        observer = (IObserver<TSource>) (safeObserver = SafeObserver<TSource>.Wrap(observer));
      IDisposable resource;
      if (CurrentThreadScheduler.IsScheduleRequired)
      {
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();

        CurrentThreadScheduler.Instance.ScheduleAction<(BasicProducer<TSource>, SingleAssignmentDisposable, IObserver<TSource>)>(
            (this, assignmentDisposable, observer),
            (tuple => tuple.Item2.Disposable = tuple.Item1.Run(tuple.Item3)));
        resource = (IDisposable) assignmentDisposable;
      }
      else
        resource = this.Run(observer);
      safeObserver?.SetResource(resource);
      return resource;
    }

    /// <summary>
    /// Core implementation of the query operator, called upon a new subscription to the producer object.
    /// </summary>
    /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
    /// <returns>Disposable representing all the resources and/or subscriptions the operator uses to process events.</returns>
    /// <remarks>The <paramref name="observer">observer</paramref> passed in to this method is not protected using auto-detach behavior upon an OnError or OnCompleted call. The implementation must ensure proper resource disposal and enforce the message grammar.</remarks>
    protected abstract IDisposable Run(IObserver<TSource> observer);
  }

public static class ObserverExtensions {
    internal static IDisposable ScheduleAction<TState>(
        this IScheduler scheduler,
        TState state,
        Action<TState> action)
    {
        if (scheduler == null)
            throw new ArgumentNullException(nameof (scheduler));
        if (action == null)
            throw new ArgumentNullException(nameof (action));
        return scheduler.Schedule<(Action<TState>, TState)>((action, state), (Func<IScheduler, (Action<TState>, TState), IDisposable>) ((_, tuple) =>
        {
            tuple.Item1(tuple.Item2);
            return Disposable.Empty;
        }));
    }


    internal static ISafeObserver<T> MakeSafe<T>(this AnonymousObserver<T> @this)
    {
        return (ISafeObserver<T>) new AnonymousSafeObserver<T>(@this.OnNext, @this.OnError, @this.OnCompleted);
    }

}

internal abstract class SafeObserver<TSource> : 
      ISafeObserver<TSource>,
      IObserver<TSource>,
      IDisposable
  {
      private SingleAssignmentDisposableValue _disposable;

      public static ISafeObserver<TSource> Wrap(IObserver<TSource> observer)
      {
          return observer is AnonymousObserver<TSource> anonymousObserver ? anonymousObserver.MakeSafe() : (ISafeObserver<TSource>) new SafeObserver<TSource>.WrappingSafeObserver(observer);
      }

      public abstract void OnNext(TSource value);

      public abstract void OnError(Exception error);

      public abstract void OnCompleted();

      public void SetResource(IDisposable resource) => this._disposable.Disposable = resource;

      public void Dispose() => this.Dispose(true);

      protected virtual void Dispose(bool disposing)
      {
          if (!disposing)
              return;
          this._disposable.Dispose();
      }

      private sealed class WrappingSafeObserver : SafeObserver<TSource>
      {
          private readonly IObserver<TSource> _observer;

          public WrappingSafeObserver(IObserver<TSource> observer) => this._observer = observer;

          public override void OnNext(TSource value)
          {
              bool flag = false;
              try
              {
                  this._observer.OnNext(value);
                  flag = true;
              }
              finally
              {
                  if (!flag)
                      this.Dispose();
              }
          }

          public override void OnError(Exception error)
          {
              using (this)
                  this._observer.OnError(error);
          }

          public override void OnCompleted()
          {
              using (this)
                  this._observer.OnCompleted();
          }
      }
  }
  /// <summary>
  /// Base interface for observers that can dispose of a resource on a terminal notification
  /// or when disposed itself.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  internal interface ISafeObserver<in T> : IObserver<T>, IDisposable
  {
      void SetResource(IDisposable resource);
  }

  /// <summary>
  /// Interface with variance annotation; allows for better type checking when detecting capabilities in SubscribeSafe.
  /// </summary>
  /// <typeparam name="TSource">Type of the resulting sequence's elements.</typeparam>
  internal interface IProducer<out TSource> : IObservable<TSource>
  {
      IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard);
  }



  /// <summary>
  /// This class fuses logic from ObserverBase, AnonymousObserver, and SafeObserver into one class. When an observer
  /// needs to be safeguarded, an instance of this type can be created by SafeObserver.Create when it detects its
  /// input is an AnonymousObserver, which is commonly used by end users when using the Subscribe extension methods
  /// that accept delegates for the On* handlers. By doing the fusion, we make the call stack depth shorter which
  /// helps debugging and some performance.
  /// </summary>
  internal sealed class AnonymousSafeObserver<T> : SafeObserver<T>
  {
      private readonly Action<T> _onNext;
      private readonly Action<Exception> _onError;
      private readonly Action _onCompleted;
      private int _isStopped;

      public AnonymousSafeObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
      {
          this._onNext = onNext;
          this._onError = onError;
          this._onCompleted = onCompleted;
      }

      public override void OnNext(T value)
      {
          if (this._isStopped != 0)
              return;
          bool flag = false;
          try
          {
              this._onNext(value);
              flag = true;
          }
          finally
          {
              if (!flag)
                  this.Dispose();
          }
      }

      public override void OnError(Exception error)
      {
          if (Interlocked.Exchange(ref this._isStopped, 1) != 0)
              return;
          using (this)
              this._onError(error);
      }

      public override void OnCompleted()
      {
          if (Interlocked.Exchange(ref this._isStopped, 1) != 0)
              return;
          using (this)
              this._onCompleted();
      }
  }
