using GameStore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDTO> games = [
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

app.MapGet("/games", () => games);

app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);

app.MapPost("/games", (CreateGameDTO newGame) =>
{
    GameDTO game = new(
      games.Count + 1,
      newGame.Name,
      newGame.Genre,
      newGame.Price,
      newGame.ReleaseDate
    );

    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});



app.Run();
