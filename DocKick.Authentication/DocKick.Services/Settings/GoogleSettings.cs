namespace DocKick.Services.Settings
{
    public record GoogleSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}