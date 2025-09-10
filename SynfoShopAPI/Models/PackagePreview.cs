using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class PackagePreview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int FranchiseId { get; set; }
        public string PackageName { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal SalePrice { get; set; }
        public int MinQuantity { get; set; }
        public string LogoUrl { get; set; }
        public string BrandName { get; set; }
        public string MainImage { get; set; }
    }
}
