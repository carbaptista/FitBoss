using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Api.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "FitBoss",
                Description = "An ASP.NET Core Web API for a gym's employees and clients - Uma API ASP.NET Core para o gerenciamento de funcionários e alunos de uma academia",
                Contact = new OpenApiContact
                {
                    Name = "Contact",
                    Url = new Uri("https://carbap.xyz")
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT Token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                        },
                        Name = "Bearer",
                    },
                    new List<string>()
                }
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

}
