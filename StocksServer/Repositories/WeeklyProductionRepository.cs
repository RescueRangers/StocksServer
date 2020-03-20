using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Transactions;
using StocksServer.Repositories.Interfaces;
using StocksServer.Data;

namespace StocksServer.Repositories
{
    public class WeeklyProductionRepository : RepositoryBase, IWeeklyProductionRepository
    {
        public WeeklyProductionRepository(IConfiguration config) : base(config)
        { }

        public async Task<Week> GetWeek(int weekNumber, int factoryId)
        {
            var sql = "SELECT * FROM Weeks Where FactoryId = @FactoryId AND WeekNo = @WeekNo";
            using (var db = Connection)
            {
                return await db.QueryFirstAsync<Week>(sql, new { Weekno = weekNumber, FactoryId = factoryId}).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Week>> GetFactoryProduction(int factoryId)
        {
            using (var db = Connection)
            {
                return await db.GetListAsync<Week>(new { FactoryId = factoryId }).ConfigureAwait(false);
            }
        }

        public async Task<int> GetFactoryProductionOnWeek(int factoryId, int weekNo)
        {
            var sql = "SELECT BladeProduction FROM Weeks Where FactoryId = @FactoryId AND WeekNo = @WeekNo";

            using (var db = Connection)
            {
                return await db.QueryFirstAsync<int>(sql, new { FactoryId = factoryId, WeekNo = weekNo }).ConfigureAwait(false);
            }
        }

        public async Task<bool> InsertFactoryProduction(Week week)
        {
            var sql = "INSERT INTO Weeks(WeekNo, FactoryId, BladeProduction) VALUES(@WeekNo, @FactoryId, @BladeProduction)";
            using (var db = Connection)
            {
                var result = await db.ExecuteAsync(sql, new { WeekNo = week.WeekNo, FactoryId = week.FactoryId, BladeProduction = week.BladeProduction }).ConfigureAwait(false);
                return result != 0;
            }
        }

        public async Task InsertFactoryProduction(Week[] weeks)
        {
            using (var db = Connection)
            {
                var sql = @"
                                IF NOT EXISTS (SELECT * FROM [dbo].[Weeks] WHERE WeekNo = @WeekNo AND FactoryId = @FactoryId)
                                    INSERT INTO [dbo].[Weeks]([WeekNo], [FactoryId], [BladeProduction])
                                    VALUES(@WeekNo, @FactoryId, @BladeProduction)
                                ELSE
                                    UPDATE [dbo].[Weeks]
                                    SET BladeProduction = @BladeProduction
                                    WHERE WeekNo = @WeekNo AND FactoryId = @FactoryId ";
                foreach (var week in weeks)
                {
                    await db.ExecuteAsync(sql, new { week.WeekNo, week.FactoryId, week.BladeProduction });
                }
            }
        }

        public async Task<bool> UpdateFactoryProduction(Week week)
        {
            var sql = "UPDATE [dbo].[Weeks] SET BladeProduction = @BladeProduction WHERE WeekNo = @WeekNo AND FactoryId = @FactoryId ";
            using (var db = Connection)
            {
                var result = await db.ExecuteAsync(sql, new { BladeProduction = week.BladeProduction, WeekNo = week.WeekNo, FactoryId = week.FactoryId}).ConfigureAwait(false);
                return result != 0;
            }
        }

        public async Task<bool> DeleteFactoryProduction(int weekNo, int factoryId)
        {
            var sql = "DELETE FROM Weeks WHERE WeekNo = @WeekNo AND FactoryId = @FactoryId";
            using (var db = Connection)
            {
                var result = await db.ExecuteAsync(sql, new { WeekNo = weekNo, FactoryId = factoryId}).ConfigureAwait(false);
                return result != 0;
            }
        }

        public async Task DeleteProductionFromFactory(int factoryId)
        {
            using (var transaction = new TransactionScope())
            {
                using (var db = Connection)
                {
                    await db.DeleteListAsync<int>(new { FactoryId = factoryId }).ConfigureAwait(false);
                }

                transaction.Complete();
            }
        }
    }
}
