namespace QuickRSS.Logic.Extensions
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;

    public static class CancellationTokenExtensions
    {
        public static IObservable<Unit> Connect(this CancellationToken token)
        {
            return Observable.Create<Unit>(obs => token.Register(() =>
            {
                obs.OnNext(Unit.Default);
                obs.OnCompleted();
            }));
        }
    }
}
