using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace StocksServer.Data
{
    [Table("Stocks")]
    public class Stock
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ItemNumber { get; set; }

        [Editable(false)]
        public string ItemDescription { get; set; }

        public int Qty { get; set; }

        [Editable(false)]
        public int QtyPerBlade { get; set; }

        public int FactoryId { get; set; }

        [Editable(false)]
        public int DeliveryQty { get; set; }

        [Editable(false)]
        public int TotalQty { get => Qty + DeliveryQty; }

        [Editable(false)]
        public int QtyAfter { get; set; }

        public void CalculateQtyAfter(Week week, double wastePercent)
        {
            var stockWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            if (stockWeekNumber == week.WeekNo)
            {
                var stockDay = (int)(((Date.Ticks >> 14) + 1) % 7);
                QtyAfter = (int)(TotalQty - ((week.BladeProduction / 7) * 8 - stockDay ) * QtyPerBlade * (1 + wastePercent));
            }
            else
            {
                QtyAfter = (int)(TotalQty - (week.BladeProduction * QtyPerBlade * (1 + wastePercent)));
            }
        }
    }
}