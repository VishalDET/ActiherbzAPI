using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class ProductFilter
    {
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string BrandId { get; set; }
        public string ProductId { get; set; }
        public string FranchiseId { get; set; }
    }
}
