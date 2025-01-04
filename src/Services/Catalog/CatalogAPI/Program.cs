using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);


var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddMarten(o =>
{
    o.Connection(connectionString!);
}).UseLightweightSessions();

var app = builder.Build();

app.MapCarter();
app.Run();
