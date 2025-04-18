using Infrastructure;
using Application;
using Infrastructure.Database;
using Api.ExceptionHandlers;
using Serilog;
using Api.Middleware;

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
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DbExceptionHandler>();
builder.Services.AddExceptionHandler<ApplicationExceptionHandler>(); 
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

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

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();