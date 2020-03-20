using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StocksServer.Data
{
    [Table("Weeks")]
    public class Week
    {
        [Required]
        [Range(1, 53)]
        public int WeekNo { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int BladeProduction { get; set; }
        public int FactoryId { get; set; }
    }
}
