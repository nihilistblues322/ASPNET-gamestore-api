using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.DTO;
using GameStore.Entities;

namespace GameStore.Mapping
{
    public static class GameMapping
    {
        public static Game ToEntity(this CreateGameDTO game)
        {
            return new Game()
            {
                Name = game.Name,
                GenreId = game.GenreId,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate
            };
        }
        public static Game ToEntity(this UpdateGameDTO game, int id)
        {
            return new Game()
            {
                Id = id,
                Name = game.Name,
                GenreId = game.GenreId,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate
            };
        }

        public static GameSummaryDTO ToGameSummaryDto(this Game game)
        {
            return new(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            );
        }
        public static GameDetailsDTO ToGameDetailsDto(this Game game)
        {
            return new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );
        }
    }
}