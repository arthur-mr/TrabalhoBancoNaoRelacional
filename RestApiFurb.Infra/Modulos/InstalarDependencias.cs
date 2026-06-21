using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Infra.Contextos;
using RestApiFurb.Infra.Repositorio;

namespace RestApiFurb.Infra.Modulos;

public static class InstalarDependencias
{
    public static IServiceCollection AdicionarBancoDeDados(this IServiceCollection services, IConfiguration configuracoes)
    {
        var connectionString = configuracoes.GetConnectionString("DefaultConnection");
        services.AddDbContext<Contexto>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IRepositorioBase, RepositorioBase>();

        return services;
    }
}