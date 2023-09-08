namespace QuickRSS.Database.Feed
{
    using System;

    public interface IFeedChangeNotifier
    {
        IObservable<Guid> Changed { get; }
        IObserver<Guid> Notify { get; }
    }
}
