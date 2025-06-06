using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChampionsChromo.Api.Middlewares;
using ChampionsChromo.Application;
using ChampionsChromo.Core.Clients.Interfaces;
using ChampionsChromo.Core.Extensions;
using ChampionsChromo.Core.Options;
using ChampionsChromo.Infrastructure;
using ChampionsChromo.Infrastructure.Clients;
using ChampionsChromo.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.JwtOptionsKey)
);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration
        .GetSection(JwtOptions.JwtOptionsKey)
        .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["ACCESS_TOKEN"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

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
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .WithOrigins(
                "http://localhost:5173",
                "https://www.albumdefigurinha.com.br",
                "https://admin.albumdefigurinha.com.br",
                "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();