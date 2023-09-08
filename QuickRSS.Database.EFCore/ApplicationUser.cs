namespace QuickRSS.Database.EFCore
{
    using Microsoft.AspNetCore.Identity;
    using QuickRSS.Entities;

    public class ApplicationUser : IdentityUser
    {
        private string? firstName;
        private string? lastName;

        public string? FirstName
        {
            get => this.firstName;
            set
            {
                this.firstName = value;
                this.QuickRssUser.FirstName = value;
            }
        }

        public string? LastName
        {
            get => this.lastName;
            set
            {
                this.lastName = value;
                this.QuickRssUser.LastName = value;
            }
        }

        public override string? Email
        {
            get => base.Email;
            set
            {
                base.Email = value;
                this.QuickRssUser.Email = value;
            }
        }

        public QuickRssUser QuickRssUser { get; set; } = new QuickRssUser();
    }
}
