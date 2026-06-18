using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PruebaCCL.Backend.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4200/")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Entity Framework Core with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddAuthorization();

// Add OpenAPI/Swagger
// builder.Services.AddOpenApi();

var app = builder.Build();

// Poblar la base de datos con datos iniciales si está vacía
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Productos.Any())
    {
        context.Productos.AddRange(
            new PruebaCCL.Backend.Models.Producto { Nombre = "Portátil HP", Cantidad = 10 },
            new PruebaCCL.Backend.Models.Producto { Nombre = "Mouse Inalámbrico", Cantidad = 50 },
            new PruebaCCL.Backend.Models.Producto { Nombre = "Teclado Mecánico", Cantidad = 25 },
            new PruebaCCL.Backend.Models.Producto { Nombre = "Monitor 24 pulgadas", Cantidad = 15 }
        );
        context.SaveChanges();
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
}
// Evitar redirección a HTTPS en local para no tener problemas con certificados auto-firmados
// app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowFrontend");

// Use Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Global protection: Require authorization for all controllers by default
app.MapControllers().RequireAuthorization();

app.Run();
