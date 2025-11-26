using FileUploadAPI.Api.Configuration;
using FileUploadAPI.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var app = CreateWebApplication(args);
await ConfigureAndRunApp(app);

WebApplication CreateWebApplication(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    LoggingConfiguration.ConfigureSerilog(builder.Configuration, builder.Environment);
    builder.Host.UseSerilog();

    // Health checks
    builder.Services.AddHealthChecks()
        .AddCheck<LivenessHealthCheck>("self");

    // Auth services
    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization();

    // Add services to the container.
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddProblemDetails();
    builder.Services.AddOpenApi();

    return builder.Build();
}
Task ConfigureAndRunApp(WebApplication app)
{
    app.UseExceptionHandler();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = r => r.Name.Contains("liveness"),
    });

    return app.RunAsync();
}