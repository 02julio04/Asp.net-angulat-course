using API.Data;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

// Para JwtBearerDefaults
using Microsoft.AspNetCore.Authentication.JwtBearer;

// Para TokenValidationParameters y SymmetricSecurityKey
using Microsoft.IdentityModel.Tokens;

// Para Encoding.UTF8
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Aquí reemplazamos "exception" minúscula por "Exception" mayúscula
        var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token not found");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

var app = builder.Build();

// Configurar el pipeline HTTP
app.UseCors(x => x.AllowAnyHeader()
                   .AllowAnyMethod()
                   .WithOrigins("http://localhost:4200/", "https://localhost:4200/"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
