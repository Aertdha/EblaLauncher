using System.Net.Http.Json;
using EblaLauncher.Models;

namespace EblaLauncher.Services
{
    public interface IGameUpdateService
    {
        Task<List<GameInfo>> CheckForUpdates();
        Task<Stream> DownloadFile(string fileId);
        Task StartUpdateLoop(CancellationToken cancellationToken);
    }

    public class GameUpdateService : IGameUpdateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private DateTime _lastCheck = DateTime.MinValue;

        public GameUpdateService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["UpdateService:ApiUrl"] 
                ?? throw new ArgumentNullException("UpdateService:ApiUrl not configured");
        }

        public async Task<List<GameInfo>> CheckForUpdates()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<GameInfo>>(
                    $"{_apiUrl}/updates?since={_lastCheck:yyyy-MM-ddTHH:mm:ssZ}");
                
                _lastCheck = DateTime.UtcNow;
                return response ?? new List<GameInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking updates: {ex.Message}");
                return new List<GameInfo>();
            }
        }

        public async Task<Stream> DownloadFile(string fileId)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/download/{fileId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task StartUpdateLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var updates = await CheckForUpdates();
                if (updates.Any())
                {
                    foreach (var game in updates)
                    {
                        Console.WriteLine($"New game found: {game.Name}");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }
    }
} 