using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jsreport.AspNetCore;
using Microsoft.Extensions.Logging;
using StocksServer.Data;
using StocksServer.Repositories.Interfaces;

namespace StocksServer.Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IItemsRepository _itemsRepository;
        private readonly IFactoryRepository _factoryRepository;
        private readonly IWeeklyProductionRepository _weeklyProductionRepository;
        private readonly IJsReportMVCService _jsReport;
        private readonly ILogger<StocksService> _logger;

        public StocksService(IStocksRepository stocksRepository, IItemsRepository itemsRepository, IFactoryRepository factoryRepository, ILogger<StocksService> logger, IWeeklyProductionRepository weeklyProductionRepository, IJsReportMVCService jsReport)
        {
            _stocksRepository = stocksRepository;
            _itemsRepository = itemsRepository;
            _factoryRepository = factoryRepository;
            _logger = logger;
            _weeklyProductionRepository = weeklyProductionRepository;
            _jsReport = jsReport;
        }

        public async Task<IEnumerable<Stock>> GetStocks()
        {
            return await _stocksRepository.GetStocks().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Factory>> GetFactory()
        {
            return await _factoryRepository.GetFactories().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Stock>> GetStocksInFactory(int id)
        {
            var stocks = await _stocksRepository.GetStocksInFactory(id).ConfigureAwait(false);
            var items = await _itemsRepository.GetItems().ConfigureAwait(false);

            return from stock in stocks
                   join item in items on stock.ItemNumber equals item.ItemNumber
                   select new Stock
                   {
                       Date = stock.Date,
                       ItemNumber = stock.ItemNumber,
                       Id = stock.Id,
                       QtyPerBlade = item.QtyPerBlade,
                       FactoryId = stock.FactoryId,
                       ItemDescription = item.ItemDescription,
                       Qty = stock.Qty
                   };
        }

        public async Task<StockResponse> PostStocks(Stock[] stocks)
        {
            if (stocks == null || stocks.Length < 1) return new StockResponse { Status = ResponseStatus.Failure, Reason = "Stocks are empty" };
            var itemNumbers = stocks.Select(s => s.ItemNumber);

            if (!itemNumbers.Any()) return new StockResponse { Status = ResponseStatus.Failure, Reason = "Stock has no item numbers" };

            foreach (var item in itemNumbers)
            {
                var exists = await _itemsRepository.CheckIfItemExists(item).ConfigureAwait(false);

                if (!exists)
                {
                    return new StockResponse { Status = ResponseStatus.Failure, Reason = $"Item {item} doesn't exist. Please add it in the items tab" };
                }
            }

            await _stocksRepository.InsertStocks(stocks).ConfigureAwait(false);

            return new StockResponse { Status = ResponseStatus.Success };
        }

        public async Task<int> GetWeeklyProduction(int factoryId, int weekNumber)
        {
            return await _weeklyProductionRepository.GetFactoryProductionOnWeek(factoryId, weekNumber).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Week>> GetFactoryProduction(int factoryId)
        {
            return await _weeklyProductionRepository.GetFactoryProduction(factoryId).ConfigureAwait(false);
        }

        public async Task<byte[]> GeneratePdfReport(StockReport stocks)
        {
            var report = await _jsReport.RenderByNameAsync("StockReport", stocks).ConfigureAwait(false);

            using (var ms = new MemoryStream())
            {
                await report.Content.CopyToAsync(ms).ConfigureAwait(false);
                return ms.ToArray();
            }
        }
    }
}
