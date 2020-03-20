using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StocksServer.Data
{
    [Table("Factories")]
    public class Factory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
