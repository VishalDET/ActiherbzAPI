using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace SynfoShopAPI.Models
{
    public class ProductInventory
    {
        public int ProductId { get; set; }
        public string MainImage { get; set; }
        public string ProductTitle { get; set; }
        public int Quantity { get; set; }
        public string BrandLogo { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public int NotExpiredQuantity { get; set; }
    }
    public class ProductInventoryDetails
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string MainImage { get; set; }
        public string ProductTitle { get; set; }
        public string MacId { get; set; }
        public string InvoiceNumber { get; set; }
        public string WarrantyStatus { get; set; }
        public string WarrantyExpiryDate { get; set; }
        public string InvoiceNumberUrl { get; set; }
        public string CnNumber { get; set; }
        public string NewMacId { get; set; }
        public int RMAStatus { get; set; }
        public int RMAFiled { get; set; }
    }
    public class ProductInventoryDetailsView
    {
        public ProductInventory ProductInventory { get; set; }
        public List<ProductInventoryDetails> ProductInventoryDetails { get; set; }
    }
}
