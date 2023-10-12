namespace QuickRSS.Api.Models
{
	using System.ComponentModel.DataAnnotations;

	public class MarkFeedItemsModel
	{
		[Required(ErrorMessage = "There need to be some item ids!")]
		public string[] ItemIds { get; set; } = Array.Empty<string>();
	}
}
