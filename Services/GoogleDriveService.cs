using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using EblaLauncher.Models;

namespace EblaLauncher.Services
{
    public interface IGoogleDriveService
    {
        Task<List<GameInfo>> CheckForUpdates();
        Task<Stream> DownloadFile(string fileId);
        Task StartUpdateLoop(CancellationToken cancellationToken);
    }

    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly string _updatesFolderId;
        private DateTime _lastCheck = DateTime.MinValue;
        
        public GoogleDriveService(IConfiguration configuration)
        {
            // Инициализация Google Drive API
            var credential = GoogleCredential.FromFile("credentials.json")
                .CreateScoped(DriveService.ScopeConstants.DriveReadonly);

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });

            _updatesFolderId = configuration["GoogleDrive:UpdatesFolderId"] 
                ?? throw new ArgumentNullException(
                    nameof(configuration), 
                    "GoogleDrive:UpdatesFolderId is not configured");
        }

        public async Task<List<GameInfo>> CheckForUpdates()
        {
            var updates = new List<GameInfo>();
            
            try
            {
                var request = _driveService.Files.List();
                request.Q = $"'{_updatesFolderId}' in parents and name = 'update.json' and modifiedTime > '{_lastCheck:yyyy-MM-ddTHH:mm:ssZ}'";
                
                var files = await request.ExecuteAsync();
                
                foreach (var file in files.Files)
                {
                    using var stream = await DownloadFile(file.Id);
                    using var reader = new StreamReader(stream);
                    var json = await reader.ReadToEndAsync();
                    var gameInfo = System.Text.Json.JsonSerializer.Deserialize<GameInfo>(json);
                    
                    if (gameInfo != null)
                    {
                        updates.Add(gameInfo);
                    }
                }

                _lastCheck = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking updates: {ex.Message}");
            }

            return updates;
        }

        public async Task<Stream> DownloadFile(string fileId)
        {
            var request = _driveService.Files.Get(fileId);
            var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            stream.Position = 0;
            return stream;
        }

        public async Task StartUpdateLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var updates = await CheckForUpdates();
                if (updates.Any())
                {
                    // Уведомляем систему о новых играх
                    foreach (var game in updates)
                    {
                        Console.WriteLine($"New game found: {game.Name}");
                        // TODO: Добавить в базу/уведомить UI
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }
    }
} 