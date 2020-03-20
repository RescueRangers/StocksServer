using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using StocksServer.Data;
using StocksServer.Repositories.Interfaces;

namespace StocksServer.Repositories
{
    public class FactoryRepository : RepositoryBase, IFactoryRepository
    {
        public FactoryRepository(Microsoft.Extensions.Configuration.IConfiguration config) : base(config)
        { }

        public async Task<IEnumerable<Factory>> GetFactories()
        {
            using (var db = Connection)
            {
                return await db.GetListAsync<Factory>().ConfigureAwait(false);
            }
        }

        public async Task<int?> InsertFactory(Factory factory)
        {
            using (var db = Connection)
            {
                var id = await db.InsertAsync<Factory>(factory).ConfigureAwait(false);

                return id;
            }
        }

        public async Task<bool> DeleteFactory(int factoryId)
        {
            var sql = "DELETE FROM Factories WHERE Id = @Id";
            using (var db = Connection)
            {
                var result = await db.ExecuteAsync(sql, new { Id = factoryId}).ConfigureAwait(false);

                return result != 0;
            }
        }

        public async Task<bool> UpdateFactory(Factory factory)
        {
            using (var db = Connection)
            {
                var result = await db.UpdateAsync<Factory>(factory).ConfigureAwait(false);

                return result != 0;
            }
        }

        public async Task<Factory> GetFactory(int id)
        {
            using (var db = Connection)
            {
                return await db.GetAsync<Factory>(id).ConfigureAwait(false);
            }
        }
    }
}
