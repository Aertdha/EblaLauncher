using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.Win32;
using System.Threading.Tasks;
using EblaLauncher.Services;
using EblaLauncher.Models;
using Microsoft.Extensions.Caching.Memory;

namespace EblaLauncher.Controllers
{
    // Контроллер для управления играми и их установкой
    public class HomeController : Controller
    {
        private readonly IGameInstaller _gameInstaller;
        private readonly IMemoryCache _cache;

        public HomeController(IGameInstaller gameInstaller, IMemoryCache cache)
        {
            _gameInstaller = gameInstaller;
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/api/games")]
        public IActionResult GetGames()
        {
            const string cacheKey = "games_list";
            
            if (!_cache.TryGetValue(cacheKey, out object? cachedGames))
            {
                var games = new[]
                {
                    new { id = 1, name = "Test Game 1", status = "installed" },
                    new { id = 2, name = "Test Game 2", status = "not_installed" }
                };
                
                _cache.Set(cacheKey, games, TimeSpan.FromMinutes(1));
                return Json(new { games });
            }

            return Json(new { games = cachedGames });
        }

        [Route("/api/install")]
        public async Task<IActionResult> InstallGame(string gameId)
        {
            try 
            {
                var installInfo = await _gameInstaller.AnalyzeTorrent(gameId);
                
                if (installInfo.HasSetup)
                {
                    // Запускаем установку через инсталлятор и отслеживаем прогресс через WebSocket
                    var installTask = _gameInstaller.InstallWithSetup(gameId, 
                        progress => NotifyProgress(gameId, progress));
                    
                    return Ok(new { status = "installing", needsSetup = true });
                }

                await _gameInstaller.ExtractFiles(gameId);
                return Ok(new { status = "ready", needsSetup = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Метод для отправки уведомлений о прогрессе установки через WebSocket
        // В будущих версиях будет реализована интеграция с SignalR
        private void NotifyProgress(string gameId, InstallProgress progress)
        {
            // TODO: Реализовать отправку уведомлений о прогрессе установки через WebSocket
        }
    }
}