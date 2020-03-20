using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using StocksServer.Data;

namespace StocksServer.Services
{
    public static class EPPlusBuilder
    {
        public static List<Week> ParseProduction(ExcelPackage xlPackage, int factoryId)
        {
            var weeks = new List<Week>();

            var workbook = xlPackage.Workbook;

            if (workbook != null)
            {
                var worksheet = workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    var rowCount = worksheet.Dimension.End.Row;
                    for (var i = 1; i <= rowCount; i++)
                    {
                        var weekHasValue = short.TryParse(worksheet.Cells[i, 1].Text, out var weekNo);
                        var productionHasvalue = int.TryParse(worksheet.Cells[i, 2].Text, out var production);

                        if (weekHasValue && productionHasvalue)
                        {
                            weeks.Add(new Week { WeekNo = weekNo, BladeProduction = production, FactoryId = factoryId });
                        }
                    }
                }
            }

            var weekNumbers = weeks.Select(s => s.WeekNo);
            var missingWeeks = Enumerable.Range(1, 53).Except(weekNumbers);

            foreach (var w in missingWeeks)
            {
                weeks.Add(new Week { BladeProduction = 0, FactoryId = factoryId, WeekNo = w });
            }

            return weeks.GroupBy(g => g.WeekNo).Select(wk => new Week { WeekNo = wk.First().WeekNo, FactoryId = wk.First().FactoryId, BladeProduction = wk.Sum(s => s.BladeProduction) }).ToList();
        }

        public static List<Stock> ParseStocks(ExcelPackage xlPackage, int factoryId)
        {
            var stocks = new List<Stock>();

            var workbook = xlPackage.Workbook;

            if (workbook != null)
            {
                var worksheet = workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    var rowCount = worksheet.Dimension.End.Row;
                    for (var i = 1; i <= rowCount; i++)
                    {
                        var itemName = worksheet.Cells[i, 1].Text;

                        var qtyHasValue = short.TryParse(worksheet.Cells[i, 2].Text, out var qty);

                        if (qtyHasValue)
                        {
                            stocks.Add(new Stock { FactoryId = factoryId, ItemNumber = itemName, Qty = qty, Date = DateTime.Now });
                        }
                    }
                }
            }

            return stocks;
        }

        public static List<Item> ParseItems(ExcelPackage xlPackage)
        {
            var items = new List<Item>();

            var workbook = xlPackage.Workbook;

            if (workbook != null)
            {
                var worksheet = workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    var rowCount = worksheet.Dimension.End.Row;
                    for (var i = 1; i <= rowCount; i++)
                    {
                        var itemName = worksheet.Cells[i, 1].Text;
                        var itemDescription = worksheet.Cells[i, 2].Text;

                        var qtyHasValue = short.TryParse(worksheet.Cells[i, 3].Text, out var qty);

                        if (qtyHasValue)
                        {
                            items.Add(new Item { ItemNumber = itemName, ItemDescription = itemDescription, QtyPerBlade = qty });
                        }
                    }
                }
            }

            return items;
        }
    }
}
