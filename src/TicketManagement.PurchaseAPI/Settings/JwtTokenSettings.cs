namespace TicketManagement.PurchaseAPI.Settings
{
    /// <summary>
    /// Provides properties for jwt token settings.
    /// </summary>
    public class JwtTokenSettings
	{
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
