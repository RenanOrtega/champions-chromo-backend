using System.Text.Json.Serialization;
using ChampionsChromo.Application;
using ChampionsChromo.Core.Clients.Interfaces;
using ChampionsChromo.Core.Extensions;
using ChampionsChromo.Infrastructure;
using ChampionsChromo.Infrastructure.Clients;
using ChampionsChromo.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

builder.Services.Configure<AbacatePayOptions>(
    builder.Configuration.GetSection("AbacatePay")
);

builder.Services
    .AddHttpClient<IAbacatePayClient, AbacatePayClient>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddCore();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();

//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserIdentityBehaviorMiddleware<,>));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
//app.UseAuthorization();
app.MapControllers();

app.Run();