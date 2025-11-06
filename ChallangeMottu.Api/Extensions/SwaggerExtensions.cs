using Microsoft.OpenApi.Models;
using ChallangeMottu.Application.Configs;

namespace ChallangeMottu.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, Settings settings)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = settings.Swagger.Title,
                Description = settings.Swagger.Description,
                Contact = settings.Swagger.Contact
            });

            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = settings.SwaggerV2.Title,
                Description = settings.SwaggerV2.Description,
                Contact = settings.SwaggerV2.Contact
            });
                
        });

        return services;
    }
}