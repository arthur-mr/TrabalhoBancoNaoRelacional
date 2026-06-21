using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

Directory.SetCurrentDirectory(AppContext.BaseDirectory);
var pathToContentRoot = AppContext.BaseDirectory;

await Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        Log.Logger.Information("Configurando host");
        configHost.SetBasePath(pathToContentRoot);
        configHost.AddEnvironmentVariables("ASPNETCORE_");
    })
    .ConfigureAppConfiguration((hostContext, configApp) =>
    {
        Log.Logger.Information("Configurando app config");
        configApp.SetBasePath(pathToContentRoot);
        configApp.AddJsonFile("appsettings.json", false, true);
        configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true, true);
        configApp.AddEnvironmentVariables("ASPNETCORE_");
    })
    .ConfigureServices((hostContext, services) =>
    {
        Log.Logger.Information("Instalando dependencias do serviço");
        services.AdicionarDependenciasServico(hostContext.Configuration);
    })
    .Build()
    .RunAsync();