namespace QuickRSS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QuickRssUser : IEquatable<QuickRssUser>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public IList<Feed> Feeds { get; set; } = Enumerable.Empty<Feed>().ToList();

        public bool Equals(QuickRssUser? other)
        {
            return other is not null && this.Id.Equals(other.Id);
        }
    }
}
