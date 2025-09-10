using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
  public class Product
  {
    //public string Heading { get; set; }
    public int ProductId { get; set; }
    public string ProductTitle { get; set; }
    public string Materials { get; set; }
    public string Length { get; set; }
    public string Washcare { get; set; }
    public string Description { get; set; }
    public double MinQuantity { get; set; }
    public double Price { get; set; }
    public double GokurtisPrice { get; set; }
    public double GST { get; set; }
    public double Total { get; set; }
    public double DiscountPercent { get; set; }
    public double SalePrice { get; set; }
    public string MainImage { get; set; }
    public string OtherImage1 { get; set; }
    public string OtherImage2 { get; set; }
    public string OtherImage3 { get; set; }
    public string Specification { get; set; }
    public string DataSheet { get; set; }
    public string MetaTag { get; set; }
    public string MetaDescription { get; set; }
    public string QuickStartGuide { get; set; }
    public int BrandId { get; set; }
    public string BrandName { get; set; }
    public string BrandUrl { get; set; }
    public string RelatedProducts { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int DisplayOrder { get; set; }
    public string ProductLabel { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int CreatedBy { get; set; }
    public int ModifiedBy { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted {  get; set; }
    public string SpType { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public int WarrantyInYear { get; set; }
    public string FranchiseCount { get; set; }
    public string ProductKeywords { get; set; }
    public string SKU { get; set; }
    public string USP { get; set; }
    public int ProductType { get; set; }
    public double RewardPoint { get; set; }
    public string USPs { get; set; }
    public string Features { get; set; }
    public string Declaration { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public string ColorImage { get; set; }
    public string ChildCategory {  get; set; }
    public string CategoryG {  get; set; }
    public string SubCategoryG {  get; set; }
    public string ChildCategoryG {  get; set; }
  }


  public class CheckoutProduct
  {
    //ProductId, ProductTitle, MinQuantity, SalePrice, Price, DiscountPercent, MainImage, BrandName, CategoryName

    public int ProductId { get; set; }
    public int FranchiseId { get; set; }
    public string ProductTitle { get; set; }
    public double MinQuantity { get; set; }
    public double Price { get; set; }
    public double SalePrice { get; set; }
    public double DiscountPercent { get; set; }
    public string MainImage { get; set; }
    public string BrandName { get; set; }
    public string CategoryName { get; set; }


  }
  public class filterProduct
  {
   public List<Product> products = new List<Product>();
   public List<Product> colors = new List<Product>();
   public List<Sizes> sizes = new List<Sizes>();
  }
}
