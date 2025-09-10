using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;


namespace SynfoShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPackageController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductPackageController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        public IActionResult GetProductPackage(ProductPackage productpackage)
        {
            try
            {
                List<ProductPackage> productPackages = new List<ProductPackage>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getproductpackage", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_ProductId", productpackage.ProductId);
                    command.Parameters.AddWithValue("_FranchiseId", productpackage.FranchiseId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductPackage productPackage = new ProductPackage
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                FranchiseId = Convert.ToInt32(reader["FranchiseId"]),
                                PackageName = reader["PackageName"].ToString(),
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDouble(reader["Price"]) : 0.0,
                                DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(reader["DiscountPercent"]) : 0.0,
                                SalePrice = reader["SalePrice"] != DBNull.Value ? Convert.ToDouble(reader["SalePrice"]) : 0.0,
                                MinQuantity = reader["MinQuantity"] != DBNull.Value ? Convert.ToDouble(reader["MinQuantity"]) : 0.0,
                                DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(reader["DisplayOrder"])) : 0,
                                StockQuantity = reader["StockQuantity"] != DBNull.Value ? Convert.ToInt32(Convert.ToDouble(reader["StockQuantity"])) : 0,
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : DateTime.MinValue,
                                IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"])
                            };

                            productPackages.Add(productPackage);
                        }
                    }
                }
                if (productPackages.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = productPackages;
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
    }
}