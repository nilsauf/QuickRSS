namespace QuickRSS.Logic.Extensions
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public static class IObservableExtensions
    {
        public static IObservable<T> DisposeOnNext<T>(this IObservable<T> source)
        {
            return Observable.Create<T>(obs =>
            {
                SerialDisposable serial = new();
                return source.Do(nextObject =>
                        serial.Disposable = nextObject is IDisposable nextDisposable ?
                            nextDisposable :
                            Disposable.Empty)
                    .SubscribeSafe(obs);
            });
        }

        public static IObservable<T> DisposeOnTermination<T>(this IObservable<T> source)
        {
            return Observable.Create<T>(obs =>
            {
                CompositeDisposable disposables = new();
                return source
                    .Do(nextObject =>
                    {
                        if (nextObject is IDisposable nextDisposable)
                            disposables.Add(nextDisposable);
                    })
                    .Finally(() => disposables.Dispose())
                    .SubscribeSafe(obs);
            });
        }

        public static IObservable<T> IgnoreTermination<T>(this IObservable<T> source)
        {
            return source.Concat(Observable.Never<T>());
        }

        public static IObservable<T> TakeUntil<T>(this IObservable<T> source, CancellationToken cancellationToken)
        {
            return source.TakeUntil(cancellationToken.Connect());
        }

        public static IObservable<T> RepeatUntil<T>(this IObservable<T> source, CancellationToken cancellationToken)
        {
            return source.Repeat().TakeUntil(cancellationToken);
        }
    }
}
