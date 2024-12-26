namespace EblaLauncher.Models
{
    public class DiscordUser
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Discriminator { get; set; }
        public string? AvatarUrl { get; set; }
        public List<string> GuildIds { get; set; } = new();
        public DateTime AuthTime { get; set; }
    }
} 