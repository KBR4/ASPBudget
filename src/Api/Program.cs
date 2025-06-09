using Infrastructure;
using Application;
using Infrastructure.Database;
using Api.ExceptionHandlers;
using Serilog;
using Api.Middleware;
using Microsoft.OpenApi.Models;
using Api.Configuration;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Application.Requests;

var builder = WebApplication.CreateBuilder(args);

//"[{Timestamp:HH:mm:ss} {Level:u3}]{Message:lj} {SourceContext}{NewLine}{Exception}"
//serilog
var logpattern = @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} 
[{Level:u3] [ClientIp = {ClientIp}] {Message:lj} {NewLine} {Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.WithClientIp()
    .WriteTo.Console(outputTemplate: logpattern)
    .WriteTo.File(Path.Combine("logs", "quiz-backend-.log"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        rollOnFileSizeLimit: true,
        outputTemplate: logpattern)
    .CreateLogger();

// Add services to the container.
builder.Services.AddSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Budget API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Authorization: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<InvalidCredentialsExceptionHandler>();
builder.Services.AddExceptionHandler<DbExceptionHandler>();
builder.Services.AddExceptionHandler<ApplicationExceptionHandler>(); 
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);
var secret = jwtSettings["Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");

builder.Services.AddAuthentication("HttponlyAuth")
    .AddCookie("HttponlyAuth", options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.Name = "auth_token";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;
    options.AddFixedWindowLimiter("login", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1); // 1-minute window
        limiterOptions.PermitLimit = 5; // Max 5 login attempts per minute
    });
});

builder.Services.AddCors(
    (options) =>
    {
        options.AddPolicy("AllowLocalhost", policy =>
        {
            policy.WithOrigins("localhost", "http://localhost:3000")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
    }
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
    migrationRunner.Run();
}

app.UseMiddleware<PerformanceMiddleware>(TimeSpan.FromMilliseconds(1000));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");

app.UseRateLimiter();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();