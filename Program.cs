using ElectronNET.API;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Electron конфигурация
if (HybridSupport.IsElectronActive)
{
    CreateElectronWindow();
}

Electron.IpcMain.On("get-games", async (args) => {
    var games = new[] {
        new { id = 1, name = "Game 1", status = "installed" }
    };
    await Electron.MainWindow.WebContents.SendAsync("games-list", games);
});

app.Run();

async void CreateElectronWindow()
{
    var window = await Electron.WindowManager.CreateWindowAsync();
    window.OnClosed += () => {
        Electron.App.Quit();
    };
} 