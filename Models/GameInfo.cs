namespace EblaLauncher.Models
{
    public class GameInfo
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public required string TorrentUrl { get; set; }
        public required string SteamFixUrl { get; set; }
        public string? Version { get; set; }
        public DateTime Added { get; set; }
        public long Size { get; set; }
        public Dictionary<string, string>? Requirements { get; set; }
        public string? SteamAppId { get; set; }
        public bool RequiresSteamEmulator { get; set; }
    }
} 