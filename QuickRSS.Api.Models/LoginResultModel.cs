namespace QuickRSS.Api.Models
{
    using System;

    public class LoginResultModel
    {
        public required string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
