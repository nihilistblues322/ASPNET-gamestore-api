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

        group.MapGet("/", (GameStoreContext db) => db.Games
            .Include(game => game.Genre)
            .Select(game => game.ToGameSummaryDto()).AsNoTracking());

        group.MapGet("/{id}", (int id, GameStoreContext db) =>
        {
            Game? game = db.Games.Find(id);
            return game is null ?
             Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

        }).WithName(GetGameEndpointName);


        group.MapPost("/", (CreateGameDTO newGame, GameStoreContext db) =>
        {
            Game game = newGame.ToEntity();
            db.Games.Add(game);
            db.SaveChanges();
            return Results.CreatedAtRoute(
                GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        });


        group.MapPut("/{id}", (int id, UpdateGameDTO updateGameDTO, GameStoreContext db) =>
        {
            var existingGame = db.Games.Find(id);
            if (existingGame is null) return Results.NotFound();

            db.Entry(existingGame)
                .CurrentValues
                .SetValues(updateGameDTO);

            db.SaveChanges();
            return Results.NoContent();
        });

        
        group.MapDelete("/{id}", (int id, GameStoreContext db) =>
        {
            db.Games.Where(game => game.Id == id).ExecuteDelete();
            return Results.NoContent();
        });
        

        return group;

    }
}
