using System.ComponentModel.DataAnnotations;

namespace GameStore.DTO;

public record class CreateGameDTO(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(30)] string Genre,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);
