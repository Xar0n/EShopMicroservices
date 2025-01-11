using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

var connectionString = builder.Configuration.GetConnectionString("Database")!;
builder.Services.AddMarten(o =>
{
    o.Connection(connectionString);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(o => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    { 
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
