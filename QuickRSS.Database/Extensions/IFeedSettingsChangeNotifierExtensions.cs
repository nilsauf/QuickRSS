namespace QuickRSS.Database.Extensions
{
    using QuickRSS.Database.Feed;
    using System;

    public static class IFeedSettingsChangeNotifierExtensions
    {
        public static void Notify(this IFeedChangeNotifier notifier, Guid id)
        {
            if (notifier is null)
                throw new ArgumentNullException(nameof(notifier));

            notifier.Notify.OnNext(id);
        }
    }
}
