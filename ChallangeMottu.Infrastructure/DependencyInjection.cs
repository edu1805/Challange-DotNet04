using ChallangeMottu.Domain.Interfaces;
using ChallangeMottu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChallangeMottu.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ChallangeMottuContext>(options =>
        {
            var conn = configuration.GetConnectionString("OracleConnection");
            options.UseOracle(conn); 
        });
        
        services.AddScoped<IMotoRepository, MotoRepository>();
        services.AddScoped<ILocalizacaoAtualRepository, LocalizacaoAtualRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}