﻿using AnimeSite.Repository.Interfaces;
using System.Data;
using Dapper;
using AnimeSite.Entity;

namespace AnimeSite.Repository
{
    public class UserAnimeTrackingRepository : IUserAnimeTrackingRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserAnimeTrackingRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<UserAnimeTracking>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<UserAnimeTracking>("SELECT * FROM UserAnimeTracking");
        }

        public async Task<UserAnimeTracking> GetByIdAsync(int id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<UserAnimeTracking>("SELECT * FROM UserAnimeTracking WHERE UserAnimeTrackingId = @Id", new { Id = id });
        }

        public async Task AddAsync(UserAnimeTracking userAnimeTracking)
        {
            var sql = "INSERT INTO UserAnimeTracking (UserId, AnimeId) VALUES (@UserId, @AnimeId)";
            await _dbConnection.ExecuteAsync(sql, userAnimeTracking);
        }

        public async Task UpdateAsync(UserAnimeTracking userAnimeTracking)
        {
            var sql = "UPDATE UserAnimeTracking SET UserId = @UserId, AnimeId = @AnimeId WHERE UserAnimeTrackingId = @UserAnimeTrackingId";
            await _dbConnection.ExecuteAsync(sql, userAnimeTracking);
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM UserAnimeTracking WHERE UserAnimeTrackingId = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
