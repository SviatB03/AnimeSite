﻿using AnimeSite.Models;
using AnimeSite.Repository.Interfaces;
using System.Data;
using Dapper;

namespace AnimeSite.Repository
{
    public class ReleaseScheduleRepository : IReleaseScheduleRepository
    {
        private readonly IDbConnection _dbConnection;

        public ReleaseScheduleRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<ReleaseSchedule>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<ReleaseSchedule>("SELECT * FROM ReleaseSchedule");
        }

        public async Task<ReleaseSchedule> GetByIdAsync(int id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<ReleaseSchedule>("SELECT * FROM ReleaseSchedule WHERE ReleaseScheduleId = @Id", new { Id = id });
        }

        public async Task AddAsync(ReleaseSchedule releaseSchedule)
        {
            var sql = "INSERT INTO ReleaseSchedule (AnimeId, ReleaseDate) VALUES (@AnimeId, @ReleaseDate)";
            await _dbConnection.ExecuteAsync(sql, releaseSchedule);
        }

        public async Task UpdateAsync(ReleaseSchedule releaseSchedule)
        {
            var sql = "UPDATE ReleaseSchedule SET AnimeId = @AnimeId, ReleaseDate = @ReleaseDate WHERE ReleaseScheduleId = @ReleaseScheduleId";
            await _dbConnection.ExecuteAsync(sql, releaseSchedule);
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM ReleaseSchedule WHERE ReleaseScheduleId = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
