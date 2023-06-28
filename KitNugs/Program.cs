using System.Security.Cryptography.X509Certificates;
using System.Text;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Client;
using KitNugs.Configuration;
using KitNugs.Logging;
using KitNugs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Configure logging - we use serilog.
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
var serviceConfiguration = new ServiceConfiguration(builder.Configuration);

builder.Services.AddScoped<IServiceConfiguration, ServiceConfiguration>();
builder.Services.AddScoped<IUserService, UserServiceKeycloak>();
builder.Services.AddScoped<ISessionIdAccessor, DefaultSessionIdAccessor>();
builder.Services.AddScoped<IUsersApi>((sp) =>
{
    var conf = sp.GetService<IServiceConfiguration>();
    var httpClient = new CustomKeycloakHttpClient(
        conf.GetConfigurationValue(ConfigurationVariables.AuthServerUrl),
        conf.GetConfigurationValue(ConfigurationVariables.RealmToManage),
        conf.GetConfigurationValue(ConfigurationVariables.ClientId),
        conf.GetConfigurationValue(ConfigurationVariables.GrantType),
        conf.GetConfigurationValue(ConfigurationVariables.ClientSecret)
    );

    var usersApi = new UsersApi(
            httpClient,
            new Configuration { BasePath = $"{httpClient.AuthServerUrl}/admin/realms" },
            null
        );

    return usersApi;
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
        });
});



if (serviceConfiguration.GetConfigurationValue(ConfigurationVariables.TokenValidation) != "false")
{
    Console.WriteLine("Token validation is used!");

    var rawCert = Convert.FromBase64String(serviceConfiguration.GetConfigurationValue(ConfigurationVariables.IssuerCertificate));
    var cert = new X509Certificate2(rawCert);

    builder.Services
        .AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.IncludeErrorDetails = true;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = serviceConfiguration.GetConfigurationValue(ConfigurationVariables.AllowedIssuer),
                ValidAudience = serviceConfiguration.GetConfigurationValue(ConfigurationVariables.AllowedAudience),
                IssuerSigningKey = new X509SecurityKey(cert)
            };
            opt.Validate();
        });
}
builder.Services.AddHttpContextAccessor();

// Add controllers
builder.Services.AddControllers();

// Enable NSwag
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument();

// Setup health checks and Prometheus endpoint
builder.Services.AddHealthChecks()
                .ForwardToPrometheus();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<LogHeaderMiddleware>();

app.UseHttpMetrics();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
app.UseAuthentication();

app.UseAuthorization();

// Map controllers
app.MapControllers();

// Ensure health endpoint and Prometheus only respond on port 8081
app.UseHealthChecks("/healthz", 8081);
app.UseMetricServer(8081, "/metrics");

using (var scope = app.Services.CreateScope())
{
    // Ensure all env variables is set.
    scope.ServiceProvider.GetRequiredService<IServiceConfiguration>();
}

app.Run();


public partial class Program { }
