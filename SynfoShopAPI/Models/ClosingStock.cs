using System.Collections.Generic;

namespace SynfoShopAPI.Models
{
    public class ClosingStock1
    {
        public List<StockItem> ClosingStock { get; set; }
    }

    public class StockItem
    {
        public string CompanyName { get; set; }
        public string CompanyGuid { get; set; }
        public string ProductName { get; set; }
        public string Godown { get; set; }
        public string BaseUnit { get; set; }
        public double BaseQTY { get; set; }
    }
}