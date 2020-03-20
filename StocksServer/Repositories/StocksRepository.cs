using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using StocksServer.Data;
using StocksServer.Repositories.Interfaces;

namespace StocksServer.Repositories
{
    public class StocksRepository : RepositoryBase, IStocksRepository
    {
        public StocksRepository(IConfiguration config) : base(config)
        { }

        public async Task<IEnumerable<Stock>> GetStocks()
        {
            using (var db = Connection)
            {
                return await db.GetListAsync<Stock>().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Stock>> GetStocksInFactory(int factoryId)
        {
            using (var db = Connection)
            {
                return await db.GetListAsync<Stock>(new { FactoryId = factoryId }).ConfigureAwait(false);
            }
        }

        public async Task<int?> InsertStock(Stock stock)
        {
            using (var db = Connection)
            {
                var sql = $@"
                                IF NOT EXISTS (SELECT * FROM [dbo].[Stocks] WHERE ItemNumber = @ItemNumber AND FactoryId = @FactoryId)
                                    INSERT INTO [dbo].[Stocks]([ItemNumber], [FactoryId], [Qty], [Date])
                                    VALUES(@ItemNumber, @FactoryId, @Qty, @Date)
                                ELSE
                                    UPDATE [dbo].[Stocks]
                                    SET Qty = @Qty, Date = @Date
                                    WHERE ItemNumber = @ItemNumber AND FactoryId = @FactoryId ";


                return await db.ExecuteAsync(sql, new { ItemNumber = stock.ItemNumber, FactoryId = stock.FactoryId, Qty = stock.Qty, Date = stock.Date}).ConfigureAwait(false);
            }
        }

        public async Task InsertStocks(Stock[] stocks)
        {
            using (var db = Connection)
            {
                var deleteSql = "Delete From Stocks WHERE FactoryId = @FactoryId";
                await db.ExecuteAsync(deleteSql, new { FactoryId = stocks[0].FactoryId }).ConfigureAwait(false);

                foreach (var stock in stocks)
                {
                    await db.InsertAsync(stock).ConfigureAwait(false);
                }
            }
        }

        public async Task<bool> DeleteStock(Stock stock)
        {
            using (var db = Connection)
            {
                var result = await db.DeleteAsync(stock).ConfigureAwait(false);

                return result != 0;
            }
        }

        public async Task<bool> UpdateStock(Stock stock)
        {
            using (var db = Connection)
            {
                var result = await db.UpdateAsync(stock).ConfigureAwait(false);

                return result != 0;
            }
        }

        public async Task<Stock> GetStock(int id)
        {
            using (var db = Connection)
            {
                return await db.GetAsync<Stock>(id).ConfigureAwait(false);
            }
        }
    }
}
