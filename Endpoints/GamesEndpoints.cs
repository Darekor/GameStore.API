namespace GameStore.API.Endpoints;
using GameStore.API.DTO;
using GameStore.API.Data;
using GameStore.API.Entities;
using GameStore.API.Mapping;
using Microsoft.EntityFrameworkCore;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                       .WithParameterValidation();


        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.
                                                          Games.
                                                          Include(game=> game.Genre).
                                                          Select(game => game.ToGameSummaryDTO()).
                                                          AsNoTracking().
                                                          ToListAsync());

        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => 
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDTO());
        }).WithName(GetGameEndpointName);

        group.MapPost("/", async (CreateGameDTO newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync(); 

            return Results.CreatedAtRoute(
                GetGameEndpointName, 
                new {game.Id}, 
                game.ToGameDetailsDTO());
        });

        group.MapPut("/{id}", async (int id, UpdateGameDTO updatedGame, GameStoreContext dbContext) => 
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
                return Results.NotFound();

            dbContext.Entry(existingGame).
                            CurrentValues.
                            SetValues(updatedGame.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}",async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.
                      Where(game => game.Id == id).
                      ExecuteDeleteAsync();

            return Results.NoContent();
        });
        return group;
    }
}
