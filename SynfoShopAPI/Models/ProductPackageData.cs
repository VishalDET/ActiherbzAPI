using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class ProductPackageData
    {
        public ProductData Product { get; set; }
        public List<ProductPackage> Packages { get; set; }
    }
}
