using System.Collections.Generic;
using System.Threading.Tasks;
using StocksServer.Data;

namespace StocksServer.Services
{
    public interface IStocksService
    {
        Task<IEnumerable<Stock>> GetStocks();
        Task<StockResponse> PostStocks(Stock[] stocks);
        Task<IEnumerable<Stock>> GetStocksInFactory(int id);
        Task<IEnumerable<Factory>> GetFactory();
        Task<int> GetWeeklyProduction(int factoryId, int weekNumber);
        Task<IEnumerable<Week>> GetFactoryProduction(int factoryId);
        Task<byte[]> GeneratePdfReport(StockReport stocks);
    }
}