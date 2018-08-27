using AngularSPA.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using AngularSPA.Auth.Helpers;

namespace AngularSPA.Extensions
{
    public static class JwtServiceCollectionExtensions 
    {
        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtOptions.IssuerOptions.Issuer;
                options.Audience = jwtOptions.IssuerOptions.Audience;
                options.SigningCredentials = new SigningCredentials(key: signingKey, algorithm: SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.IssuerOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.IssuerOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
                {
                    configureOptions.ClaimsIssuer = jwtOptions.IssuerOptions.Issuer;
                    configureOptions.TokenValidationParameters = tokenValidationParameters;
                    configureOptions.SaveToken = true;
                }
            );

            return services;
        }
    }
}
