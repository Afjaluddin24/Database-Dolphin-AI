using Dolphin_AI.ApplicationContext;
using Dolphin_AI.Controllers.JWT;
using Dolphin_AI.Emailservice;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ✅ CORS ADD
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Services
builder.Services.AddScoped<EmailService>();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddScoped<JwtTokenHelper>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbcontext>(options =>
    options.UseNpgsql(connectionString));

// JWT
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// ✅ CORS અહીં લખવું
app.UseCors("AllowAll");
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Urls.Add("http://0.0.0.0:8080");

app.Run();