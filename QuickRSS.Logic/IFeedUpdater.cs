namespace QuickRSS.Logic
{
    using QuickRSS.Entities;
    using System.Reactive;

    public interface IFeedUpdater
    {
        ValueTask<Unit> UpdateFeedAsync(Feed feed);
    }
}
