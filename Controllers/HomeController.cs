using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace EblaLauncher.Controllers
{
    public class HomeController : Controller
    {
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
            // Прямой доступ к файловой системе
            var gamePath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles), "Games", gameId);
            
            // Работа с реестром Windows
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Games"))
            {
                // ...
            }
            
            return Ok();
        }
    }
} 