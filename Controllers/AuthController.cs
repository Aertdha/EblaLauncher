using Microsoft.AspNetCore.Mvc;
using EblaLauncher.Services;
using IOFile = System.IO.File;

namespace EblaLauncher.Controllers
{
    /// Контроллер для управления аутентификацией через Discord
    public class AuthController : Controller
    {
        private readonly IDiscordAuthService _discordAuth;
        private readonly IUserDataService _userData;

        public AuthController(
            IDiscordAuthService discordAuth,
            IUserDataService userData)
        {
            _discordAuth = discordAuth;
            _userData = userData;
        }

        /// Инициирует процесс аутентификации через Discord
        [HttpGet("/auth/login")]
        public async Task<IActionResult> Login()
        {
            var authUrl = await _discordAuth.GetAuthUrl();
            return Redirect(authUrl);
        }

        /// Обрабатывает ответ от Discord после успешной аутентификации
        [HttpGet("/auth/callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            try
            {
                var user = await _discordAuth.ValidateToken(code);
                await _userData.SaveUserData(user);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Выполняет выход пользователя путем удаления локальных данных
        [HttpPost("/auth/logout")]
        public IActionResult Logout()
        {
            try
            {
                var path = Path.Combine(_userData.UserDataPath, "user.json");
                if (IOFile.Exists(path))
                    IOFile.Delete(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}