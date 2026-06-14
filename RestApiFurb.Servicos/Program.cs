using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestApiFurb.Dominio.Mediator.Modulos;
using RestApiFurb.Servicos.Consumidores;

System.Net.ServicePointManager.ServerCertificateValidationCallback =
    (sender, certificate, chain, sslPolicyErrors) => true;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddHostedService<ComandaCriadaConsumidor>();
        var connectionString = ctx.Configuration.GetConnectionString("DefaultConnection");
        services.AdicionarInfraEstrutura(connectionString);
    })
    .RunConsoleAsync();