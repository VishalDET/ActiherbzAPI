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
    public class CommonPackageController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CommonPackageController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult CommonPackage(CommonPackageAdd commonpackage)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonpackage", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", commonpackage.Id);
                    command.Parameters.AddWithValue("_ProductId", commonpackage.ProductId);
                    command.Parameters.AddWithValue("_FranchiseId", commonpackage.FranchiseId);
                    command.Parameters.AddWithValue("_PackageName", commonpackage.PackageName);
                    command.Parameters.AddWithValue("_Color", commonpackage.Color);
                    command.Parameters.AddWithValue("_Size", commonpackage.Size);
                    command.Parameters.AddWithValue("_MinQuantity", commonpackage.MinQuantity);
                    command.Parameters.AddWithValue("_Price", commonpackage.Price);
                    command.Parameters.AddWithValue("_SalePrice", commonpackage.SalePrice);
                    command.Parameters.AddWithValue("_DisplayOrder", commonpackage.DisplayOrder);
                    command.Parameters.AddWithValue("_IsActive", commonpackage.IsActive);
                    command.Parameters.AddWithValue("_SpType", commonpackage.SpType);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (commonpackage.SpType == "C" || commonpackage.SpType == "U" || commonpackage.SpType == "D")
                    {
                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);
                        if (result != 0)
                        {
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
                    else if (commonpackage.SpType == "E" || commonpackage.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<CommonPackage> packageList = new List<CommonPackage>();

                                while (reader.Read())
                                {
                                    var packageModel = new CommonPackage
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                        FranchiseId = reader.IsDBNull(reader.GetOrdinal("FranchiseId")) ? 0 : reader.GetInt32("FranchiseId"),
                                        Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? 0 : reader.GetInt32("Color"),
                                        Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? 0 : reader.GetInt32("Size"),
                                        PackageName = reader.IsDBNull(reader.GetOrdinal("PackageName")) ? null : reader.GetString("PackageName"),
                                        Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0.0 : reader.GetDouble("Price"),
                                        DiscountPercent = reader.IsDBNull(reader.GetOrdinal("DiscountPercent")) ? 0.0 : reader.GetDouble("DiscountPercent"),
                                        MinQuantity = reader.IsDBNull(reader.GetOrdinal("MinQuantity")) ? 0.0 : reader.GetDouble("MinQuantity"),
                                        SalePrice = reader.IsDBNull(reader.GetOrdinal("SalePrice")) ? 0.0 : reader.GetDouble("SalePrice"),
                                        DisplayOrder = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? 0 : reader.GetInt32("DisplayOrder"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                                    };

                                    packageList.Add(packageModel);
                                }

                                //return Ok(packageList);
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = packageList;
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

