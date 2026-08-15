using Microsoft.EntityFrameworkCore;
using SpaceSector.Api.Data;
using SpaceSector.Api.Services.Npcs;
using SpaceSector.Api.Services.Cameras;
using SpaceSector.Api.Services.Sessions;
using SpaceSector.Api.Services.Detections;
using SpaceSector.Api.Services.Surveillance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("UnityWebGL", policy =>
    {
        policy
            .WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<SpaceSectorDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("SpaceSectorDb")));

builder.Services.AddScoped<INpcService, NpcService>();
builder.Services.AddScoped<ICameraService, CameraService>();
builder.Services.AddScoped<ISimulationSessionService, SimulationSessionService>();
builder.Services.AddScoped<IDetectionService, DetectionService>();
builder.Services.AddScoped<ICameraVisibilityService, CameraVisibilityService>();
builder.Services.AddScoped<ISurveillanceService, SurveillanceService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SpaceSectorDbContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("UnityWebGL");

app.UseAuthorization();

app.MapControllers();

app.Run();
