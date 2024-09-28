using Serilog;
using Transazioni.API.Extensions;
using Transazioni.Application;
using Transazioni.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureSwagger();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks();

builder.Services.AddJwtAuthentication();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.ApplyMigrations();
await app.SeedData();

app.UseCors(opt =>
{
    opt.WithOrigins("http://localhost:4200", "https://moneymap.site");
    opt.AllowAnyMethod();
    opt.AllowAnyHeader();
    opt.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/healthz");

app.MapControllers();

await app.RunAsync();
