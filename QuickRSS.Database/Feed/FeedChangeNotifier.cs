namespace QuickRSS.Database.Feed
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public class FeedChangeNotifier : IFeedChangeNotifier, IDisposable
    {
        private readonly Subject<Guid> signalSubject = new Subject<Guid>();

        public IObservable<Guid> Changed => signalSubject.AsObservable();

        public IObserver<Guid> Notify => signalSubject.AsObserver();

        public void Dispose() => signalSubject.Dispose();
    }
}
