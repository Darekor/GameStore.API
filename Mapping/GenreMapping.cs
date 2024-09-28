using System;
using GameStore.API.DTO;
using GameStore.API.Entities;

namespace GameStore.API.Mapping;

public static class GenreMapping
{
    public static GenreDTO toDTO(this Genre genre)
    {
        return new GenreDTO(genre.Id,genre.Name);
    }
}
