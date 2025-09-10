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
    public class CommonProductMappingController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CommonProductMappingController(IConfiguration config)
        {
            _config = config;
        }
        [Route("api/CommonProductMapping")]
        [HttpPost]
        public IActionResult CommonProductMapping(CommonProductMapping productMapping)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonproductmapping", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", productMapping.Id);
                    command.Parameters.AddWithValue("_FranchiseId", productMapping.FranchiseId);
                    command.Parameters.AddWithValue("_ProductId", productMapping.ProductId);
                    command.Parameters.AddWithValue("_IsActive", productMapping.IsActive);
                    command.Parameters.AddWithValue("_SpType", productMapping.SpType);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (productMapping.SpType == "C" || productMapping.SpType == "U" || productMapping.SpType == "D")
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
                    else if (productMapping.SpType == "E" || productMapping.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<CommonProductMapping> productMappingList = new List<CommonProductMapping>();

                                while (reader.Read())
                                {
                                    var productMappingModel = new CommonProductMapping
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                        FranchiseId = reader.IsDBNull(reader.GetOrdinal("FranchiseId")) ? 0 : reader.GetInt32("FranchiseId"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                                    };

                                    productMappingList.Add(productMappingModel);
                                }

                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = productMappingList;
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
