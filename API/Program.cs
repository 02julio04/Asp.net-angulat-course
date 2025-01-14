using API.Data;
using API.Entities;
using API.Interfaces;
using API.Services;
using API.Extensions;
using Microsoft.EntityFrameworkCore;

// Para JwtBearerDefaults
using Microsoft.AspNetCore.Authentication.JwtBearer;

// Para TokenValidationParameters y SymmetricSecurityKey
using Microsoft.IdentityModel.Tokens;

// Para Encoding.UTF8
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

// Configurar el pipeline HTTP
app.UseCors(x => x.AllowAnyHeader()
                   .AllowAnyMethod()
                   .WithOrigins("http://localhost:4200/", "https://localhost:4200/"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
