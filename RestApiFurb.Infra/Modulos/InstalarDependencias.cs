using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Infra.Contextos;
using RestApiFurb.Infra.Repositorio;

namespace RestApiFurb.Infra.Modulos;

public static class InstalarDependencias
{
    public static IServiceCollection AdicionarBancoDeDados(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Contexto>(options => options.UseSqlServer(connectionString));
        services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));

        return services;
    }
}