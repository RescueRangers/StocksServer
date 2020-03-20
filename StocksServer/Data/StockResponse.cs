using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksServer.Data
{
    public class StockResponse
    {
        public ResponseStatus Status { get; set; }
        public string Reason { get; set; }
        public IEnumerable<Stock> Stocks { get; set; }
    }
}
