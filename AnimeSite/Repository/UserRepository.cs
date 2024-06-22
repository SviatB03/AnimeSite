using AnimeSite.Repository.Interfaces;
using System.Data;
using Dapper;
using AnimeSite.Entity;

namespace AnimeSite.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<User>("SELECT * FROM User");
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<User>("SELECT * FROM User WHERE UserId = @Id", new { Id = id });
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<User>("SELECT * FROM User WHERE Email = @Email", new { Email = email });
        }

        public async Task AddAsync(User user)
        {
            var sql = "INSERT INTO User (Username, Email, Password, MobileNumber, UserType) VALUES (@Username, @Email, @Password, @MobileNumber, @UserType)";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task UpdateAsync(User user)
        {
            var sql = "UPDATE User SET Username = @Username, Email = @Email, Password = @Password, MobileNumber = @MobileNumber, UserType = @UserType WHERE UserId = @UserId";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM User WHERE UserId = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<User>("SELECT * FROM User WHERE Email = @Email AND Password = @Password", new { Email = email, Password = password });
        }
    }

}
