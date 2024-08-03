using GameStore.DTO;
using GameStore.Entities;

namespace GameStore.Mapping;

public static class GenreMapping
{
    public static GenreDTO toDto(this Genre genre)
    {
        return new GenreDTO(genre.Id, genre.Name);
    }
}
