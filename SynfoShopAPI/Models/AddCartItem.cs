using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class AddCartItem
    {
        public int IsApp { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int PackageId { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }

    public class PinCode
    {
        public int Id { get; set; }
        public int Pincode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string dates { get; set; }
    }
    public class CartProduct
    {
        public string ItemType { get; set; }
        public string MainImage { get; set; }
        public int Id { get; set; }
        public string ProductTitle { get; set; }
        public int IsApp { get; set; }
        public int Units { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public int PackageId { get; set; }
        public double Price { get; set; }
        public double DiscountPercent { get; set; }
        public double DiscountPrice { get; set; }
        public double SalePrice { get; set; }
        public double TotalPrice { get; set; }
        public double GSTAmount { get; set; }
        public double ConveniencePer { get; set; }
        public double ConvenienceFee { get; set; }
        public double GrandTotalWithoutShipping { get; set; }
        public double ShippingCharges { get; set; }
        public double GrandTotal { get; set; }
        public string DeliveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }

    }
    public class CartProductAndFranchiseId
    {

        public int UserId { get; set; }
        public string ProductIds { get; set; }
        public string FranchiseIds { get; set; }
    }
    public class CartSProductId
    {
        public int UserId { get; set; }
        public int Category { get; set; }
        public string ProductId { get; set; }
        public string FranchiseId { get; set; }
    }
}
