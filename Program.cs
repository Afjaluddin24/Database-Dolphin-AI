using Dolphin_AI.ApplicationContext;
using Dolphin_AI.Controllers.JWT;
using Dolphin_AI.Emailservice;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Services
builder.Services.AddScoped<EmailService>();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddScoped<JwtTokenHelper>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 DATABASE SAFE LOAD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Database connection string is missing.");
}

builder.Services.AddDbContext<ApplicationDbcontext>(options =>
    options.UseNpgsql(connectionString));


// 🔐 JWT SAFE LOAD
var jwtKey = builder.Configuration["Jwt:Key"] ?? "TemporarySuperSecretKey123456";

var key = Encoding.ASCII.GetBytes(jwtKey);

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
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "Dolphin-AI",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "Dolphin-AI",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// Enable Swagger in Production
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Required for Render
app.Urls.Add("http://0.0.0.0:8080");

app.Run();