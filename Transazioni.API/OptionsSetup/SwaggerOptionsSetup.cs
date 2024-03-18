using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Transazioni.API.OptionsSetup;

public class SwaggerOptionsSetup : IConfigureOptions<SwaggerGenOptions>
{
    private const string Scheme = "Bearer";
    private const string BearerFormat = "Jwt";

    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1",
            new OpenApiInfo { Title = "Transactions API", Version = "v1" });

        options.AddSecurityDefinition(Scheme, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = BearerFormat,
            Scheme = Scheme
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = Scheme
                    }
                },
                new string[]{}
            }
        });
    }
}
