namespace QuickRSS.Logic
{
    using DynamicData;
    using QuickRSS.Entities;

    public interface IFeedProvider
    {
        IObservable<IChangeSet<Feed, Guid>> Connect();
    }
}
