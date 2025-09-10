using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace SynfoShopAPI.Controllers
{

    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IConfiguration _config;
        public StockController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            //  GSTClientId = _config["AppSeettings:GSTClientId"];
            //  GSTClientSecret = _config["AppSeettings:GSTclient_secret"];
        }

        [Route("api/AddFranchiseStock")]
        [HttpPost]
        public IActionResult AddFranchiseStock(ClosingStock1 closingstock)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                if (closingstock.ClosingStock != null)
                {
                    if (closingstock.ClosingStock.Count > 0)
                    {
                        int result;

                        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                        {
                            connection.Open();
                            int i = 0;
                            foreach (StockItem item in closingstock.ClosingStock)
                            {
                                i++;
                                MySqlCommand command = new MySqlCommand("usp_addfranchisestock", connection);
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("_CompanyName", item.CompanyName);
                                command.Parameters.AddWithValue("_CompanyGuid", item.CompanyGuid);
                                command.Parameters.AddWithValue("_ProductName", item.ProductName);
                                command.Parameters.AddWithValue("_Godown", item.Godown);
                                command.Parameters.AddWithValue("_BaseUnit", item.BaseUnit);
                                command.Parameters.AddWithValue("_BaseQTY", item.BaseQTY);
                                command.ExecuteNonQuery();

                            }
                            if (i > 0)
                            {
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.message = i + " rows submitted successfully.";
                                return Ok(apiResult);

                            }
                            else
                            {
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                                apiResult.data = null;
                                return Ok(apiResult);

                            }
                        }
                    }
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = null;
                        return Ok(apiResult);
                    }
                   
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                    apiResult.data = null;
                    return Ok(apiResult);
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
