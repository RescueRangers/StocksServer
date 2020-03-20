using System.Collections.Generic;
using System.Threading.Tasks;
using StocksServer.Data;

namespace StocksServer.Repositories.Interfaces
{
    public interface IItemsRepository
    {
        Task<bool> CheckIfItemExists(string itemNumber);
        Task<bool> DeleteItem(string itemNumber);
        Task<Item> GetItem(string itemNumber);
        Task<IEnumerable<Item>> GetItems();
        Task<bool> InsertItem(Item item);
        Task InsertItems(Item[] items);
        Task<bool> UpdateItem(Item item);
    }
}