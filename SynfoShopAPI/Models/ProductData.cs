using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class ProductData : Product
    {

        //public List<string> Packages { get; set; } = new List<string>();
        // public List<ProductPackage> Packages { get; set; }
        public List<franchisePackage> FranchisePackage { get; set; }

        public string PriceRange { get; set; }
        public int PackageId { get; internal set; }
        //public string State { get; set; }


    }

    public class CheckoutProductData : CheckoutProduct
    {
        //public string PackageName { get; set; }

        //public List<string> Packages { get; set; } = new List<string>();
        // public List<ProductPackage> Packages { get; set; }
        public List<franchisePackage> FranchisePackage { get; set; }

        //public string State { get; set; }


    }
    public class franchisePackage
    {
        public int FranchiseId { get; set; }
        public int Color { get; set; }
        public string ColorText { get; set; }
        public string ColorCode { get; set; }
        public string city { get; set; }
        public double Price { get; set; }
        public string DeliveryBy { get; set; }
        public List<ProductPackage> Packages { get; set; }
    }



    //public class ProductReference
    //{
    //    public int ProductId { get; set; }
    //    public int ProductReferenceId { get; set; }

    //}
}
