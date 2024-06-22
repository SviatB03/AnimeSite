using AnimeSite.Repository.Interfaces;
using AnimeSite.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace AnimeSite.Repository
{
    public class AnimeRepository : IAnimeRepository
    {
        private readonly IDbConnection _dbConnection;

        public AnimeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Anime>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<Anime>("SELECT * FROM Anime");
        }

        public async Task<Anime> GetByIdAsync(int id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<Anime>("SELECT * FROM Anime WHERE AnimeId = @Id", new { Id = id });
        }

        public async Task AddAsync(Anime anime)
        {
            var sql = "INSERT INTO Anime (Title, Description, ImagePath, GenreId) VALUES (@Title, @Description, @ImagePath, @GenreId)";
            await _dbConnection.ExecuteAsync(sql, anime);
        }

        public async Task UpdateAsync(Anime anime)
        {
            var sql = "UPDATE Anime SET Title = @Title, Description = @Description, ImagePath = @ImagePath, GenreId = @GenreId WHERE AnimeId = @AnimeId";
            await _dbConnection.ExecuteAsync(sql, anime);
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Anime WHERE AnimeId = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
