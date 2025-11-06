namespace ChallangeMottu.Api.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddOracle(
                configuration.GetConnectionString("OracleConnection")!,
                name: "oracle-database",
                tags: new[] { "database" }
            );

        return services;
    }
}