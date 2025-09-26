using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

string connectionString = File.ReadAllText("../connectionString.txt");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case BadHttpRequestException:
            case ArgumentException:
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = exception.Message });
                break;
            case KeyNotFoundException:
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { error = exception.Message });
                break;
            case Exception:
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { error = exception.Message });
                break;
            default:
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { error = "An unknown error occurred." });
                break;
        }
    });
});

app.MapGet("/players", async (IPersonService service) =>
{
    return await service.ListAllPlayersAsync();
});

app.MapPost("/players", async (Person newPlayer, IPersonService service) =>
{
    var createdPlayer = await service.RegisterPlayerAsync(newPlayer);
    return Results.Created($"/players/{createdPlayer.Id}", createdPlayer);
});

app.MapGet("/players/{id}", async (int id, IPersonService service) =>
{
    var player = await service.GetPlayerProfileAsync(id);
    return player is not null ? Results.Ok(player) : Results.NotFound();
});

app.MapDelete("/players/{id}", async (int id, IPersonService service) =>
{
    await service.RemovePlayerAsync(id);
    return Results.Ok();
});

app.MapGet("/players/{id}/tournaments", async (int id, IPersonService service) =>
{
    var tournaments = await service.ListPlayerTournaments(id);
    return Results.Ok(tournaments);
});

app.MapGet("/tournaments", async (ITournamentService service) =>
{
    return await service.ListAllTournamentsAsync();
});
app.MapPost("/tournaments", async (TournamentDto newTournament, ITournamentService service) =>
{
    var createdTournament = await service.AddTournamentAsync(newTournament);
    return Results.Created($"/tournaments/{createdTournament.Id}", createdTournament);
});

app.MapGet("/tournaments/{id}", async (int id, ITournamentService service) =>
{
    var tournament = await service.GetTournamentDetailsAsync(id);
    return tournament is not null ? Results.Ok(tournament) : Results.NotFound();
});

app.MapDelete("/tournaments/{id}", async (int id, ITournamentService service) =>
{
    await service.RemoveTournamentAsync(id);
    return Results.Ok();
});

app.MapGet("/tournaments/{id}/participants", async (int id, ITournamentService service) =>
{
    var participants = await service.ListTournamentParticipantsAsync(id);
    return Results.Ok(participants);
});
app.MapPost("/tournaments/{tournamentId}/participants", async (int tournamentId, [FromBody] int playerId, ITournamentService service) =>
{
    await service.RegisterParticipantAsync(tournamentId, playerId);
    return Results.Ok();
});
app.MapDelete("/tournaments/{tournamentId}/participants", async (int tournamentId, [FromBody] int playerId, ITournamentService service) =>
{
    await service.WithdrawParticipantAsync(tournamentId, playerId);
    return Results.Ok();
});

app.Run();
