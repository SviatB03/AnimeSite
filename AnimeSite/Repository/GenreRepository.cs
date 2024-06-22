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
            return await _dbConnection.QueryAsync<Genre>("SELECT * FROM genre");
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<Genre>("SELECT * FROM genre WHERE GenreId = @Id", new { Id = id });
        }

        public async Task AddAsync(Genre genre)
        {
            var sql = "INSERT INTO genre (Name) VALUES (@Name)";
            await _dbConnection.ExecuteAsync(sql, genre);
        }

        public async Task UpdateAsync(Genre genre)
        {
            var sql = "UPDATE genre SET Name = @Name WHERE GenreId = @GenreId";
            await _dbConnection.ExecuteAsync(sql, genre);
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM genre WHERE GenreId = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
