using ElectronNET.API;
using ElectronNET.API.Entities;
using EblaLauncher.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);

// Регистрация базовых сервисов
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

// Регистрация сервисов приложения
builder.Services.AddScoped<IGameInstaller, GameInstaller>();
builder.Services.AddScoped<IDiscordAuthService, DiscordAuthService>();
builder.Services.AddSingleton<IUserDataService, UserDataService>();
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

var app = builder.Build();

// Настройка CSP для безопасности приложения
app.Use(async (context, next) =>
{
    context.Response.Headers.Append(
        "Content-Security-Policy",
        "default-src 'self' https://discord.com; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (HybridSupport.IsElectronActive)
{
    await Task.Run(async () => {
        var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
        {
            Width = 1200,
            Height = 800,
            Show = true,
            Frame = false,
            WebPreferences = new WebPreferences
            {
                NodeIntegration = true,
                ContextIsolation = false,
                WebSecurity = true,
                AllowRunningInsecureContent = false
            },
            Title = "EblaLauncher"
        });

        browserWindow.OnReadyToShow += () => browserWindow.Show();

        // Обработка IPC сообщений управления окном
        Electron.IpcMain.On("minimize", (args) => browserWindow.Minimize());
        Electron.IpcMain.On("maximize", async (args) => {
            if (await browserWindow.IsMaximizedAsync())
                browserWindow.Restore();
            else
                browserWindow.Maximize();
        });
        Electron.IpcMain.On("close", (args) => browserWindow.Close());

        browserWindow.OnClosed += () => Electron.App.Quit();
    });
}

app.Run(); 