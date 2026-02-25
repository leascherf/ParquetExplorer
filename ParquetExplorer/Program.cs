using Microsoft.Extensions.DependencyInjection;
using ParquetExplorer.Services;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();
        ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();
        var mainForm = serviceProvider.GetRequiredService<MainForm>();
        Application.Run(mainForm);
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // Register services against their interfaces (Dependency Inversion Principle)
        services.AddSingleton<IParquetService, ParquetService>();
        services.AddSingleton<ICompareService, CompareService>();
        services.AddSingleton<IExplorerService, ExplorerService>();
        services.AddSingleton<IAzureBlobService, AzureBlobService>();

        // Register forms as transient so each resolve creates a fresh instance
        services.AddTransient<MainForm>();
    }
}