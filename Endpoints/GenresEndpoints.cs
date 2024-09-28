using System;
using GameStore.API.Data;
using GameStore.API.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints (this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Genres.Select(genre=>genre.toDTO()).AsNoTracking().ToListAsync());

        return group;
    }
}
