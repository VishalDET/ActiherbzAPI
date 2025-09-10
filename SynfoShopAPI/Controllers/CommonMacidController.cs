using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
//using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SynfoShopAPI.Controllers
{
  
    [ApiController]
    public class CommonMacidController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CommonMacidController(IConfiguration config)
        {
            _config = config;
        }

        [Route("api/commonMacid")]
        [HttpPost]
        public IActionResult CommonMacid(Macid macid)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonmacid", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", macid.Id);
                    command.Parameters.AddWithValue("_ProductId", macid.ProductId);
                    command.Parameters.AddWithValue("_PackageId", macid.PackageId);
                    command.Parameters.AddWithValue("_FranchiseId", macid.FranchiseId);
                    command.Parameters.AddWithValue("_MacId", macid.MacId);
                    command.Parameters.AddWithValue("_OrderId", macid.OrderId);
                    command.Parameters.AddWithValue("_OrderDetailsId", macid.OrderDetailsId);
                    command.Parameters.AddWithValue("_SpType", macid.SpType);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (macid.SpType == "C" || macid.SpType == "U" || macid.SpType == "D")
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
                    else if (macid.SpType == "E")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Macid> macList = new List<Macid>();

                                while (reader.Read())
                                {
                                    var MacidModel = new Macid
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                        PackageId = reader.IsDBNull(reader.GetOrdinal("PackageId")) ? 0 : reader.GetInt32("PackageId"),
                                        FranchiseId = reader.IsDBNull(reader.GetOrdinal("FranchiseId")) ? 0 : reader.GetInt32("FranchiseId"),
                                        MacId = reader.IsDBNull(reader.GetOrdinal("MacId")) ? null : reader.GetString("MacId"),
                                        OrderId = reader.IsDBNull(reader.GetOrdinal("OrderId")) ? 0 : reader.GetInt32("OrderId"),
                                        OrderDetailsId = reader.IsDBNull(reader.GetOrdinal("OrderDetailsId")) ? 0 : reader.GetInt32("OrderDetailsId"),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    };

                                    macList.Add(MacidModel);
                                }
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = macList;
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
                    else if (macid.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Macid> macidList = new List<Macid>();

                                while (reader.Read())
                                {
                                    var macidModel = new Macid
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? 0 : reader.GetInt32("ProductId"),
                                        PackageId = reader.IsDBNull(reader.GetOrdinal("PackageId")) ? 0 : reader.GetInt32("PackageId"),
                                        FranchiseId = reader.IsDBNull(reader.GetOrdinal("FranchiseId")) ? 0 : reader.GetInt32("FranchiseId"),
                                        MacId = reader.IsDBNull(reader.GetOrdinal("MacId")) ? null : reader.GetString("MacId"),
                                        OrderId = reader.IsDBNull(reader.GetOrdinal("OrderId")) ? 0 : reader.GetInt32("OrderId"),
                                        OrderDetailsId = reader.IsDBNull(reader.GetOrdinal("OrderDetailsId")) ? 0 : reader.GetInt32("OrderDetailsId"),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    };

                                    macidList.Add(macidModel);
                                }                                
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = macidList;
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
