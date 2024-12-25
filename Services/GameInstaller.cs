using System;
using System.Threading.Tasks;
using EblaLauncher.Models;

namespace EblaLauncher.Services
{
    public interface IGameInstaller
    {
        Task<InstallInfo> AnalyzeTorrent(string gameId);
        Task ExtractFiles(string gameId);
        Task InstallWithSetup(string gameId, Action<InstallProgress> progressCallback);
    }

    public class GameInstaller : IGameInstaller
    {
        public async Task<InstallInfo> AnalyzeTorrent(string gameId)
        {
            return await Task.Run(() => new InstallInfo 
            { 
                HasSetup = true, // Здесь будет реальная проверка
                SetupPath = $"Games/{gameId}/setup.exe",
                GamePath = $"Games/{gameId}"
            });
        }

        public async Task ExtractFiles(string gameId)
        {
            await Task.Run(() => {
                // Логика распаковки торрента
            });
        }

        public async Task InstallWithSetup(string gameId, Action<InstallProgress> progressCallback)
        {
            await Task.Run(() => {
                // Имитация процесса установки
                for (int i = 0; i <= 100; i += 10)
                {
                    progressCallback(new InstallProgress 
                    { 
                        Percentage = i,
                        Status = i < 100 ? "installing" : "completed",
                        CurrentFile = $"file_{i}.dat"
                    });
                    Task.Delay(1000).Wait();
                }
            });
        }
    }
} 