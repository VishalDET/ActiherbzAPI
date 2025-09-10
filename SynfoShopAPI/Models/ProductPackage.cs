using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class ProductPackage
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PackageId { get; set; }
        public int FranchiseId { get; set; }
        public int AddedQuantity { get; set; }
        public string PackageName { get; set; }
        public string State { get; set; }
        public string city { get; set; }
        public double Price { get; set; }
        public double DiscountPercent { get; set; }
        public double SalePrice { get; set; }
        public string Materials { get; set; }
        public double MinQuantity { get; set; }
        public int DisplayOrder { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public string Size { get; set; }
        public string ColorText { get; set; }
        public string ColorCode { get; set; }
        public string SizeText { get; set; }

    }
    public class FranchisePackage
    {
        public string ColorText { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public double price { get; set; }
        public int FranchiseId { get; set;}
        public List<ProductPackage> ProductPackage { get; set; }

    }

}
