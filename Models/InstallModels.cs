namespace EblaLauncher.Models
{
    public class InstallInfo
    {
        public bool HasSetup { get; set; }
        public string? SetupPath { get; set; }
        public required string GamePath { get; set; }
    }

    public class InstallProgress
    {
        public int Percentage { get; set; }
        public required string Status { get; set; }
        public string? CurrentFile { get; set; }
    }
} 