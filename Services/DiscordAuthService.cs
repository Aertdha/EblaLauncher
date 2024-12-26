using System.Net.Http.Headers;
using System.Text.Json;
using EblaLauncher.Models;

namespace EblaLauncher.Services
{
    // Интерфейс сервиса авторизации через Discord
    public interface IDiscordAuthService
    {
        Task<string> GetAuthUrl();
        Task<DiscordUser> ValidateToken(string code);
        Task<bool> IsUserInGuild(string userId, string guildId);
    }

    // Сервис для авторизации через Discord OAuth2
    public class DiscordAuthService : IDiscordAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;
        private readonly string _requiredGuildId;

        public DiscordAuthService(IConfiguration configuration)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://discord.com/api/v10/")
            };

            _clientId = configuration["Discord:ClientId"] 
                ?? throw new ArgumentNullException("Discord:ClientId");
            _clientSecret = configuration["Discord:ClientSecret"] 
                ?? throw new ArgumentNullException("Discord:ClientSecret");
            _redirectUri = configuration["Discord:RedirectUri"] 
                ?? throw new ArgumentNullException("Discord:RedirectUri");
            _requiredGuildId = configuration["Discord:GuildId"] 
                ?? throw new ArgumentNullException("Discord:GuildId");
        }

        // Возвращает URL для авторизации через Discord
        public Task<string> GetAuthUrl()
        {
            var scopes = new[] { "identify", "guilds" };
            var url = $"https://discord.com/api/oauth2/authorize" +
                     $"?client_id={_clientId}" +
                     $"&redirect_uri={Uri.EscapeDataString(_redirectUri)}" +
                     $"&response_type=code" +
                     $"&scope={string.Join("%20", scopes)}";
            
            return Task.FromResult(url);
        }

        // Проверяет код авторизации и возвращает данные пользователя Discord
        public async Task<DiscordUser> ValidateToken(string code)
        {
            var tokenResponse = await _httpClient.PostAsync("oauth2/token", new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["client_id"] = _clientId,
                    ["client_secret"] = _clientSecret,
                    ["grant_type"] = "authorization_code",
                    ["code"] = code,
                    ["redirect_uri"] = _redirectUri
                }));

            var tokenData = await JsonSerializer.DeserializeAsync<JsonElement>(
                await tokenResponse.Content.ReadAsStreamAsync());
            var accessToken = tokenData.GetProperty("access_token").GetString();

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", accessToken);

            var userResponse = await _httpClient.GetAsync("users/@me");
            var userData = await JsonSerializer.DeserializeAsync<JsonElement>(
                await userResponse.Content.ReadAsStreamAsync());

            var user = new DiscordUser
            {
                Id = userData.GetProperty("id").GetString()!,
                Username = userData.GetProperty("username").GetString()!,
                Discriminator = userData.GetProperty("discriminator").GetString()!,
                AuthTime = DateTime.UtcNow
            };

            if (!await IsUserInGuild(user.Id, _requiredGuildId))
            {
                throw new UnauthorizedAccessException("User is not a member of required guild");
            }

            return user;
        }

        public async Task<bool> IsUserInGuild(string userId, string guildId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"users/@me/guilds");
                var guilds = await JsonSerializer.DeserializeAsync<JsonElement>(
                    await response.Content.ReadAsStreamAsync());

                foreach (var guild in guilds.EnumerateArray())
                {
                    if (guild.GetProperty("id").GetString() == guildId)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}