using System.Reactive.Subjects;

namespace Hammlet.Extensions;

public abstract class ScopedObservable<T, E> : IObservable<T>, IDisposable
{
    private readonly IDisposable _subscription;
    private readonly Subject<T> _subject = new();

    public ScopedObservable(IObservable<E> innerObservable)
    {
        _subscription = innerObservable.Subscribe(e => OnNext(_subject,e));
    }

    protected abstract void OnNext(IObserver<T> observer, E obj);

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return _subject.Subscribe(observer);
    }

    public void Dispose()
    {
        // When disposed unsubscribe from inner observable
        // this will make all subscribers of our Subject stop receiving events
        _subscription.Dispose();
        _subject.Dispose();
    }
}