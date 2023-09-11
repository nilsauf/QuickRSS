namespace QuickRSS.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class FeedSettingsModel
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }

        [Required(ErrorMessage = "Url is required")]
        [Url]
        public required string Url { get; set; }

        [Required(ErrorMessage = "RefreshRate in ms is required")]
        public double RefreshRateMilliseconds { get; set; }

        public int MaxItemCount { get; set; } = 25;
    }
}
