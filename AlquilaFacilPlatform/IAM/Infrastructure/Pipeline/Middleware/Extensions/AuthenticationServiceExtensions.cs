using System.Text;
using AlquilaFacilPlatform.IAM.Infrastructure.Tokens.JWT.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AlquilaFacilPlatform.IAM.Infrastructure.Pipeline.Middleware.Extensions;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenSettings = configuration.GetSection("TokenSettings").Get<TokenSettings>();
        var key = Encoding.ASCII.GetBytes(tokenSettings!.Secret);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Cambiar a true en producción
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                // Added to handle SignalR connections
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        // Verifies if the access token is present in the query string
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments(SignalRRoutes.ReadingHub))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}
