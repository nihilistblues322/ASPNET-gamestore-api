using GameStore.Data;
using GameStore.DTO;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;


public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        group.MapGet("/", async (GameStoreContext db) =>
          await db.Games
            .Include(game => game.Genre)
            .Select(game => game.ToGameSummaryDto()).AsNoTracking().ToListAsync());

        group.MapGet("/{id}", async (int id, GameStoreContext db) =>
        {
            Game? game = await db.Games.FindAsync(id);
            return game is null ?
             Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

        }).WithName(GetGameEndpointName);


        group.MapPost("/", async (CreateGameDTO newGame, GameStoreContext db) =>
        {
            Game game = newGame.ToEntity();
            db.Games.Add(game);
            await db.SaveChangesAsync();
            return Results.CreatedAtRoute(
                GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        });


        group.MapPut("/{id}", async (int id, UpdateGameDTO updateGameDTO, GameStoreContext db) =>
        {
            var existingGame = await db.Games.FindAsync(id);
            if (existingGame is null) return Results.NotFound();

            db.Entry(existingGame)
                .CurrentValues
                .SetValues(updateGameDTO);

            await db.SaveChangesAsync();
            return Results.NoContent();
        });


        group.MapDelete("/{id}", async (int id, GameStoreContext db) =>
        {
            await db.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });


        return group;

    }
}
