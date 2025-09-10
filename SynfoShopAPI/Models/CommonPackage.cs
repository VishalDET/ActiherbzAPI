using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SynfoShopAPI.Models
{
    public class CommonPackage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int FranchiseId { get; set; }
        public string PackageName { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
        public double Price { get; set; }
        public double DiscountPercent { get; set; }
        public double SalePrice { get; set; }
        public double MinQuantity { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string SpType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        //public string SubCategoryName { get; set; }
        public int Result { get; set; }

    }
    public class CommonPackageAdd
    {


        public int Id { get; set; }
        public int ProductId { get; set; }
        public int FranchiseId { get; set; }
        public string PackageName { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
        public double Price { get; set; }
        public double DiscountPercent { get; set; }
        public double SalePrice { get; set; }
        public double MinQuantity { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string SpType { get; set; }
        public int Result { get; set; }

    }
}
