using System.Reflection;
using ChallangeMottu.Api.Extensions;
using ChallangeMottu.Application;
using ChallangeMottu.Application.Configs;
using ChallangeMottu.Infrastructure;
using ChallangeMottu.Application.Mappings;
using ChallangeMottu.Application.Validators;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

var configs = builder.Configuration.Get<Settings>();
builder.Services.AddSingleton(configs);

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreateMotoDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUsuarioDtoValidator>();
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddVersioning();
// Swagger
builder.Services.AddSwagger(configs);

// AutoMapper
builder.Services.AddAutoMapper(typeof(MotoProfile), typeof(LocalizacaoAtualProfile), typeof(UsuarioProfile));

// --- Registrar camadas via DI ---
builder.Services.AddInfrastructure(builder.Configuration); // DbContext + Repositories
builder.Services.AddApplication(); // Services (MotoService, LocalizacaoAtualService, etc.)

// HealthCheck
builder.Services.AddHealthCheckConfiguration(builder.Configuration);

var app = builder.Build();

// ------------------------------
// Health Check Endpoints
// ------------------------------
app.MapHealthChecks("/health");
app.MapHealthChecks("/health-details", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json",  "MotoMap.API v1");
        ui.SwaggerEndpoint("/swagger/v2/swagger.json",  "MotoMap.API v2");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();