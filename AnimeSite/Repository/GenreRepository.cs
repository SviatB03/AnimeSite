using AnimeSite.Repository.Interfaces;
using System.Data;
using Dapper;
using AnimeSite.Entity;

namespace AnimeSite.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly IDbConnection _dbConnection;

        public GenreRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<Genre>("SELECT * FROM Genre");
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<Genre>("SELECT * FROM Genre WHERE GenreId = @Id", new { Id = id });
        }

        public async Task AddAsync(Genre genre)
        {
            var sql = "INSERT INTO Genre (Name) VALUES (@Name)";
            await _dbConnection.ExecuteAsync(sql, genre);
        }

        public async Task UpdateAsync(Genre genre)
        {
            var sql = "UPDATE Genre SET Name = @Name WHERE GenreId = @GenreId";
            await _dbConnection.ExecuteAsync(sql, genre);
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Genre WHERE GenreId = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
