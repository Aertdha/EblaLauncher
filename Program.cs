using ElectronNET.API;
using ElectronNET.API.Entities;
using EblaLauncher.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IGameInstaller, GameInstaller>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (HybridSupport.IsElectronActive)
{
    Console.WriteLine("Electron is active, creating window...");
    await Task.Run(CreateElectronWindow);
}
else
{
    Console.WriteLine("Electron is not active!");
}

app.Run();

async Task CreateElectronWindow()
{
    try 
    {
        Console.WriteLine("Creating browser window...");
        var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
        {
            Width = 1200,
            Height = 800,
            Show = true,
            Frame = false,
            WebPreferences = new WebPreferences
            {
                NodeIntegration = true,
                ContextIsolation = false
            },
            Title = "EblaLauncher"
        });

        Console.WriteLine("Window created, setting up handlers...");

        browserWindow.OnReadyToShow += () => {
            Console.WriteLine("Window is ready to show");
            browserWindow.Show();
        };

        Electron.IpcMain.On("minimize", (args) => {
            browserWindow.Minimize();
        });

        Electron.IpcMain.On("maximize", async (args) => {
            if (await browserWindow.IsMaximizedAsync())
                browserWindow.Restore();
            else
                browserWindow.Maximize();
        });

        Electron.IpcMain.On("close", (args) => {
            browserWindow.Close();
        });

        browserWindow.OnClosed += () => {
            Electron.App.Quit();
        };

        Console.WriteLine("Window setup complete");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating window: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
} 