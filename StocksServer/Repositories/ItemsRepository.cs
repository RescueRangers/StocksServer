using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using StocksServer.Data;
using StocksServer.Repositories.Interfaces;

namespace StocksServer.Repositories
{
    public class ItemsRepository : RepositoryBase, IItemsRepository
    {
        public ItemsRepository(IConfiguration config) : base(config)
        { }

        public async Task<IEnumerable<Item>> GetItems()
        {
            using (var db = Connection)
            {
                return await db.GetListAsync<Item>().ConfigureAwait(false);
            }
        }

        public async Task<bool> InsertItem(Item item)
        {
            var sql = @"INSERT INTO Items (ItemNumber, ItemDescription, QtyPerBlade) VALUES (@ItemNumber, @ItemDescription, @QtyPerBlade)";
            using (var db = Connection)
            {
                var id = await db.ExecuteAsync(sql, new{ ItemNumber = item.ItemNumber, ItemDescription = item.ItemDescription, QtyPerBlade = item.QtyPerBlade}).ConfigureAwait(false);

                return id > 0;
            }
        }

        public async Task InsertItems(Item[] items)
        {
            using (var db = Connection)
            {
                var sql = @"
     INSERT INTO [dbo].[Items]
           ([ItemNumber]
           ,[ItemDescription]
           ,[QtyPerBlade])
     VALUES
           (@ItemNumber
           ,@ItemDescription
           ,@QtyPerBlade)";


                foreach (var item in items)
                {
                    await db.ExecuteAsync(sql, new { item.ItemNumber, item.ItemDescription, item.QtyPerBlade }).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Check if a given item exist in the database
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <returns>Returns true if it does, flase if it doesn't</returns>
        public async Task<bool> CheckIfItemExists(string itemNumber)
        {
            if (string.IsNullOrWhiteSpace(itemNumber)) return false;

            using (var db = Connection)
            {
                var sql = @"
     SELECT COUNT(1)
     FROM Items
     WHERE ItemNumber = @ItemNumber;";

                var result = await db.QuerySingleAsync<int>(sql, new { itemNumber }).ConfigureAwait(false);
                return result != 0;
            }
        }

        public async Task<bool> DeleteItem(string itemNumber)
        {
            var sql = @"DELETE FROM Items WHERE ItemNumber = @ItemNumber";
            using (var db = Connection)
            {
                var result = await db.ExecuteAsync(sql, new { ItemNumber = itemNumber}).ConfigureAwait(false);

                return result != 0;
            }
        }

        public async Task<bool> UpdateItem(Item item)
        {
            var sql = @"UPDATE ITEMS SET ItemDescription = @ItemDescription, QtyPerBlade = @QtyPerBlade WHERE ItemNumber = @ItemNumber";
            using (var db = Connection)
            {
                var result = await db.ExecuteAsync(sql, new { ItemDescription = item.ItemDescription, QtyPerBlade = item.QtyPerBlade, ItemNumber = item.ItemNumber}).ConfigureAwait(false);

                return result != 0;
            }
        }

        public async Task<Item> GetItem(string itemNumber)
        {
            var sql = @"Select * from Items WHERE ItemNumber = @ItemNumber";
            using (var db = Connection)
            {
                return await db.QuerySingleAsync<Item>(sql, new { ItemNumber = itemNumber}).ConfigureAwait(false);
            }
        }
    }
}