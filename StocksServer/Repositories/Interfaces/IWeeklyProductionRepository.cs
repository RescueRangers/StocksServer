using System.Collections.Generic;
using System.Threading.Tasks;
using StocksServer.Data;

namespace StocksServer.Repositories.Interfaces
{
    public interface IWeeklyProductionRepository
    {
        Task<bool> DeleteFactoryProduction(int weekNo, int factoryId);
        Task DeleteProductionFromFactory(int factoryId);
        Task<IEnumerable<Week>> GetFactoryProduction(int factoryId);
        Task<int> GetFactoryProductionOnWeek(int factoryId, int weekNo);
        Task InsertFactoryProduction(Week[] weeks);
        Task<bool> UpdateFactoryProduction(Week week);
        Task<Week> GetWeek(int weekNumber, int factoryId);
        Task<bool> InsertFactoryProduction(Week week);
    }
}