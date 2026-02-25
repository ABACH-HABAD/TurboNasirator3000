using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using TurboNasiratorClasses.Computations;
using TurboNasiratorClasses.RandomElements;
using TurboNasiratorClasses.Create;
using TurboNasiratorClasses.Delete;
using TurboNasiratorClasses.Pack;
using TurboNasiratorClasses.Unpack;

namespace TurboNasirator3000;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    private static IServiceProvider services = null!;
    public static IServiceProvider Services { get => services; set => services = value; }

    public App()
    {
        _host = Host.CreateDefaultBuilder().ConfigureAppConfiguration((context, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            string basePath = AppContext.BaseDirectory;

        }).ConfigureServices((context, services) =>
        {
            services.AddSingleton<Random>();

            services.AddTransient<INameGenerator, NameGeneratorService>();
            services.AddTransient<IKeyGenerator, KeyGeneratorService>();

            services.AddTransient<IProgression, GeometricProgression>();
            services.AddTransient<IProgressCalculation, ProgressCalculationService>();

            services.AddTransient<ICreateFolder, CreateFolderService>();
            services.AddTransient<ICreate, CreateService>();
            services.AddTransient<IDeleteFolder, DeleteFolderService>();
            services.AddTransient<IDelete, DeleteService>();
            services.AddTransient<IPack, PackService>();
            services.AddTransient<IUnpack, UnpackService>();

            services.AddSingleton<MainWindow>();

        }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        Services = _host.Services;

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
}

