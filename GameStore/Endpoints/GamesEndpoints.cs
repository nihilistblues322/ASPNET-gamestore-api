using GameStore.Data;
using GameStore.DTO;
using GameStore.Entities;
using GameStore.Mapping;

namespace GameStore.Endpoints;


public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDTO> games = [
        new(
        1,
        "Street Fighter V",
        "Fighting",
        19.99m,
        new DateOnly(2016, 10, 21)
    ),
    new(
        2,
        "The Legend of Zelda: Breath of the Wild",
        "Action-adventure",
        59.99m,
        new DateOnly(2017, 3, 3)
    ),
    new(
        3,
        "Super Mario Odyssey",
        "Platform",
        79.99m,
        new DateOnly(2017, 10, 27)
    ),
    new(
        4,
        "Horizon Zero Dawn",
        "Action-adventure",
        23.91m,
        new DateOnly(2017, 11, 11)
    ),
    new(
        5,
        "Final Fantasy 7",
        "Roleplaying",
        81.55m,
        new DateOnly(2010, 9, 30)
    )

    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        group.MapGet("/", () => games);

        group.MapGet("/{id}", (int id) =>
        {
            GameDTO? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);

        }).WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDTO newGame, GameStoreContext db) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = db.Genres.Find(newGame.GenreId);

            db.Games.Add(game);
            db.SaveChanges();
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToDto());
        });

        group.MapPut("/{id}", (int id, UpdateGameDTO updateGameDTO) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1) return Results.NotFound();

            games[index] = new GameDTO(
                id,
                updateGameDTO.Name,
                updateGameDTO.Genre,
                updateGameDTO.Price,
                updateGameDTO.ReleaseDate
            );

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });

        return group;

    }
}
