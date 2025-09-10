using System.Collections.Generic;

namespace SynfoShopAPI.Models
{
    public class BrandProducts
    {
        public string BrandLogo { get; set; }
        public string BrandName { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalValue { get; set; }
    }
    public class CategoryProducts
    {
        public List<BrandProducts> ValueBrand { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalValue { get; set; }
        public string CategoryName { get; set; }
    }
}
