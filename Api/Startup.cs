using Api.Application;
using Api.Filters;
using Api.Infrastructure;
using Api.Middlewares;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Api;


public class Startup
{
    public Startup(ConfigurationManager configuration)
    {
        Configuration = configuration;
    }
    public ConfigurationManager Configuration { get; set; }

    public void RegisterServices(IServiceCollection services)
    {
        // services.Configure<AzureAdClientSettings>(Configuration.GetSection("AzureAdClientSettings"));


        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilters>();
        }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

        services.AddHttpContextAccessor();
        services.AddInfrastructure(Configuration);
        services.AddApplication();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SDSConnectors.Api", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        services.AddCors(options =>
        {
            options.AddPolicy("DevPolicy",
                builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    builder.SetIsOriginAllowed(x => true);
                });
        });
    }
    public void SetupMiddlewares(WebApplication app)
    {
        app.UseCors("DevPolicy");
        app.UseMiddleware<UnauthorizedMiddleware>();
    }
}
