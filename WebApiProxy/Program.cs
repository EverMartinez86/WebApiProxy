using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using WebApiProxy.Business;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

#pragma warning disable S1075 // URIs should not be hardcoded
    string uriString = "https://www.mipagina.com";
#pragma warning restore S1075 // URIs should not be hardcoded
    builder.Services.AddSwaggerGen(options =>
    {
        options.ExampleFilters();

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Proxy Api",
            Description = "",
            TermsOfService = new Uri(uriString),
            Contact = new OpenApiContact
            {
                Name = "Contact the developer",
                Url = new Uri(uriString)
            },
            License = new OpenApiLicense
            {
                Name = "License",
                Url = new Uri(uriString)
            }
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
            new string[]{}
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    });

    builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddDependencyBusinessLayer(builder.Configuration);

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Error inicializando program.cs");
    throw;
}
finally 
{
    LogManager.Shutdown();
}










