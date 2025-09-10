using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Twilio.TwiML.Voice;

namespace SynfoShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CommonProductController(IConfiguration config)
        {
            _config = config;
        }


        [HttpPost]
        public IActionResult CommonProduct(Product product)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonproduct", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_ProductId", product.ProductId);
                    command.Parameters.AddWithValue("_ProductTitle", product.ProductTitle);
                    command.Parameters.AddWithValue("_Materials", product.Materials);
                    command.Parameters.AddWithValue("_Length", product.Length);
                    command.Parameters.AddWithValue("_Washcare", product.Washcare);
                    command.Parameters.AddWithValue("_Description", product.Description);
                    command.Parameters.AddWithValue("_MinQuantity", product.MinQuantity);
                    command.Parameters.AddWithValue("_Price", product.Price);
                    // command.Parameters.AddWithValue("_ChildCategory", product.ChildCategory);
                    command.Parameters.AddWithValue("_GST", product.GST);
                    command.Parameters.AddWithValue("_Total", product.Total);
                    command.Parameters.AddWithValue("_SalePrice", product.SalePrice);
                    command.Parameters.AddWithValue("_MainImage", product.MainImage);
                    command.Parameters.AddWithValue("_OtherImage1", product.OtherImage1);
                    command.Parameters.AddWithValue("_OtherImage2", product.OtherImage2);
                    command.Parameters.AddWithValue("_OtherImage3", product.OtherImage3);
                    command.Parameters.AddWithValue("_Specification", product.Specification);
                    command.Parameters.AddWithValue("_DataSheet", product.DataSheet);
                    command.Parameters.AddWithValue("_MetaTag", product.MetaTag);
                    command.Parameters.AddWithValue("_MetaDescription", product.MetaDescription);
                    command.Parameters.AddWithValue("_QuickStartGuide", product.QuickStartGuide);
                    command.Parameters.AddWithValue("_BrandId", product.BrandId);
                    command.Parameters.AddWithValue("_RelatedProducts", product.RelatedProducts);
                    command.Parameters.AddWithValue("_CategoryId", product.CategoryId);
                    command.Parameters.AddWithValue("_DisplayOrder", product.DisplayOrder);
                    command.Parameters.AddWithValue("_ProductLabel", product.ProductLabel);
                    command.Parameters.AddWithValue("_StockQuantity", product.StockQuantity);
                    command.Parameters.AddWithValue("_CreatedBy", product.CreatedBy);
                    command.Parameters.AddWithValue("_CreatedDate", product.CreatedDate);
                    command.Parameters.AddWithValue("_ModifiedBy", product.ModifiedBy);
                    command.Parameters.AddWithValue("_ModifiedDate", product.ModifiedDate);
                    command.Parameters.AddWithValue("_IsActive", product.IsActive);
                    command.Parameters.AddWithValue("_IsDeleted", product.IsDeleted);
                    command.Parameters.AddWithValue("_SpType", product.SpType);
                    command.Parameters.AddWithValue("_SKU", product.SKU);
                    //command.Parameters.AddWithValue("_SKU", product.CategoryName); ;
                    command.Parameters.AddWithValue("_ProductType", product.ProductType);
                    command.Parameters.AddWithValue("_RewardPoint", product.RewardPoint);
                    command.Parameters.AddWithValue("_WarrantyInYear", product.WarrantyInYear);
                    command.Parameters.AddWithValue("_USPs", product.USPs);
                    command.Parameters.AddWithValue("_ProductKeywords", product.ProductKeywords);
                    command.Parameters.AddWithValue("FranchiseCount", 0);
                    command.Parameters.AddWithValue("_Color", 0);
                    command.Parameters.AddWithValue("_Size", 0);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (product.SpType == "C" || product.SpType == "U" || product.SpType == "D")
                    {
                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);
                        if (result != 0)
                        {
                            string[] categoryIds = product.ChildCategory.Split(',');
                            foreach (string id in categoryIds)
                            {
                                MySqlCommand childCommand = new MySqlCommand("usp_categorymapping", connection);
                                childCommand.CommandType = System.Data.CommandType.StoredProcedure;
                                {
                                    childCommand.Parameters.AddWithValue("_ProductId", result);
                                    childCommand.Parameters.AddWithValue("_CategoryId", Convert.ToInt32(id));
                                };
                                childCommand.ExecuteNonQuery();
                            }
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                    }
                    else if (product.SpType == "E" || product.SpType == "R" || product.SpType == "LS")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Product> productList = new List<Product>();

                                while (reader.Read())
                                {
                                    var productModel = new Product
                                    {
                                        ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        ProductTitle = reader.IsDBNull(reader.GetOrdinal("ProductTitle")) ? null : reader.GetString(reader.GetOrdinal("ProductTitle")),
                                        Materials = reader.IsDBNull(reader.GetOrdinal("Materials")) ? null : reader.GetString(reader.GetOrdinal("Materials")),
                                        Length = reader.IsDBNull(reader.GetOrdinal("Length")) ? null : reader.GetString(reader.GetOrdinal("Length")),
                                        Washcare = reader.IsDBNull(reader.GetOrdinal("Washcare")) ? null : reader.GetString(reader.GetOrdinal("Washcare")),
                                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                        MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble(reader.GetOrdinal("MinQuantity")),
                                        Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble(reader.GetOrdinal("Price")),
                                        // GokurtisPrice = reader.IsDBNull(reader.GetOrdinal("GokurtisPrice")) ? 0 : reader.GetDouble(reader.GetOrdinal("GokurtisPrice")),
                                        //GST = reader.IsDBNull(reader.GetOrdinal("GST")) ? 0 : reader.GetDouble(reader.GetOrdinal("GST")),
                                        // Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetDouble(reader.GetOrdinal("Total")),
                                        DiscountPercent = reader.IsDBNull(reader.GetOrdinal("DiscountPercent")) ? 0 : reader.GetDouble(reader.GetOrdinal("DiscountPercent")),
                                        SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble(reader.GetOrdinal("SalePrice")),
                                        MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString(reader.GetOrdinal("MainImage")),
                                        OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? null : reader.GetString(reader.GetOrdinal("OtherImage1")),
                                        OtherImage2 = reader.IsDBNull(reader.GetOrdinal("OtherImage2")) ? null : reader.GetString(reader.GetOrdinal("OtherImage2")),
                                        OtherImage3 = reader.IsDBNull(reader.GetOrdinal("OtherImage3")) ? null : reader.GetString(reader.GetOrdinal("OtherImage3")),
                                        Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : reader.GetString(reader.GetOrdinal("Specification")),
                                        DataSheet = reader.IsDBNull(reader.GetOrdinal("DataSheet")) ? null : reader.GetString(reader.GetOrdinal("DataSheet")),
                                        MetaTag = reader.IsDBNull(reader.GetOrdinal("MetaTag")) ? null : reader.GetString(reader.GetOrdinal("MetaTag")),
                                        MetaDescription = reader.IsDBNull(reader.GetOrdinal("MetaDescription")) ? null : reader.GetString(reader.GetOrdinal("MetaDescription")),
                                        QuickStartGuide = reader.IsDBNull(reader.GetOrdinal("QuickStartGuide")) ? null : reader.GetString(reader.GetOrdinal("QuickStartGuide")),
                                        BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32(reader.GetOrdinal("BrandId")),
                                        BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString(reader.GetOrdinal("BrandName")),
                                        // BrandUrl = reader.IsDBNull(reader.GetOrdinal("BrandUrl")) ? null : reader.GetString(reader.GetOrdinal("BrandUrl")),
                                        RelatedProducts = reader.IsDBNull(reader.GetOrdinal("RelatedProducts")) ? null : reader.GetString(reader.GetOrdinal("RelatedProducts")),
                                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                        //  CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString(reader.GetOrdinal("CategoryName")),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32(reader.GetOrdinal("DisplayOrder")),
                                        ProductLabel = reader.IsDBNull(reader.GetOrdinal("ProductLabel")) ? null : reader.GetString(reader.GetOrdinal("ProductLabel")),
                                        StockQuantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("ModifiedBy")),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        //  IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                        //  SpType = reader.IsDBNull(reader.GetOrdinal("SpType")) ? null : reader.GetString(reader.GetOrdinal("SpType")),
                                        // Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                                        // SubCategory = reader.IsDBNull(reader.GetOrdinal("SubCategory")) ? null : reader.GetString(reader.GetOrdinal("SubCategory")),
                                        WarrantyInYear = reader.IsDBNull(reader.GetOrdinal("WarrantyInYear")) ? 0 : reader.GetInt32(reader.GetOrdinal("WarrantyInYear")),
                                        //  FranchiseCount = reader.IsDBNull(reader.GetOrdinal("FranchiseCount")) ? null : reader.GetString(reader.GetOrdinal("FranchiseCount")),
                                        ProductKeywords = reader.IsDBNull(reader.GetOrdinal("ProductKeywords")) ? null : reader.GetString(reader.GetOrdinal("ProductKeywords")),
                                        SKU = reader.IsDBNull(reader.GetOrdinal("SKU")) ? null : reader.GetString(reader.GetOrdinal("SKU")),
                                        // USP = reader.IsDBNull(reader.GetOrdinal("USP")) ? null : reader.GetString(reader.GetOrdinal("USP")),
                                        ProductType = reader.IsDBNull(reader.GetOrdinal("ProductType")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProductType")),
                                        RewardPoint = reader.IsDBNull(reader.GetOrdinal("RewardPoint")) ? 0 : reader.GetDouble(reader.GetOrdinal("RewardPoint")),
                                        USPs = reader.IsDBNull(reader.GetOrdinal("USPs")) ? null : reader.GetString(reader.GetOrdinal("USPs")),
                                        Features = reader.IsDBNull(reader.GetOrdinal("Features")) ? null : reader.GetString(reader.GetOrdinal("Features")),
                                        Declaration = reader.IsDBNull(reader.GetOrdinal("Declaration")) ? null : reader.GetString(reader.GetOrdinal("Declaration")),
                                        Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color")),
                                        Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString(reader.GetOrdinal("Size")),
                                        CategoryG = reader.IsDBNull(reader.GetOrdinal("CategoryG")) ? null : reader.GetString(reader.GetOrdinal("CategoryG")),
                                        SubCategoryG = reader.IsDBNull(reader.GetOrdinal("SubCategoryG")) ? null : reader.GetString(reader.GetOrdinal("SubCategoryG")),
                                        ChildCategoryG = reader.IsDBNull(reader.GetOrdinal("ChildCategoryG")) ? null : reader.GetString(reader.GetOrdinal("ChildCategoryG")),
                                        // ColorImage = reader.IsDBNull(reader.GetOrdinal("ColorImage")) ? null : reader.GetString(reader.GetOrdinal("ColorImage")),
                                        // ChildCategory = reader.IsDBNull(reader.GetOrdinal("ChildCategory")) ? null : reader.GetString(reader.GetOrdinal("ChildCategory")),

                                    };
                                    //var productModel = new Product
                                    //{
                                    //  ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                    //  ProductTitle = reader.IsDBNull(reader.GetOrdinal("ProductTitle")) ? null : reader.GetString("ProductTitle"),
                                    //  Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),

                                    //    Materials = reader.IsDBNull(reader.GetOrdinal("Materials")) ? null : reader.GetString(reader.GetOrdinal("Materials")),
                                    //    Length = reader.IsDBNull(reader.GetOrdinal("Length")) ? null : reader.GetString(reader.GetOrdinal("Length")),
                                    //    Washcare = reader.IsDBNull(reader.GetOrdinal("Washcare")) ? null : reader.GetString(reader.GetOrdinal("Washcare")),


                                    //    MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble("MinQuantity"),
                                    //  Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price"),
                                    //  SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice"),
                                    //  MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage"),
                                    //  OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? null : reader.GetString("OtherImage1"),
                                    //  OtherImage2 = reader.IsDBNull(reader.GetOrdinal("OtherImage2")) ? null : reader.GetString("OtherImage2"),
                                    //  OtherImage3 = reader.IsDBNull(reader.GetOrdinal("OtherImage3")) ? null : reader.GetString("OtherImage3"),
                                    //  Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : reader.GetString("Specification"),
                                    //  DataSheet = reader.IsDBNull(reader.GetOrdinal("DataSheet")) ? null : reader.GetString("DataSheet"),
                                    //  MetaTag = reader.IsDBNull(reader.GetOrdinal("MetaTag")) ? null : reader.GetString("MetaTag"),
                                    //  MetaDescription = reader.IsDBNull(reader.GetOrdinal("MetaDescription")) ? null : reader.GetString("MetaDescription"),
                                    //  QuickStartGuide = reader.IsDBNull(reader.GetOrdinal("QuickStartGuide")) ? null : reader.GetString("QuickStartGuide"),
                                    //  BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32("BrandId"),
                                    //  BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString("BrandName"),
                                    //  RelatedProducts = reader.IsDBNull(reader.GetOrdinal("RelatedProducts")) ? null : reader.GetString("RelatedProducts"),
                                    //  CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId"),
                                    ////  CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString("CategoryName"),
                                    //  DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                    //  ProductLabel = reader.IsDBNull(reader.GetOrdinal("ProductLabel")) ? null : reader.GetString("ProductLabel"),
                                    //  StockQuantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32("StockQuantity"),
                                    //  CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                    //  ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate"),
                                    //  CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32("CreatedBy"),
                                    //  ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? 0 : reader.GetInt32("ModifiedBy"),
                                    //  IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    ////  IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                    //  SKU = reader.IsDBNull(reader.GetOrdinal("SKU")) ? null : reader.GetString("SKU"),
                                    //  ProductType = reader.IsDBNull(reader.GetOrdinal("ProductType")) ? 0 : reader.GetInt32("ProductType"),
                                    //  RewardPoint = reader.IsDBNull(reader.GetOrdinal("RewardPoint")) ? 0 : reader.GetInt32("RewardPoint"),
                                    //  WarrantyInYear = reader.IsDBNull(reader.GetOrdinal("WarrantyInYear")) ? 0 : reader.GetInt32("WarrantyInYear"),
                                    //  USPs = reader.IsDBNull(reader.GetOrdinal("USPs")) ? null : reader.GetString("USPs"),
                                    //  ProductKeywords = reader.IsDBNull(reader.GetOrdinal("ProductKeywords")) ? null : reader.GetString("ProductKeywords"),
                                    //  Features = reader.IsDBNull(reader.GetOrdinal("Features")) ? null : reader.GetString("Features"),
                                    //  Declaration = reader.IsDBNull(reader.GetOrdinal("Declaration")) ? null : reader.GetString("Declaration"),
                                    //  Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString("Color"),
                                    //  ColorImage = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString("Image"),
                                    //  Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString("Size"),
                                    //};

                                    productList.Add(productModel);
                                }

                                //return Ok(productList);
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = productList;
                                return Ok(apiResult);
                            }
                            else
                            {
                                //return NotFound();
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { };
                                return NotFound(apiResult);
                            }
                        }
                    }
                    else
                    {
                        //return BadRequest("Invalid SpType");
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                        apiResult.data = new { };
                        return BadRequest(apiResult);
                    }
                }

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }

        }

    }
}
