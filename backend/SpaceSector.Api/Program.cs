using Microsoft.EntityFrameworkCore;
using SpaceSector.Api.Data;
using SpaceSector.Api.Services.Npcs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<SpaceSectorDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("SpaceSectorDb")));

builder.Services.AddScoped<INpcService, NpcService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
