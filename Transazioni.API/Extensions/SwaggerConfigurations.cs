using Transazioni.API.OptionsSetup;

namespace Transazioni.API.Extensions;

public static class SwaggerConfigurations
{
    /// <summary>
    /// Swagger configurations
    /// </summary>
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureOptions<SwaggerOptionsSetup>();
    }
}
