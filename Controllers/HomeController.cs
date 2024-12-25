using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.Win32;
using System.Threading.Tasks;
using EblaLauncher.Services;
using EblaLauncher.Models;

namespace EblaLauncher.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameInstaller _gameInstaller;

        public HomeController(IGameInstaller gameInstaller)
        {
            _gameInstaller = gameInstaller;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/api/games")]
        public IActionResult GetGames()
        {
            return Json(new {
                games = new[]
                {
                    new { id = 1, name = "Test Game 1", status = "installed" },
                    new { id = 2, name = "Test Game 2", status = "not_installed" }
                }
            });
        }

        [Route("/api/install")]
        public async Task<IActionResult> InstallGame(string gameId)
        {
            try 
            {
                var installInfo = await _gameInstaller.AnalyzeTorrent(gameId);
                
                if (installInfo.HasSetup)
                {
                    // Запускаем длинный процесс установки
                    var installTask = _gameInstaller.InstallWithSetup(gameId, 
                        progress => NotifyProgress(gameId, progress));
                    
                    // Возвращаем OK сразу, статусы будут приходить через WebSocket/SignalR
                    return Ok(new { status = "installing", needsSetup = true });
                }
                else
                {
                    // Просто распаковываем файлы
                    await _gameInstaller.ExtractFiles(gameId);
                    return Ok(new { status = "ready", needsSetup = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private void NotifyProgress(string gameId, InstallProgress progress)
        {
            // Отправка статуса через WebSocket/SignalR
        }
    }
} 