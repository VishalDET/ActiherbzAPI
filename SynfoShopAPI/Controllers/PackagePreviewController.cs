using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SynfoShopAPI.Controllers
{
   
    [ApiController]
    public class PackagePreviewController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PackagePreviewController(IConfiguration config)
        {
            _config = config;
        }
        [Route("api/GetPackagePreview")]
        [HttpGet]
        public IActionResult GetPackagePreview(int productId, int franchiseId)
        {
            try
            {
                List<PackagePreview> packagePreviews = new List<PackagePreview>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getpackagepreview", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_ProductId", productId);
                    command.Parameters.AddWithValue("_FranchiseId", franchiseId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PackagePreview packagePreview = new PackagePreview
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                FranchiseId = Convert.ToInt32(reader["FranchiseId"]),
                                PackageName = reader["PackageName"].ToString(),
                                DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]),
                                SalePrice = Convert.ToDecimal(reader["SalePrice"]),
                                MinQuantity = Convert.ToInt32(reader["MinQuantity"]),
                                LogoUrl = reader["LogoUrl"].ToString(),
                                BrandName = reader["BrandName"].ToString(),
                                MainImage = reader["MainImage"].ToString()
                            };

                            packagePreviews.Add(packagePreview);
                        }
                    }
                }

                if (packagePreviews.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = packagePreviews;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<PackagePreview>();
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
