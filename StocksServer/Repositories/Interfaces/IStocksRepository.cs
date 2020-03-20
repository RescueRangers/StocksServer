using System.Collections.Generic;
using System.Threading.Tasks;
using StocksServer.Data;

namespace StocksServer.Repositories.Interfaces
{
    public interface IStocksRepository
    {
        Task<bool> DeleteStock(Stock stock);
        Task<Stock> GetStock(int id);
        Task<IEnumerable<Stock>> GetStocks();
        Task<int?> InsertStock(Stock stock);
        Task<bool> UpdateStock(Stock stock);
        Task<IEnumerable<Stock>> GetStocksInFactory(int factoryId);
        Task InsertStocks(Stock[] stocks);
    }
}