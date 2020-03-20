using System;
using System.Collections.Generic;
using System.Text;

namespace StocksServer.Data
{
    public class StockReport
    {
        public Stock[] Stocks { get; set; }
        public int WeeklyBladeProduction { get; set; }
        public int WeekNumber { get; set; }
        public double WastePercent { get; set; }
        public DateTime ReportDate { get; set; }
        public string Factory { get; set; }
    }
}
