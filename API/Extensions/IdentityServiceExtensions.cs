using API.Data;
using API.Entities;
using API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// Para JwtBearerDefaults
using Microsoft.AspNetCore.Authentication.JwtBearer;

// Para TokenValidationParameters y SymmetricSecurityKey
using Microsoft.IdentityModel.Tokens;

// Para Encoding.UTF8
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, 
        IConfiguration config)
        {
             services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Aquí reemplazamos "exception" minúscula por "Exception" mayúscula
                var tokenKey = config["TokenKey"] ?? throw new Exception("Token not found");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        return services;
        }
    }
}