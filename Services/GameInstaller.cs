using System;
using System.Threading.Tasks;
using EblaLauncher.Models;

namespace EblaLauncher.Services
{
    // Интерфейс для установки игр
    public interface IGameInstaller
    {
        Task<InstallInfo> AnalyzeTorrent(string gameId);
        Task ExtractFiles(string gameId);
        Task InstallWithSetup(string gameId, Action<InstallProgress> progressCallback);
    }

    // Реализация установщика игр
    public class GameInstaller : IGameInstaller
    {
        // Анализирует торрент файл игры и возвращает информацию об установке
        public async Task<InstallInfo> AnalyzeTorrent(string gameId)
        {
            return await Task.Run(() => new InstallInfo 
            { 
                HasSetup = true,
                SetupPath = $"Games/{gameId}/setup.exe",
                GamePath = $"Games/{gameId}"
            });
        }

        // Распаковывает файлы из торрента
        public async Task ExtractFiles(string gameId)
        {
            await Task.Run(() => {
                // TODO: Реализовать распаковку торрента
            });
        }

        // Запускает установку игры через setup.exe с отслеживанием прогресса
        public async Task InstallWithSetup(string gameId, Action<InstallProgress> progressCallback)
        {
            await Task.Run(() => {
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