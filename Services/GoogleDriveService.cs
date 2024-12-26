using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using EblaLauncher.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace EblaLauncher.Services
{
    /// Сервис для взаимодействия с Google Drive API, обеспечивающий функционал обновления игр
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
        private readonly IMemoryCache _cache;
        private DateTime _lastCheck = DateTime.MinValue;
        
        public GoogleDriveService(IConfiguration configuration, IMemoryCache cache)
        {
            _cache = cache;
            var credential = GoogleCredential.FromFile("credentials.json")
                .CreateScoped(DriveService.ScopeConstants.DriveReadonly);

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "EblaLauncher"
            });

            _updatesFolderId = configuration["GoogleDrive:UpdatesFolderId"] 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// Выполняет проверку обновлений игр в указанной директории Google Drive,
        /// анализируя файлы update.json, измененные после последней проверки
        public async Task<List<GameInfo>> CheckForUpdates()
        {
            const string cacheKey = "game_updates";
            
            if (_cache.TryGetValue(cacheKey, out List<GameInfo>? cachedUpdates))
            {
                return cachedUpdates ?? new List<GameInfo>();
            }

            var updates = new List<GameInfo>();
            try
            {
                var request = _driveService.Files.List();
                request.Q = $"'{_updatesFolderId}' in parents and name = 'update.json'";
                request.Fields = "files(id,name,modifiedTime)";

                var files = await request.ExecuteAsync();
                foreach (var file in files.Files)
                {
                    using var stream = await DownloadFile(file.Id);
                    var gameInfo = await JsonSerializer.DeserializeAsync<GameInfo>(stream);
                    if (gameInfo != null)
                    {
                        updates.Add(gameInfo);
                    }
                }

                _cache.Set(cacheKey, updates, TimeSpan.FromMinutes(5));
                _lastCheck = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking updates: {ex.Message}");
            }

            return updates;
        }

        /// Загружает файл из Google Drive по указанному идентификатору
        /// и возвращает его в виде потока данных
        public async Task<Stream> DownloadFile(string fileId)
        {
            var request = _driveService.Files.Get(fileId);
            var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            stream.Position = 0;
            return stream;
        }

        /// Запускает бесконечный цикл проверки обновлений с 5-минутным интервалом.
        /// Цикл может быть прерван через cancellationToken
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