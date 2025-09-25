using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

string connectionString = File.ReadAllText("../connectionString.txt");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/players", async (IPersonService service) =>
{
    return await service.ListAllPlayersAsync();
});

app.MapPost("/players", async (Person newPlayer, IPersonService service) =>
{
    var createdPlayer = await service.RegisterPlayerAsync(newPlayer);
    return Results.Created($"/players/{createdPlayer.Id}", createdPlayer);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
