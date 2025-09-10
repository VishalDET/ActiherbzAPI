using iText.Kernel.Colors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Twilio.Types;
//using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace SynfoShopAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductsController(IConfiguration config)
        {
            _config = config;
        }

        [Route("api/GetProductsAll")]
        [HttpGet]
        public IActionResult GetProductsAll()
        {
            try
            {
                List<Product> productList = new List<Product>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getproductsall", connection); // Update the stored procedure name
                    command.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                //Heading = reader["Heading"].ToString(),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                Description = reader["Description"].ToString(),
                                MinQuantity = reader["MinQuantity"] != DBNull.Value ? Convert.ToDouble(reader["MinQuantity"]) : 0.0,
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDouble(reader["Price"]) : 0.0,
                                DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(reader["DiscountPercent"]) : 0.0,
                                SalePrice = reader["SalePrice"] != DBNull.Value ? Convert.ToDouble(reader["SalePrice"]) : 0.0,
                                MainImage = reader["MainImage"].ToString(),
                                OtherImage1 = reader["OtherImage1"].ToString(),
                                OtherImage2 = reader["OtherImage2"].ToString(),
                                OtherImage3 = reader["OtherImage3"].ToString(),
                                Specification = reader["Specification"].ToString(),
                                DataSheet = reader["DataSheet"].ToString(),
                                MetaTag = reader["MetaTag"].ToString(),
                                MetaDescription = reader["MetaDescription"].ToString(),
                                QuickStartGuide = reader["QuickStartGuide"].ToString(),
                                BrandId = Convert.ToInt32(reader["BrandId"]),
                                BrandName = reader["BrandName"].ToString(),
                                RelatedProducts = reader["RelatedProducts"].ToString(),
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                CategoryName = reader["CategoryName"].ToString(),
                                DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(reader["DisplayOrder"]) : 0,
                                ProductLabel = reader["ProductLabel"].ToString(),
                                StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : DateTime.MinValue,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Convert.ToInt32(reader["CreatedBy"]) : 0,
                                ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(reader["ModifiedBy"]) : 0,
                                IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]),
                                ColorImage = reader["Image"].ToString(),
                                Size = reader["Size"].ToString()
                            };

                            productList.Add(product);
                        }
                    }
                }

                if (productList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = productList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Brand>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetProduct")]
        [HttpPost]
        public IActionResult GetProducts(int categoryId)
        {
            try
            {
                List<Product> productList = new List<Product>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getproducts", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_CategoryId", categoryId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                Description = reader["Description"].ToString(),
                                MinQuantity = reader["MinQuantity"] != DBNull.Value ? Convert.ToDouble(reader["MinQuantity"]) : 0.00,
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDouble(reader["Price"]) : 0.0,
                                DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(reader["DiscountPercent"]) : 0.0,
                                SalePrice = reader["SalePrice"] != DBNull.Value ? Convert.ToDouble(reader["SalePrice"]) : 0.0,
                                MainImage = reader["MainImage"].ToString(),
                                OtherImage1 = reader["OtherImage1"].ToString(),
                                OtherImage2 = reader["OtherImage2"].ToString(),
                                OtherImage3 = reader["OtherImage3"].ToString(),
                                Specification = reader["Specification"].ToString(),
                                DataSheet = reader["DataSheet"].ToString(),
                                MetaTag = reader["MetaTag"].ToString(),
                                MetaDescription = reader["MetaDescription"].ToString(),
                                QuickStartGuide = reader["QuickStartGuide"].ToString(),
                                BrandId = Convert.ToInt32(reader["BrandId"]),
                                BrandName = reader["BrandName"].ToString(),
                                RelatedProducts = reader["RelatedProducts"].ToString(),
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                CategoryName = reader["CategoryName"].ToString(),
                                DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(reader["DisplayOrder"]) : 0,
                                ProductLabel = reader["ProductLabel"].ToString(),
                                StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : DateTime.MinValue,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Convert.ToInt32(reader["CreatedBy"]) : 0,
                                ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(reader["ModifiedBy"]) : 0,
                                IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"])
                            };

                            productList.Add(product);
                        }
                    }
                }

                if (productList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = productList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Brand>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetProductsAndPackages")]
        [HttpPost]
        public ActionResult<IEnumerable<ProductData>> GetProductsAndPackages(int categoryId, string state, int userId)
        {
            List<ProductData> productDataList = new List<ProductData>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("usp_getproducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_CategoryId", categoryId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = Convert.ToInt32(reader["ProductId"]);
                            string productTitle = Convert.ToString(reader["ProductTitle"]);

                            string Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description");
                            double MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble("MinQuantity");
                            double Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price");
                            double SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice");
                            string MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage");
                            string OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? null : reader.GetString("OtherImage1");
                            string OtherImage2 = reader.IsDBNull(reader.GetOrdinal("OtherImage2")) ? null : reader.GetString("OtherImage2");
                            string OtherImage3 = reader.IsDBNull(reader.GetOrdinal("OtherImage3")) ? null : reader.GetString("OtherImage3");
                            string Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : reader.GetString("Specification");
                            string DataSheet = reader.IsDBNull(reader.GetOrdinal("DataSheet")) ? null : reader.GetString("DataSheet");
                            string MetaTag = reader.IsDBNull(reader.GetOrdinal("MetaTag")) ? null : reader.GetString("MetaTag");
                            string MetaDescription = reader.IsDBNull(reader.GetOrdinal("MetaDescription")) ? null : reader.GetString("MetaDescription");
                            string QuickStartGuide = reader.IsDBNull(reader.GetOrdinal("QuickStartGuide")) ? null : reader.GetString("QuickStartGuide");
                            int BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32("BrandId");
                            string BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString("BrandName");
                            string BrandUrl = reader.IsDBNull(reader.GetOrdinal("BrandUrl")) ? null : reader.GetString("BrandUrl");
                            string RelatedProducts = reader.IsDBNull(reader.GetOrdinal("RelatedProducts")) ? null : reader.GetString("RelatedProducts");
                            int CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId");
                            int DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder");
                            string ProductLabel = reader.IsDBNull(reader.GetOrdinal("ProductLabel")) ? null : reader.GetString("ProductLabel");
                            int StockQuantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32("StockQuantity");
                            string PriceRange = reader.IsDBNull(reader.GetOrdinal("PriceRange")) ? null : reader.GetString("PriceRange");


                            var productData = new ProductData
                            {
                                PriceRange = PriceRange,
                                ProductId = productId,
                                ProductTitle = productTitle,
                                Description = Description,
                                MinQuantity = MinQuantity,
                                Price = Price,
                                SalePrice = SalePrice,
                                MainImage = MainImage,
                                OtherImage1 = OtherImage1,
                                OtherImage2 = OtherImage2,
                                OtherImage3 = OtherImage3,
                                Specification = Specification,
                                DataSheet = DataSheet,
                                MetaTag = MetaTag,
                                MetaDescription = MetaDescription,
                                QuickStartGuide = QuickStartGuide,
                                BrandId = BrandId,
                                BrandName = BrandName,
                                BrandUrl = BrandUrl,
                                RelatedProducts = RelatedProducts,
                                CategoryId = CategoryId,
                                DisplayOrder = DisplayOrder,
                                ProductLabel = ProductLabel,
                                StockQuantity = StockQuantity,

                                FranchisePackage = new List<franchisePackage>()
                            };

                            productDataList.Add(productData);
                        }
                    }
                }

                foreach (var productData in productDataList)
                {
                    int productId = productData.ProductId;

                    using (MySqlCommand packageCommand = new MySqlCommand("usp_getproductpackage_temp", connection))
                    {
                        packageCommand.CommandType = CommandType.StoredProcedure;
                        packageCommand.Parameters.AddWithValue("_ProductId", productId);
                        packageCommand.Parameters.AddWithValue("_State", state);
                        packageCommand.Parameters.AddWithValue("_UserId", userId);

                        DataTable dtCity = new DataTable();
                        DataSet ds = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(packageCommand);
                        da.Fill(ds);

                        DataTable dtPackages = new DataTable();
                        dtPackages = ds.Tables[0];
                        dtCity = ds.Tables[1];

                        foreach (DataRow dr in dtCity.Rows)
                        {
                            DataTable _dtNew = new DataTable();
                            _dtNew = dtPackages.AsEnumerable().Where(p => p["city"].ToString() == dr["city"].ToString() && Convert.ToInt32(p["FranchiseId"]) == Convert.ToInt32(dr["FranchiseId"])).CopyToDataTable();

                            List<ProductPackage> _packages = new List<ProductPackage>();

                            foreach (DataRow dr1 in _dtNew.Rows)
                            {
                                ProductPackage _productPackage = new ProductPackage
                                {
                                    Id = Convert.ToInt32(dr1["Id"]),
                                    ProductId = Convert.ToInt32(dr1["ProductId"]),
                                    FranchiseId = Convert.ToInt32(dr1["FranchiseId"]),
                                    PackageName = dr1["PackageName"].ToString(),
                                    Color = dr1["Color"].ToString(),
                                    ColorText = dr1["ColorText"].ToString(),
                                    SizeText = dr1["SizeText"].ToString(),
                                    Image = dr1["Image"].ToString(),
                                    Size = dr1["Size"].ToString(),
                                    city = dr1["city"].ToString(),
                                    State = dr1["state"].ToString(),
                                    Price = dr1["Price"] != DBNull.Value ? Convert.ToDouble(dr1["Price"]) : 0.0,
                                    DiscountPercent = dr1["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(dr1["DiscountPercent"]) : 0.0,
                                    SalePrice = dr1["SalePrice"] != DBNull.Value ? Convert.ToDouble(dr1["SalePrice"]) : 0.0,
                                    MinQuantity = dr1["MinQuantity"] != DBNull.Value ? Convert.ToDouble(dr1["MinQuantity"]) : 0.0,
                                    DisplayOrder = dr1["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["DisplayOrder"])) : 0,
                                    StockQuantity = dr1["StockQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["StockQuantity"])) : 0,
                                    AddedQuantity = dr1["AddedQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["AddedQuantity"])) : 0,
                                    IsActive = dr1["IsActive"] != DBNull.Value && Convert.ToBoolean(dr1["IsActive"])
                                };
                                _packages.Add(_productPackage);

                            }

                            franchisePackage _franchisePackage = new franchisePackage()
                            {
                                Color = Convert.ToInt32(dr["Color"]),
                                ColorText = dr["ColorText"].ToString(),
                                ColorCode = dr["ColorCode"].ToString(),
                                city = dr["city"].ToString(),
                                DeliveryBy = dr["DeliveryBy"].ToString(),
                                FranchiseId = Convert.ToInt32(dr["FranchiseId"]),
                                Price = Convert.ToDouble(dr["Price"]),
                                Packages = _packages,

                            };

                            productData.FranchisePackage.Add(_franchisePackage);
                        }
                    }
                }
            }
            if (productDataList.Count > 0)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = productDataList;
                return Ok(apiResult);
            }
            else
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = new List<Brand>();
                return NotFound(apiResult);
            }
        }


        [Route("api/GetProductsAndPackagesByProductId")]
        [HttpPost]
        public ActionResult<IEnumerable<ProductData>> GetProductsAndPackagesByProductId(int ProductId, int UserId)
        {
            List<ProductData> productDataList = new List<ProductData>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("usp_getproductdetailsbyIdwebsite", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_ProductId", ProductId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = Convert.ToInt32(reader["ProductId"]);
                            int PackageId = Convert.ToInt32(reader["PackageId"]);
                            string productTitle = Convert.ToString(reader["ProductTitle"]);
                            string Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description");
                            string Materials = reader.IsDBNull(reader.GetOrdinal("Materials")) ? null : reader.GetString("Materials");
                            double MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble("MinQuantity");
                            double Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price");
                            double SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice");
                            double DiscountPercent = reader.IsDBNull(reader.GetOrdinal("DiscountPercent")) ? 0 : reader.GetDouble("DiscountPercent");
                            string MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage");
                            string OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? null : reader.GetString("OtherImage1");
                            string OtherImage2 = reader.IsDBNull(reader.GetOrdinal("OtherImage2")) ? null : reader.GetString("OtherImage2");
                            string OtherImage3 = reader.IsDBNull(reader.GetOrdinal("OtherImage3")) ? null : reader.GetString("OtherImage3");
                            string Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : reader.GetString("Specification");
                            string DataSheet = reader.IsDBNull(reader.GetOrdinal("DataSheet")) ? null : reader.GetString("DataSheet");
                            string MetaTag = reader.IsDBNull(reader.GetOrdinal("MetaTag")) ? null : reader.GetString("MetaTag");
                            string MetaDescription = reader.IsDBNull(reader.GetOrdinal("MetaDescription")) ? null : reader.GetString("MetaDescription");
                            string QuickStartGuide = reader.IsDBNull(reader.GetOrdinal("QuickStartGuide")) ? null : reader.GetString("QuickStartGuide");
                            int BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32("BrandId");
                            string BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString("BrandName");
                            //string BrandUrl = reader.IsDBNull(reader.GetOrdinal("BrandUrl")) ? null : reader.GetString("BrandUrl");
                            string RelatedProducts = reader.IsDBNull(reader.GetOrdinal("RelatedProducts")) ? null : reader.GetString("RelatedProducts");
                            int CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId");
                            int DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder");
                            string ProductLabel = reader.IsDBNull(reader.GetOrdinal("ProductLabel")) ? null : reader.GetString("ProductLabel");
                            int StockQuantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32("StockQuantity");
                            string PriceRange = reader.IsDBNull(reader.GetOrdinal("PriceRange")) ? null : reader.GetString("PriceRange");
                            string USPs = reader.IsDBNull(reader.GetOrdinal("USPs")) ? null : reader.GetString("USPs");
                            string SKU = reader.IsDBNull(reader.GetOrdinal("SKU")) ? null : reader.GetString("SKU");
                            string Feature = reader.IsDBNull(reader.GetOrdinal("Features")) ? null : reader.GetString("Features");
                            string Declaration = reader.IsDBNull(reader.GetOrdinal("Declaration")) ? null : reader.GetString("Declaration");

                            var productData = new ProductData
                            {
                                PriceRange = PriceRange,
                                ProductId = productId,
                                PackageId = PackageId,
                                ProductTitle = productTitle,
                                Description = Description,
                                MinQuantity = MinQuantity,
                                Price = Price,
                                SalePrice = SalePrice,
                                DiscountPercent = DiscountPercent,
                                Materials = Materials,
                                MainImage = MainImage,
                                OtherImage1 = OtherImage1,
                                OtherImage2 = OtherImage2,
                                OtherImage3 = OtherImage3,
                                Specification = Specification,
                                DataSheet = DataSheet,
                                MetaTag = MetaTag,
                                MetaDescription = MetaDescription,
                                QuickStartGuide = QuickStartGuide,
                                BrandId = BrandId,
                                BrandName = BrandName,
                                //BrandUrl = BrandUrl,
                                RelatedProducts = RelatedProducts,
                                CategoryId = CategoryId,
                                DisplayOrder = DisplayOrder,
                                ProductLabel = ProductLabel,
                                StockQuantity = StockQuantity,
                                USPs = USPs,
                                SKU = SKU,
                                Features = Feature,
                                Declaration = Declaration,
                                FranchisePackage = new List<franchisePackage>()
                            };

                            productDataList.Add(productData);
                        }
                    }
                }

                foreach (var productData in productDataList)
                {
                    int productId = productData.ProductId;

                    using (MySqlCommand packageCommand = new MySqlCommand("usp_getproductpackage_temp", connection))
                    {
                        packageCommand.CommandType = CommandType.StoredProcedure;
                        packageCommand.Parameters.AddWithValue("_ProductId", productId);
                        packageCommand.Parameters.AddWithValue("_State", 0);
                        packageCommand.Parameters.AddWithValue("_UserId", 1); //UserId

                        DataTable dtCity = new DataTable();
                        DataSet ds = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(packageCommand);
                        da.Fill(ds);

                        DataTable dtPackages = new DataTable();
                        dtPackages = ds.Tables[0];
                        dtCity = ds.Tables[1];

                        foreach (DataRow dr in dtCity.Rows)
                        {
                            DataTable _dtNew = new DataTable();
                            _dtNew = dtPackages.AsEnumerable().Where(p => Convert.ToInt32(p["Color"]) == Convert.ToInt32(dr["Color"])).CopyToDataTable();

                            List<ProductPackage> _packages = new List<ProductPackage>();

                            foreach (DataRow dr1 in _dtNew.Rows)
                            {
                                ProductPackage _productPackage = new ProductPackage
                                {
                                    Id = Convert.ToInt32(dr1["Id"]),
                                    ProductId = Convert.ToInt32(dr1["ProductId"]),
                                    PackageId = Convert.ToInt32(dr1["PackageId"]),
                                    FranchiseId = Convert.ToInt32(dr1["FranchiseId"]),
                                    PackageName = dr1["PackageName"].ToString(),
                                    Color = dr1["Color"].ToString(),
                                    ColorText = dr1["ColorText"].ToString(),
                                    SizeText = dr1["SizeText"].ToString(),
                                    Image = dr1["Image"].ToString(),
                                    Size = dr1["Size"].ToString(),
                                    city = dr1["city"].ToString(),
                                    State = dr1["state"].ToString(),
                                    Price = dr1["Price"] != DBNull.Value ? Convert.ToDouble(dr1["Price"]) : 0.0,
                                    DiscountPercent = dr1["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(dr1["DiscountPercent"]) : 0.0,
                                    SalePrice = dr1["SalePrice"] != DBNull.Value ? Convert.ToDouble(dr1["SalePrice"]) : 0.0,
                                    MinQuantity = dr1["MinQuantity"] != DBNull.Value ? Convert.ToDouble(dr1["MinQuantity"]) : 0.0,
                                    DisplayOrder = dr1["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["DisplayOrder"])) : 0,
                                    StockQuantity = dr1["StockQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["StockQuantity"])) : 0,
                                    AddedQuantity = dr1["AddedQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["AddedQuantity"])) : 0,
                                    IsActive = dr1["IsActive"] != DBNull.Value && Convert.ToBoolean(dr1["IsActive"])
                                };
                                _packages.Add(_productPackage);

                            }

                            franchisePackage _franchisePackage = new franchisePackage()
                            {
                                city = dr["city"].ToString(),
                                Color = Convert.ToInt32(dr["Color"]),
                                ColorText = dr["ColorText"].ToString(),
                                ColorCode = dr["ColorCode"].ToString(),
                                DeliveryBy = dr["DeliveryBy"].ToString(),
                                FranchiseId = Convert.ToInt32(dr["FranchiseId"]),
                                Price = Convert.ToDouble(dr["Price"]),
                                Packages = _packages,

                            };

                            productData.FranchisePackage.Add(_franchisePackage);
                        }
                    }
                }
            }
            if (productDataList.Count > 0)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = productDataList;
                return Ok(apiResult);
            }
            else
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = new List<Brand>();
                return NotFound(apiResult);
            }
        }


        [Route("api/GetBeforeyouCheckoutProductsAndPackages")]
        [HttpPost]
        public ActionResult<IEnumerable<CheckoutProduct>> GetBeforeyouCheckoutProductsAndPackages(string franchise_ids, string ProductIds, int userId)
        {
            List<CheckoutProductData> productDataList = new List<CheckoutProductData>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("usp_getcheckoutsuggestedproduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("franchise_ids", franchise_ids);
                    command.Parameters.AddWithValue("ProductIds", ProductIds);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {//ProductId, ProductTitle, MinQuantity, SalePrice, Price, DiscountPercent, MainImage, BrandName, CategoryName
                            int productId = Convert.ToInt32(reader["ProductId"]);
                            string productTitle = Convert.ToString(reader["ProductTitle"]);
                            double MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble("MinQuantity");
                            double Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price");
                            double SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice");
                            string MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage");

                            var productData = new CheckoutProductData
                            {
                                ProductId = productId,
                                ProductTitle = productTitle,
                                MinQuantity = MinQuantity,
                                Price = Price,
                                SalePrice = SalePrice,
                                MainImage = MainImage,
                                FranchisePackage = new List<franchisePackage>()
                            };
                            productDataList.Add(productData);
                        }
                    }
                }

                foreach (var productData in productDataList)
                {
                    int productId = productData.ProductId;

                    using (MySqlCommand packageCommand = new MySqlCommand("usp_getbestproductsbyfranchiseids", connection))
                    {
                        packageCommand.CommandType = CommandType.StoredProcedure;
                        packageCommand.Parameters.AddWithValue("franchise_ids", franchise_ids);
                        packageCommand.Parameters.AddWithValue("ProductIds", ProductIds);
                        packageCommand.Parameters.AddWithValue("_UserId", userId);

                        DataTable dtCity = new DataTable();
                        DataSet ds = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(packageCommand);
                        da.Fill(ds);

                        DataTable dtPackages = new DataTable();
                        dtPackages = ds.Tables[0];
                        dtCity = ds.Tables[1];

                        foreach (DataRow dr in dtCity.Rows)
                        {
                            DataTable _dtNew = new DataTable();
                            _dtNew = dtPackages.AsEnumerable().Where(p => p["city"].ToString() == dr["city"].ToString() && Convert.ToInt32(p["FranchiseId"]) == Convert.ToInt32(dr["FranchiseId"]) && Convert.ToInt32(dr["ProductId"]) == Convert.ToInt32(p["ProductId"])).CopyToDataTable();

                            List<ProductPackage> _packages = new List<ProductPackage>();

                            foreach (DataRow dr1 in _dtNew.Rows)
                            {
                                ProductPackage _productPackage = new ProductPackage
                                {
                                    Id = Convert.ToInt32(dr1["Id"]),
                                    ProductId = Convert.ToInt32(dr1["ProductId"]),
                                    FranchiseId = Convert.ToInt32(dr1["FranchiseId"]),
                                    PackageName = dr1["PackageName"].ToString(),
                                    city = dr1["city"].ToString(),
                                    State = dr1["state"].ToString(),
                                    Price = dr1["Price"] != DBNull.Value ? Convert.ToDouble(dr1["Price"]) : 0.0,
                                    DiscountPercent = dr1["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(dr1["DiscountPercent"]) : 0.0,
                                    SalePrice = dr1["SalePrice"] != DBNull.Value ? Convert.ToDouble(dr1["SalePrice"]) : 0.0,
                                    MinQuantity = dr1["MinQuantity"] != DBNull.Value ? Convert.ToDouble(dr1["MinQuantity"]) : 0.0,
                                    StockQuantity = dr1["StockQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["StockQuantity"])) : 0,
                                    AddedQuantity = dr1["AddedQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["AddedQuantity"])) : 0,
                                };
                                _packages.Add(_productPackage);

                            }

                            franchisePackage _franchisePackage = new franchisePackage()
                            {
                                city = dr["city"].ToString(),
                                DeliveryBy = dr["DeliveryBy"].ToString(),
                                FranchiseId = Convert.ToInt32(dr["FranchiseId"]),
                                Price = Convert.ToDouble(dr["Price"]),
                                Packages = _packages,

                            };

                            productData.FranchisePackage.Add(_franchisePackage);
                        }
                    }
                }
            }
            if (productDataList.Count > 0)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = productDataList;
                return Ok(apiResult);
            }
            else
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = new List<Brand>();
                return NotFound(apiResult);
            }
        }

        [Route("api/GetCheckoutSugProduct")]
        [HttpPost]
        public ActionResult<IEnumerable<CheckoutProduct>> usp_getcheckoutsugproduct(int userId)
        {
            List<CheckoutProductData> productDataList = new List<CheckoutProductData>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("usp_getcheckoutsugproduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_UserId", userId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {//ProductId, ProductTitle, MinQuantity, SalePrice, Price, DiscountPercent, MainImage, BrandName, CategoryName
                            int productId = Convert.ToInt32(reader["ProductId"]);
                            int franchiseId = Convert.ToInt32(reader["FranchiseId"]);
                            string productTitle = Convert.ToString(reader["ProductTitle"]);
                            double MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble("MinQuantity");
                            double Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price");
                            double SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice");
                            string MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage");

                            var productData = new CheckoutProductData
                            {
                                ProductId = productId,
                                FranchiseId = franchiseId,
                                ProductTitle = productTitle,
                                MinQuantity = MinQuantity,
                                Price = Price,
                                SalePrice = SalePrice,
                                MainImage = MainImage,
                                FranchisePackage = new List<franchisePackage>()
                            };
                            productDataList.Add(productData);
                        }
                    }
                }

                foreach (var productData in productDataList)
                {
                    int productId = productData.ProductId;
                    int franchiseId = productData.FranchiseId;

                    using (MySqlCommand packageCommand = new MySqlCommand("usp_getcheckoutsugpackage", connection))
                    {
                        packageCommand.CommandType = CommandType.StoredProcedure;
                        packageCommand.Parameters.AddWithValue("_franchiseId", franchiseId);
                        packageCommand.Parameters.AddWithValue("_ProductId", productId);
                        packageCommand.Parameters.AddWithValue("_UserId", userId);

                        DataTable dtCity = new DataTable();
                        DataSet ds = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(packageCommand);
                        da.Fill(ds);

                        DataTable dtPackages = new DataTable();
                        dtPackages = ds.Tables[0];
                        dtCity = ds.Tables[1];

                        foreach (DataRow dr in dtCity.Rows)
                        {
                            DataTable _dtNew = new DataTable();
                            _dtNew = dtPackages.AsEnumerable().Where(p => p["city"].ToString() == dr["city"].ToString() && Convert.ToInt32(p["FranchiseId"]) == Convert.ToInt32(dr["FranchiseId"])).CopyToDataTable();

                            List<ProductPackage> _packages = new List<ProductPackage>();

                            foreach (DataRow dr1 in _dtNew.Rows)
                            {
                                ProductPackage _productPackage = new ProductPackage
                                {
                                    Id = Convert.ToInt32(dr1["Id"]),
                                    ProductId = Convert.ToInt32(dr1["ProductId"]),
                                    FranchiseId = Convert.ToInt32(dr1["FranchiseId"]),
                                    PackageName = dr1["PackageName"].ToString(),
                                    city = dr1["city"].ToString(),
                                    State = dr1["state"].ToString(),
                                    Price = dr1["Price"] != DBNull.Value ? Convert.ToDouble(dr1["Price"]) : 0.0,
                                    DiscountPercent = dr1["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(dr1["DiscountPercent"]) : 0.0,
                                    SalePrice = dr1["SalePrice"] != DBNull.Value ? Convert.ToDouble(dr1["SalePrice"]) : 0.0,
                                    MinQuantity = dr1["MinQuantity"] != DBNull.Value ? Convert.ToDouble(dr1["MinQuantity"]) : 0.0,
                                    StockQuantity = dr1["StockQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["StockQuantity"])) : 0,
                                    AddedQuantity = dr1["AddedQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(dr1["AddedQuantity"])) : 0,
                                };
                                _packages.Add(_productPackage);

                            }

                            franchisePackage _franchisePackage = new franchisePackage()
                            {
                                city = dr["city"].ToString(),
                                DeliveryBy = dr["DeliveryBy"].ToString(),
                                FranchiseId = Convert.ToInt32(dr["FranchiseId"]),
                                Price = Convert.ToDouble(dr["Price"]),
                                Packages = _packages,
                            };

                            productData.FranchisePackage.Add(_franchisePackage);
                        }
                    }
                }
            }
            if (productDataList.Count > 0)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = productDataList;
                return Ok(apiResult);
            }
            else
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = new List<Brand>();
                return NotFound(apiResult);
            }
        }

        [Route("api/GetCartFranchiseIdandProductid")]
        [HttpGet]
        public IActionResult GetCartFranchiseIdandProductid(int UserId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getcartfranchiseidandproductid", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_UserId", UserId);
                    List<CartProductAndFranchiseId> liSD = new List<CartProductAndFranchiseId>();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartProductAndFranchiseId _model = new CartProductAndFranchiseId
                            {
                                UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? 0 : reader.GetInt32("UserId"),
                                FranchiseIds = reader["FranchiseIds"].ToString(),
                                ProductIds = reader["ProductIds"].ToString(),
                            };
                            liSD.Add(_model);
                        }
                        if (liSD.Count > 0)
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = liSD;
                            return Ok(apiResult);
                        }
                        else
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new List<SunburstData>();
                            return Ok(apiResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }




        [Route("api/GetProductIdBasedonCart")]
        [HttpGet]
        public IActionResult GetProductIdBasedonCart(int UserId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getproductIdbasedoncart", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_UserId", UserId);
                    List<CartSProductId> liSD = new List<CartSProductId>();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartSProductId _model = new CartSProductId
                            {
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? 0 : reader.GetInt32("Category"),
                                UserId = reader.GetInt32(UserId),
                                FranchiseId = reader["FranchiseId"].ToString(),
                                ProductId = reader["ProductId"].ToString(),
                            };
                            liSD.Add(_model);
                        }
                        if (liSD.Count > 0)
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = liSD;
                            return Ok(apiResult);
                        }
                        else
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new List<CartSProductId>();
                            return Ok(apiResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        //[Route("api/GetProductsAndPackages1")]
        //[HttpPost]
        //public ActionResult<IEnumerable<ProductData>> GetProductsAndPackages1(int categoryId, int state)
        //{
        //    List<ProductData> productDataList = new List<ProductData>();

        //    using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        //    {
        //        connection.Open();

        //        using (MySqlCommand command = new MySqlCommand("usp_getproducts", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("_CategoryId", categoryId);

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    int productId = Convert.ToInt32(reader["ProductId"]);
        //                    string productTitle = Convert.ToString(reader["ProductTitle"]);

        //                    string Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description");
        //                    double MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble("MinQuantity");
        //                    double Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price");
        //                    double SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice");
        //                    string MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage");
        //                    string OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? null : reader.GetString("OtherImage1");
        //                    string OtherImage2 = reader.IsDBNull(reader.GetOrdinal("OtherImage2")) ? null : reader.GetString("OtherImage2");
        //                    string OtherImage3 = reader.IsDBNull(reader.GetOrdinal("OtherImage3")) ? null : reader.GetString("OtherImage3");
        //                    string Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : reader.GetString("Specification");
        //                    string DataSheet = reader.IsDBNull(reader.GetOrdinal("DataSheet")) ? null : reader.GetString("DataSheet");
        //                    string MetaTag = reader.IsDBNull(reader.GetOrdinal("MetaTag")) ? null : reader.GetString("MetaTag");
        //                    string MetaDescription = reader.IsDBNull(reader.GetOrdinal("MetaDescription")) ? null : reader.GetString("MetaDescription");
        //                    string QuickStartGuide = reader.IsDBNull(reader.GetOrdinal("QuickStartGuide")) ? null : reader.GetString("QuickStartGuide");
        //                    int BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32("BrandId");
        //                    string RelatedProducts = reader.IsDBNull(reader.GetOrdinal("RelatedProducts")) ? null : reader.GetString("RelatedProducts");
        //                    int CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId");
        //                    int DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder");
        //                    string ProductLabel = reader.IsDBNull(reader.GetOrdinal("ProductLabel")) ? null : reader.GetString("ProductLabel");
        //                    int StockQuantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32("StockQuantity");

        //                    var productData = new ProductData
        //                    {
        //                        ProductId = productId,
        //                        ProductTitle = productTitle,
        //                        Description = Description,
        //                        MinQuantity = MinQuantity,
        //                        Price = Price,
        //                        SalePrice = SalePrice,
        //                        MainImage = MainImage,
        //                        OtherImage1 = OtherImage1,
        //                        OtherImage2 = OtherImage2,
        //                        OtherImage3 = OtherImage3,
        //                        Specification = Specification,
        //                        DataSheet = DataSheet,
        //                        MetaTag = MetaTag,
        //                        MetaDescription = MetaDescription,
        //                        QuickStartGuide = QuickStartGuide,
        //                        BrandId = BrandId,
        //                        RelatedProducts = RelatedProducts,
        //                        CategoryId = CategoryId,
        //                        DisplayOrder = DisplayOrder,
        //                        ProductLabel = ProductLabel,
        //                        StockQuantity = StockQuantity,

        //                        franchisePackage = new List<franchisePackage>()
        //                    };

        //                    productDataList.Add(productData);
        //                }
        //            }
        //        }

        //        foreach (var productData in productDataList)
        //        {
        //            int productId = productData.ProductId;

        //            using (MySqlCommand packageCommand = new MySqlCommand("usp_getproductpackage", connection))
        //            {
        //                packageCommand.CommandType = CommandType.StoredProcedure;
        //                packageCommand.Parameters.AddWithValue("_ProductId", productId);
        //                packageCommand.Parameters.AddWithValue("_State", state);

        //                DataTable dt = new DataTable();
        //                MySqlDataAdapter da = new MySqlDataAdapter(packageCommand);
        //                da.Fill(dt);
        //                List<franchisePackage> franchiselist = new List<franchisePackage>();


        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    franchisePackage franchise = new franchisePackage();



        //                    franchiselist.
        //                }
        //            }
        //        }


        //    }
        //    if (productDataList.Count > 0)
        //    {
        //        ServiceRequestProcessor processor = new ServiceRequestProcessor();
        //        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
        //        apiResult.data = productDataList;
        //        return Ok(apiResult);
        //    }
        //    else
        //    {
        //        ServiceRequestProcessor processor = new ServiceRequestProcessor();
        //        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
        //        apiResult.data = new List<Brand>();
        //        return NotFound(apiResult);
        //    }
        //}




        [Route("api/GetAllFilterProducts")]
        [HttpPost]
        public IActionResult GetAllProducts(ProductFilter productfilter)
        {
            List<Product> products = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("usp_getallproducts", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_CategoryId", productfilter.CategoryId);
                command.Parameters.AddWithValue("_SubCategoryId", productfilter.SubCategoryId);
                command.Parameters.AddWithValue("_BrandId", productfilter.BrandId);
                command.Parameters.AddWithValue("_ProductId", productfilter.ProductId);
                command.Parameters.AddWithValue("_FranchiseId", productfilter.FranchiseId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                            ProductTitle = reader.IsDBNull(reader.GetOrdinal("ProductTitle")) ? null : reader.GetString("ProductTitle"),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                            MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble("MinQuantity"),
                            Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price"),
                            SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice"),
                            MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage"),
                            OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? null : reader.GetString("OtherImage1"),
                            OtherImage2 = reader.IsDBNull(reader.GetOrdinal("OtherImage2")) ? null : reader.GetString("OtherImage2"),
                            OtherImage3 = reader.IsDBNull(reader.GetOrdinal("OtherImage3")) ? null : reader.GetString("OtherImage3"),
                            Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : reader.GetString("Specification"),
                            DataSheet = reader.IsDBNull(reader.GetOrdinal("DataSheet")) ? null : reader.GetString("DataSheet"),
                            MetaTag = reader.IsDBNull(reader.GetOrdinal("MetaTag")) ? null : reader.GetString("MetaTag"),
                            MetaDescription = reader.IsDBNull(reader.GetOrdinal("MetaDescription")) ? null : reader.GetString("MetaDescription"),
                            QuickStartGuide = reader.IsDBNull(reader.GetOrdinal("QuickStartGuide")) ? null : reader.GetString("QuickStartGuide"),
                            BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32("BrandId"),
                            RelatedProducts = reader.IsDBNull(reader.GetOrdinal("RelatedProducts")) ? null : reader.GetString("RelatedProducts"),
                            CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId"),
                            DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                            ProductLabel = reader.IsDBNull(reader.GetOrdinal("ProductLabel")) ? null : reader.GetString("ProductLabel"),
                            StockQuantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32("StockQuantity"),
                            CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                            ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate"),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32("CreatedBy"),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? 0 : reader.GetInt32("ModifiedBy"),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),


                            SKU = reader.IsDBNull(reader.GetOrdinal("SKU")) ? null : reader.GetString("SKU"),
                            ProductType = reader.IsDBNull(reader.GetOrdinal("ProductType")) ? 0 : reader.GetInt32("ProductType"),
                            RewardPoint = reader.IsDBNull(reader.GetOrdinal("RewardPoint")) ? 0 : reader.GetInt32("RewardPoint"),
                            WarrantyInYear = reader.IsDBNull(reader.GetOrdinal("WarrantyInYear")) ? 0 : reader.GetInt32("WarrantyInYear"),
                            USPs = reader.IsDBNull(reader.GetOrdinal("USPs")) ? null : reader.GetString("USPs"),
                            ProductKeywords = reader.IsDBNull(reader.GetOrdinal("ProductKeywords")) ? null : reader.GetString("ProductKeywords"),
                            BrandName = reader["BrandName"].ToString(),
                            Category = reader["Category"].ToString(),
                            SubCategory = reader["SubCategory"].ToString(),

                        };

                        products.Add(product);
                    }
                }
            }

            return Ok(products);
        }


        [Route("api/GetProductsforAssign")]
        [HttpPost]
        public IActionResult GetProductsforAssign(ProductFilter productfilter)
        {
            List<Product> products = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("usp_getproductsforassign", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_CategoryId", productfilter.CategoryId);
                command.Parameters.AddWithValue("_SubCategoryId", productfilter.SubCategoryId);
                command.Parameters.AddWithValue("_ChildCategoryId", productfilter.ProductId);
                command.Parameters.AddWithValue("_BrandId", productfilter.BrandId);
                command.Parameters.AddWithValue("_FranchiseId", productfilter.FranchiseId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                            ProductTitle = reader.IsDBNull(reader.GetOrdinal("ProductTitle")) ? null : reader.GetString("ProductTitle"),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                            MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0 : reader.GetDouble("MinQuantity"),
                            Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price"),
                            SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice"),
                            MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? null : reader.GetString("MainImage"),
                            OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? null : reader.GetString("OtherImage1"),
                            OtherImage2 = reader.IsDBNull(reader.GetOrdinal("OtherImage2")) ? null : reader.GetString("OtherImage2"),
                            OtherImage3 = reader.IsDBNull(reader.GetOrdinal("OtherImage3")) ? null : reader.GetString("OtherImage3"),
                            Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : reader.GetString("Specification"),
                            DataSheet = reader.IsDBNull(reader.GetOrdinal("DataSheet")) ? null : reader.GetString("DataSheet"),
                            MetaTag = reader.IsDBNull(reader.GetOrdinal("MetaTag")) ? null : reader.GetString("MetaTag"),
                            MetaDescription = reader.IsDBNull(reader.GetOrdinal("MetaDescription")) ? null : reader.GetString("MetaDescription"),
                            QuickStartGuide = reader.IsDBNull(reader.GetOrdinal("QuickStartGuide")) ? null : reader.GetString("QuickStartGuide"),
                            BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32("BrandId"),
                            RelatedProducts = reader.IsDBNull(reader.GetOrdinal("RelatedProducts")) ? null : reader.GetString("RelatedProducts"),
                            CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32("CategoryId"),
                            DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                            ProductLabel = reader.IsDBNull(reader.GetOrdinal("ProductLabel")) ? null : reader.GetString("ProductLabel"),
                            StockQuantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32("StockQuantity"),
                            CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                            ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate"),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32("CreatedBy"),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? 0 : reader.GetInt32("ModifiedBy"),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),


                            SKU = reader.IsDBNull(reader.GetOrdinal("SKU")) ? null : reader.GetString("SKU"),
                            ProductType = reader.IsDBNull(reader.GetOrdinal("ProductType")) ? 0 : reader.GetInt32("ProductType"),
                            RewardPoint = reader.IsDBNull(reader.GetOrdinal("RewardPoint")) ? 0 : reader.GetInt32("RewardPoint"),
                            WarrantyInYear = reader.IsDBNull(reader.GetOrdinal("WarrantyInYear")) ? 0 : reader.GetInt32("WarrantyInYear"),
                            USPs = reader.IsDBNull(reader.GetOrdinal("USPs")) ? null : reader.GetString("USPs"),
                            ProductKeywords = reader.IsDBNull(reader.GetOrdinal("ProductKeywords")) ? null : reader.GetString("ProductKeywords"),
                            BrandName = reader["BrandName"].ToString(),
                            Category = reader["Category"].ToString(),
                            SubCategory = reader["SubCategory"].ToString(),

                        };

                        products.Add(product);
                    }
                }
            }

            return Ok(products);
        }
        //

        [Route("api/GetAdminSunburstData")]
        [HttpPost]
        public IActionResult GetAdminSunburstData([FromBody] ProductFilter productfilter)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getcategorysubburstdata", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_CategoryId", productfilter.CategoryId);
                    command.Parameters.AddWithValue("_SubCategoryId", productfilter.SubCategoryId);
                    command.Parameters.AddWithValue("_BrandId", productfilter.BrandId);
                    command.Parameters.AddWithValue("_ProductId", productfilter.ProductId);
                    command.Parameters.AddWithValue("_FranchiseId", productfilter.FranchiseId);

                    List<SunburstData> liSD = new List<SunburstData>();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SunburstData _model = new SunburstData
                            {
                                id = reader["id"].ToString(),
                                parent = reader["parent"].ToString(),
                                name = reader["name"].ToString(),
                                value = Convert.ToDouble(reader["Value"])
                            };
                            liSD.Add(_model);
                        }
                        if (liSD.Count > 0)
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = liSD;
                            return Ok(apiResult);
                        }
                        else
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new List<SunburstData>();
                            return Ok(apiResult);
                        }
                    }
                }
            }
            catch (Exception)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = new List<CategorySunburst>();
                return Ok(apiResult);
            }
        }

        [Route("api/GetUserInventory")]
        [HttpGet]
        public IActionResult GetUserInventory(int UserId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getuserinventory", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_UserId", UserId);
                    List<ProductInventory> liSD = new List<ProductInventory>();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductInventory _model = new ProductInventory
                            {
                                ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                MainImage = reader["MainImage"].ToString(),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                Quantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32("StockQuantity"),
                                NotExpiredQuantity = reader.IsDBNull(reader.GetOrdinal("NotExpiredQuantity")) ? 0 : reader.GetInt32("NotExpiredQuantity"),
                                BrandLogo = reader["Brandlogo"].ToString(),
                                BrandName = reader["BrandName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                            };
                            liSD.Add(_model);
                        }
                        if (liSD.Count > 0)
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = liSD;
                            return Ok(apiResult);
                        }
                        else
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new List<SunburstData>();
                            return Ok(apiResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetInventoryProductDetails")]
        [HttpGet]
        public IActionResult GetInventoryProductDetails(int UserId, int ProductId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getinventoryproductdetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_UserId", UserId);
                    command.Parameters.AddWithValue("_ProductId", ProductId);

                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(ds);

                    DataTable dtProductInventory = new DataTable();
                    dtProductInventory = ds.Tables[0];

                    if (dtProductInventory.Rows.Count > 0)
                    {
                        List<ProductInventoryDetails> liSD = new List<ProductInventoryDetails>();
                        ProductInventory _model = new ProductInventory
                        {
                            ProductId = Convert.ToInt32(dtProductInventory.Rows[0]["ProductId"].ToString()),
                            MainImage = dtProductInventory.Rows[0]["MainImage"].ToString(),
                            ProductTitle = dtProductInventory.Rows[0]["ProductTitle"].ToString(),
                            Quantity = Convert.ToInt32(dtProductInventory.Rows[0]["StockQuantity"].ToString())
                        };


                        DataTable dtPdv = new DataTable();
                        dtPdv = ds.Tables[1];
                        if (dtPdv.Rows.Count > 0)
                        {
                            foreach (DataRow dr1 in dtPdv.Rows)
                            {
                                ProductInventoryDetails _odel = new ProductInventoryDetails
                                {
                                    MacId = dr1.IsNull("MacId") ? null : (dr1["MacId"].ToString()),
                                    OrderId = dr1.IsNull("OrderId") ? 0 : Convert.ToInt32(dr1["OrderId"]),
                                    ProductId = dr1.IsNull("ProductId") ? 0 : Convert.ToInt32(dr1["ProductId"]),
                                    ProductTitle = dr1.IsNull("ProductTitle") ? null : (dr1["ProductTitle"].ToString()),
                                    MainImage = dr1.IsNull("MainImage") ? null : (dr1["MainImage"].ToString()),
                                    InvoiceNumber = dr1.IsNull("InvoiceNumber") ? null : (dr1["InvoiceNumber"].ToString()),
                                    InvoiceNumberUrl = dr1.IsNull("InvoiceNumberUrl") ? null : (dr1["InvoiceNumberUrl"].ToString()),
                                    WarrantyStatus = dr1.IsNull("WarrantyStatus") ? null : (dr1["WarrantyStatus"].ToString()),
                                    WarrantyExpiryDate = dr1["WarrantyExpiryDate"].ToString(),
                                    RMAStatus = dr1.IsNull("RMAStatus") ? 0 : Convert.ToInt32(dr1["RMAStatus"]),
                                    NewMacId = dr1.IsNull("NewMacId") ? null : (dr1["NewMacId"].ToString()),
                                    CnNumber = dr1.IsNull("CnNumber") ? null : (dr1["CnNumber"].ToString()),
                                    RMAFiled = dr1.IsNull("RMAFiled") ? 0 : Convert.ToInt32(dr1["RMAFiled"]),

                                };
                                liSD.Add(_odel);

                            }
                            ProductInventoryDetailsView ProductInventoryDetailsView = new ProductInventoryDetailsView
                            {
                                ProductInventory = _model,
                                ProductInventoryDetails = liSD
                            };
                            if (ProductInventoryDetailsView != null)
                            {
                                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = ProductInventoryDetailsView;
                                return Ok(apiResult);
                            }
                            else
                            {
                                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new List<ProductInventoryDetailsView>();
                                return Ok(apiResult);
                            }

                        }
                        else
                        {
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new List<ProductInventoryDetailsView>();
                            return Ok(apiResult);
                        }
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = new List<ProductInventoryDetailsView>();
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetBrandProductsData")]
        [HttpPost]
        public IActionResult GetBrandProductsData(ProductFilter productfilter)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getbrandproductsdata", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_CategoryId", productfilter.CategoryId);
                    command.Parameters.AddWithValue("_FranchiseId", productfilter.FranchiseId);

                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(ds);

                    DataTable dtProductCategory = new DataTable();
                    dtProductCategory = ds.Tables[0];

                    if (dtProductCategory.Rows.Count > 0)
                    {
                        DataTable dtBrandProduct = new DataTable();
                        dtBrandProduct = ds.Tables[1];
                        if (dtBrandProduct.Rows.Count > 0)
                        {
                            List<BrandProducts> _liModel = new List<BrandProducts>();

                            foreach (DataRow dr1 in dtBrandProduct.Rows)
                            {
                                BrandProducts _odel = new BrandProducts
                                {
                                    BrandLogo = dr1.IsNull("BrandLogo") ? null : (dr1["BrandLogo"].ToString()),
                                    BrandName = dr1.IsNull("BrandName") ? null : (dr1["BrandName"].ToString()),
                                    TotalQuantity = dr1.IsNull("StockQuantity") ? 0 : Convert.ToDouble(dr1["StockQuantity"]),
                                    TotalValue = dr1.IsNull("TotalAmount") ? 0 : Convert.ToDouble(dr1["TotalAmount"])
                                };
                                _liModel.Add(_odel);
                            }
                            CategoryProducts _model = new CategoryProducts
                            {
                                TotalValue = Convert.ToDouble(dtProductCategory.Rows[0]["TotalAmount"].ToString()),
                                CategoryName = dtProductCategory.Rows[0]["CategoryName"].ToString(),
                                TotalQuantity = Convert.ToDouble(dtProductCategory.Rows[0]["StockQuantity"].ToString()),
                                ValueBrand = _liModel
                            };

                            if (_model != null)
                            {
                                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = _model;
                                return Ok(apiResult);
                            }
                            else
                            {
                                //Commented below 5 lines to avoid 2 tables (repeatation of records in products table)
                                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new List<ProductInventoryDetailsView>();
                                return Ok(apiResult);
                            }

                        }
                        else
                        {
                            //Commented below 5 lines to avoid 2 tables (repeatation of records in products table)
                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new List<ProductInventoryDetailsView>();
                            return Ok(apiResult);
                        }
                    }
                    else
                    {
                        //Commented below 5 lines to avoid 2 tables (repeatation of records in products table)
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = new List<ProductInventoryDetailsView>();
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                //Commented below 5 lines to avoid 2 tables (repeatation of records in products table)
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = new List<ProductInventoryDetailsView>();
                return Ok(apiResult);
                //above code is newly added, need to remove. below code is the original code
                //ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                //return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }



        [Route("api/AddProductRef")]
        [HttpPost]
        public IActionResult AddProductRef(int ProductId, int ProductReferenceId)
        {
            try
            {
                int result = 0;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_addproductreference", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_ProductId", ProductId);
                    command.Parameters.AddWithValue("_ProductReferenceId", ProductReferenceId);
                    //command.Parameters.AddWithValue("_ModifiedBy", modifiedBy);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters["_Result"].Value);
                }

                if (result == 1)
                {

                    // UpdateRma updateRma = new UpdateRma();

                    //if (rmaStatus == 2)// Order Confirmed
                    //{
                    //    List<UpdateRma> updateRmas = new List<UpdateRma>();
                    //    // updateRmas = Od.GetOrderDetailsForsms(rmaId);
                    //    string updateRm = string.Empty;

                    //    foreach (UpdateRma item in updateRmas)
                    //    {
                    //        if (updateRm == string.Empty)
                    //        {
                    //            updateRm = item.RMAStatus.ToString();
                    //        }

                    //    }

                    //}
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = result;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = 0;
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetReferenceProduct")]
        [HttpPost]
        public IActionResult GetReferenceProduct(int Id)
        {
            try
            {
                int result = 0;
                List<ProductReference> _model = new List<ProductReference>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getreferenceproduct", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_ProductId", Id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductReference _odel = new ProductReference
                            {
                                ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                ProductReferenceId = reader.IsDBNull(reader.GetOrdinal("ProductReferenceId")) ? 0 : reader.GetInt32("ProductReferenceId"),
                            };
                            _model.Add(_odel);
                        }
                    }
                    if (_model.Count > 0)
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = _model;
                        return Ok(apiResult);
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = _model;
                        return NotFound(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetBestseller")]
        [HttpGet]
        public IActionResult GetBestseller()
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    string SpType = "R";
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getbestseller", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Product> _bestsellerview = new List<Product>();

                            while (reader.Read())
                            {
                                var _bestseller = new Product
                                {
                                    ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                    ProductTitle = reader.IsDBNull(reader.GetOrdinal("ProductTitle")) ? "" : reader.GetString("ProductTitle"),
                                    Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price"),
                                    DiscountPercent = reader.IsDBNull(reader.GetOrdinal("DiscountPercent")) ? 0 : reader.GetDouble("DiscountPercent"),
                                    SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice"),
                                    MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? "" : reader.GetString("MainImage"),
                                    OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? "" : reader.GetString("OtherImage1"),

                                };

                                _bestsellerview.Add(_bestseller);
                            }

                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = _bestsellerview;
                            return Ok(apiResult);

                            //return Ok(registrationList);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new { };
                            return NotFound(apiResult);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/GetFestiveCollaction")]
        [HttpGet]
        public IActionResult GetFestiveCollaction()
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    string SpType = "R";
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getfestivecollaction", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Product> _bestsellerview = new List<Product>();

                            while (reader.Read())
                            {
                                var _bestseller = new Product
                                {
                                    ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                    ProductTitle = reader.IsDBNull(reader.GetOrdinal("ProductTitle")) ? "" : reader.GetString("ProductTitle"),
                                    Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDouble("Price"),
                                    DiscountPercent = reader.IsDBNull(reader.GetOrdinal("DiscountPercent")) ? 0 : reader.GetDouble("DiscountPercent"),
                                    SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0 : reader.GetDouble("SalePrice"),
                                    MainImage = reader.IsDBNull(reader.GetOrdinal("MainImage")) ? "" : reader.GetString("MainImage"),
                                    OtherImage1 = reader.IsDBNull(reader.GetOrdinal("OtherImage1")) ? "" : reader.GetString("OtherImage1"),

                                };

                                _bestsellerview.Add(_bestseller);
                            }

                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = _bestsellerview;
                            return Ok(apiResult);

                            //return Ok(registrationList);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new { };
                            return NotFound(apiResult);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/GetFilterProducts")]
        [HttpGet]
        public IActionResult GetFilterProducts(int categoryId, string colors, string sizes, double priceMin, double priceMax, string productTitle)
        {
            try
            {
                ProductFilter productFilter = new ProductFilter();
                List<Product> productList = new List<Product>();
                List<Product> licolors = new List<Product>();
                List<Sizes> lisizes = new List<Sizes>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getfilterproducts", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_CategoryId", categoryId);
                    command.Parameters.AddWithValue("_Colors", colors);
                    command.Parameters.AddWithValue("_Sizes", sizes);
                    command.Parameters.AddWithValue("_PriceMin", priceMin);
                    command.Parameters.AddWithValue("_PriceMax", priceMax);
                    command.Parameters.AddWithValue("_ProductTitle", productTitle);

                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(ds);
                    DataTable dtProduct;
                    DataTable dtColor;
                    DataTable dtSize;
                    if (ds != null && ds.Tables.Count == 3)
                    {
                        dtProduct = ds.Tables[0];
                        dtColor = ds.Tables[2];
                        dtSize = ds.Tables[1];
                    }
                    else
                    {
                        dtProduct = null;
                        dtColor = null;
                        dtSize = null;
                    }
                    foreach (DataRow reader in dtColor.Rows)
                    {
                        Product color = new Product
                        {
                            Color = reader["Color"].ToString(),
                            ProductId = DAL.Utilities.validateInt(reader["Id"])
                        };
                        licolors.Add(color);
                    }

                    foreach (DataRow reader in dtSize.Rows)
                    {
                        Sizes size = new Sizes
                        {
                            Size = reader["Size"].ToString(),
                            Id = DAL.Utilities.validateInt(reader["Id"])
                        };
                        lisizes.Add(size);
                    }

                    foreach (DataRow reader in dtProduct.Rows)
                    {
                        Product product = new Product
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductTitle = reader["ProductTitle"].ToString(),
                            Description = reader["Description"].ToString(),
                            MinQuantity = reader["MinQuantity"] != DBNull.Value ? Convert.ToDouble(reader["MinQuantity"]) : 0.00,
                            Price = reader["Price"] != DBNull.Value ? Convert.ToDouble(reader["Price"]) : 0.0,
                            DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(reader["DiscountPercent"]) : 0.0,
                            SalePrice = reader["SalePrice"] != DBNull.Value ? Convert.ToDouble(reader["SalePrice"]) : 0.0,
                            MainImage = reader["MainImage"].ToString(),
                            OtherImage1 = reader["OtherImage1"].ToString(),
                            OtherImage2 = reader["OtherImage2"].ToString(),
                            OtherImage3 = reader["OtherImage3"].ToString(),
                            Specification = reader["Specification"].ToString(),
                            DataSheet = reader["DataSheet"].ToString(),
                            MetaTag = reader["MetaTag"].ToString(),
                            MetaDescription = reader["MetaDescription"].ToString(),
                            QuickStartGuide = reader["QuickStartGuide"].ToString(),
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            BrandName = reader["BrandName"].ToString(),
                            RelatedProducts = reader["RelatedProducts"].ToString(),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(reader["DisplayOrder"]) : 0,
                            ProductLabel = reader["ProductLabel"].ToString(),
                            StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                            CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                            ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : DateTime.MinValue,
                            CreatedBy = reader["CreatedBy"] != DBNull.Value ? Convert.ToInt32(reader["CreatedBy"]) : 0,
                            ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(reader["ModifiedBy"]) : 0,
                            IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"])
                        };

                        productList.Add(product);
                    }
                }
                filterProduct fp = new filterProduct
                {
                    products = productList,
                    colors = licolors,
                    sizes = lisizes,
                };
                if (productList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = fp;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Brand>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/DeleteProduct")]
        [HttpPost]
        public IActionResult DeleteProducts(Product product)
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
                            //string[] categoryIds = product.ChildCategory.Split(',');
                            //foreach (string id in categoryIds)
                            //{
                            //    MySqlCommand childCommand = new MySqlCommand("usp_categorymapping", connection);
                            //    childCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            //    {
                            //        childCommand.Parameters.AddWithValue("_ProductId", result);
                            //        childCommand.Parameters.AddWithValue("_CategoryId", Convert.ToInt32(id));
                            //    };
                            //    childCommand.ExecuteNonQuery();
                            //}
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
                                        RelatedProducts = reader.IsDBNull(reader.GetOrdinal("RelatedProducts")) ? null : reader.GetString(reader.GetOrdinal("RelatedProducts")),
                                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32(reader.GetOrdinal("DisplayOrder")),
                                        ProductLabel = reader.IsDBNull(reader.GetOrdinal("ProductLabel")) ? null : reader.GetString(reader.GetOrdinal("ProductLabel")),
                                        StockQuantity = reader.IsDBNull(reader.GetOrdinal("StockQuantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("ModifiedBy")),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        WarrantyInYear = reader.IsDBNull(reader.GetOrdinal("WarrantyInYear")) ? 0 : reader.GetInt32(reader.GetOrdinal("WarrantyInYear")),
                                        ProductKeywords = reader.IsDBNull(reader.GetOrdinal("ProductKeywords")) ? null : reader.GetString(reader.GetOrdinal("ProductKeywords")),
                                        SKU = reader.IsDBNull(reader.GetOrdinal("SKU")) ? null : reader.GetString(reader.GetOrdinal("SKU")),
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

                                    };


                                    productList.Add(productModel);
                                }
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = productList;
                                return Ok(apiResult);
                            }
                            else
                            {
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { };
                                return NotFound(apiResult);
                            }
                        }
                    }
                    else
                    {
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

