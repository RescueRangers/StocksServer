using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksServer.Data
{
    [Table("Items")]
    public class Item
    {
        [Required]
        [StringLength(24, ErrorMessage = "Item number is too long.")]
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int QtyPerBlade { get; set; }
    }
}