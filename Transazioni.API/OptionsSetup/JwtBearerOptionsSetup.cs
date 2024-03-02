using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Transazioni.API.Costants;
using Transazioni.Infrastructure.Authentication.Jwt;

namespace Transazioni.API.OptionsSetup;

public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions;

    public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        CheckOptions();
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
        };

        options.Events ??= new JwtBearerEvents();

        options.Events.OnMessageReceived = context =>
        {

            if (context.Request.Cookies.ContainsKey(CookieNames.AccessToken))
            {
                context.Token = context.Request.Cookies[CookieNames.AccessToken];
            }

            return Task.CompletedTask;
        };
    }

    private void CheckOptions()
    {
        if (_jwtOptions is null)
        {
            throw new ArgumentNullException(nameof(JwtOptions));
        }

        if (string.IsNullOrWhiteSpace(_jwtOptions.Audience))
        {
            throw new ArgumentNullException(nameof(JwtOptions.Audience));
        }

        if (string.IsNullOrWhiteSpace(_jwtOptions.Issuer))
        {
            throw new ArgumentNullException(nameof(JwtOptions.Issuer));
        }

        if (string.IsNullOrWhiteSpace(_jwtOptions.SecretKey))
        {
            throw new ArgumentNullException(nameof(JwtOptions.SecretKey));
        }
    }
}
