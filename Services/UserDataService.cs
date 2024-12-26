using System.Text.Json;
using EblaLauncher.Models;
using EblaLauncher.Services;

namespace EblaLauncher.Services
{
    public interface IUserDataService
    {
        string UserDataPath { get; }
        Task SaveUserData(DiscordUser? user);
        Task<DiscordUser?> LoadUserData();
        Task SaveInstalledGames(List<string> gameIds);
        Task<List<string>> GetInstalledGames();
    }

    public class UserDataService : IUserDataService
    {
        private readonly string _baseDataPath;
        private readonly IEncryptionService _encryption;
        public string UserDataPath { get; }

        public UserDataService(IEncryptionService encryption)
        {
            _encryption = encryption;
            // %APPDATA%/EblaLauncher
            _baseDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "EblaLauncher"
            );
            
            UserDataPath = Path.Combine(_baseDataPath, "userdata");
            
            // Создаем директории если не существуют
            Directory.CreateDirectory(_baseDataPath);
            Directory.CreateDirectory(UserDataPath);
        }

        public async Task SaveUserData(DiscordUser? user)
        {
            var path = Path.Combine(UserDataPath, "user.json");
            if (user == null)
            {
                // Если null - удаляем файл
                if (File.Exists(path))
                    File.Delete(path);
                return;
            }

            var json = JsonSerializer.Serialize(user);
            var encrypted = _encryption.Encrypt(json);
            await File.WriteAllTextAsync(path, encrypted);
        }

        public async Task<DiscordUser?> LoadUserData()
        {
            var path = Path.Combine(UserDataPath, "user.json");
            if (!File.Exists(path)) return null;

            try 
            {
                var encrypted = await File.ReadAllTextAsync(path);
                var json = _encryption.Decrypt(encrypted);
                return JsonSerializer.Deserialize<DiscordUser>(json);
            }
            catch
            {
                return null;
            }
        }

        public async Task SaveInstalledGames(List<string> gameIds)
        {
            var path = Path.Combine(UserDataPath, "installed_games.json");
            await File.WriteAllTextAsync(path, 
                JsonSerializer.Serialize(gameIds, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                }));
        }

        public async Task<List<string>> GetInstalledGames()
        {
            var path = Path.Combine(UserDataPath, "installed_games.json");
            if (!File.Exists(path)) return new List<string>();

            var json = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
    }
} 