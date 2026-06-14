using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiFurb.ImportadorDados.Importador;
using RestApiFurb.Infra.Modulos;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== INICIANDO SCRIPT DE CARGA DE DADOS ===");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Erro: A ConnectionString 'DefaultConnection' não foi encontrada no appsettings.json.");
            Console.ResetColor();
            return;
        }

        var services = new ServiceCollection();

        services.AdicionarBancoDeDados(connectionString);
        services.AddScoped<ProdutoImportador>();
        var serviceProvider = services.BuildServiceProvider();

        var importador = serviceProvider.GetRequiredService<ProdutoImportador>();

        try
        {
            await importador.AdicionarProdutosEmMassaAsync(default);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== PROCESSO FINALIZADO COM SUCESSO ===");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erro fatal durante a execução: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Detalhes internos: {ex.InnerException.Message}");
            }
            Console.ResetColor();
        }

        Console.WriteLine("Pressione qualquer tecla para fechar...");
        Console.ReadKey();
    }
}