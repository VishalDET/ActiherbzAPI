using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FranchiseId { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string MainImage { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string MacId { get; set; }
        public int PackageId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class OrderDetailsMac
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FranchiseId { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string MainImage { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public int PackageId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string OrderMacIds { get; set; }
        public List<OrderMac> OrderMacList { get; set; }
    }
    public class OrderMac
    {
        public int OrderId { get; set; }
        public int OrderDetailsId { get; set; }
        public string MacId { get; set; }
    }
}
