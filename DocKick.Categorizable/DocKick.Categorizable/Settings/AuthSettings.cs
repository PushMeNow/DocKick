namespace DocKick.Categorizable.Settings
{
    public record AuthSettings
    {
        public string Authority { get; set; }
        public string MetadataAddress { get; set; }
    }
}