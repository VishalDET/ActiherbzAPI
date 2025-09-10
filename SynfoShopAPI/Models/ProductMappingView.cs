using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class ProductMappingView : CommonProductMapping
    {
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        //public string SubCategoryName { get; set; }
        public string MainImage { get; set; }
        public string FranchiseName { get; set; }
        public int StockQuantity { get; set; }
        public double MRP { get; set; }
        public string Zone { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string RelationashipManager { get; set; }

        public List<CommonPackage> packages { get; set; }
    }
}
