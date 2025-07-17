using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using app.api.Data;
using app.api.Services;
using app.api.Middleware;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.ConfigureCors();

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "your-secret-key-here"))
        };
    });

builder.Services.AddAuthorization();

// Register Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ScheduleService>();
builder.Services.AddScoped<AppointmentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowVueApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("Tentando conectar ao banco de dados e garantir que as tabelas existem...");
        context.Database.EnsureCreated();
        logger.LogInformation("Conexão e criação do banco de dados bem-sucedidas!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao conectar ou criar o banco de dados!");
        throw;
    }
}

app.Run();
