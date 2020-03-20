using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StocksServer.Data;

namespace StocksServer.Repositories.Interfaces
{
    public interface IFactoryRepository
    {
        Task<bool> DeleteFactory(int factoryId);
        Task<IEnumerable<Factory>> GetFactories();
        Task<int?> InsertFactory(Factory factory);
        Task<bool> UpdateFactory(Factory factory);
        Task<Factory> GetFactory(int id);
    }
}
