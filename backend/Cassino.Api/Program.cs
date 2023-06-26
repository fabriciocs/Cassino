using Cassino.Api.Configuration;
using Cassino.Application;
using Cassino.Application.Hubs;
using Cassino.Infra;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//Melhorar os logs
builder
    .Host
    .UseSystemd()
    .UseSerilog((_, lc) =>
    {
        lc.WriteTo.Console();
        lc.WriteTo.Debug();
    });

builder
    .Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder
    .Services
    .AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });

builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.AddSignalR().AddAzureSignalR("Endpoint=https://signaltestpix.service.signalr.net;AccessKey=mYDV9DNqoReXp0D7YKuDYVzUC43Qpwl2Hrv6mHaM0rM=;Version=1.0;");
builder.Services.AddApiConfiguration();
builder.Services.ConfigureServices();
builder.Services.AddVersioning();
builder.Services.AddSwagger();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.AddOpenTelemetryTracingConfig(builder.Configuration);

// add azure

// Add CORs
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapHub<PixHub>("/hubs/pix");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseMigrations(app.Services);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();