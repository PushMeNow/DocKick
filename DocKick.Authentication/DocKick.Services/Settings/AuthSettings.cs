using IdentityServer4.Models;

namespace DocKick.Services.Settings
{
    public record AuthSettings
    {
        public string Authority { get; set; }

        public Client[] Clients { get; set; }

        public string MetadataAddress { get; set; }
    }
}