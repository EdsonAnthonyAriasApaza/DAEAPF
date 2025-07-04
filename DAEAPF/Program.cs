using AspNetCoreRateLimit;
using DAEAPF.Application.Interfaces.Services.Negocios;
using DAEAPF.Application.Interfaces.Services.Productos;
using DAEAPF.Application.Interfaces.Services.Usuarios;
using DAEAPF.Application.Services.Negocios;
using DAEAPF.Application.Services.Productos;
using DAEAPF.Application.Services.Usuarios;
using Microsoft.EntityFrameworkCore;
using DAEAPF.Infrastructure.Context;
using DAEAPF.Infrastructure;
using System;
using DAEAPF.Application.Interfaces.Repositories;
using DAEAPF.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// =============================
// CADENA DE CONEXIÓN
// =============================
string? mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");
string connectionString;

if (!string.IsNullOrWhiteSpace(mysqlUrl) && mysqlUrl.StartsWith("mysql://"))
{
    // Parsear Railway MYSQL_URL
    var uri = new Uri(mysqlUrl);

    var userInfo = uri.UserInfo.Split(':', 2);
    var username = userInfo[0];
    var password = userInfo.Length > 1 ? userInfo[1] : "";

    var server = uri.Host;
    var port = uri.Port;
    var database = uri.AbsolutePath.Trim('/');

    connectionString = $"Server={server};Port={port};Database={database};User={username};Password={password};SslMode=None;";
}
else
{
    // Fallback local (appsettings.json)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

Console.WriteLine("Cadena de conexión: " + connectionString);

// =============================
// DB CONTEXT
// =============================
builder.Services.AddDbContext<NegociosAppContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// =============================
// INFRAESTRUCTURA
// =============================
builder.Services.AddInfrastructure(builder.Configuration);

// =============================
// CORS
// =============================
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                    "https://daeapf-production.up.railway.app",
                    "http://localhost:3000"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// =============================
// CONTROLADORES Y SERVICIOS
// =============================
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INegocioService, NegocioService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<INegocioRepository, NegocioRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();


// =============================
// SWAGGER
// =============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DAEAPF API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese el token JWT en el campo: Bearer {su_token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// =============================
// PIPELINE HTTP
// =============================
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("ShowSwagger"))
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DAEAPF API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// === HABILITAR CORS ===
app.UseCors(MyAllowSpecificOrigins);

app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
