using GameStore.Entities;
using Microsoft.EntityFrameworkCore;
namespace GameStore.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Action" },
            new { Id = 2, Name = "Strategy" },
            new { Id = 3, Name = "RPG" },
            new { Id = 4, Name = "Shooter" },
            new { Id = 5, Name = "MMORPG" }

        );
    }
}

