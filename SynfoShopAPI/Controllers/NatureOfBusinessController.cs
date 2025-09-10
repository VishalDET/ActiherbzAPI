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
    public class NatureOfBusinessController : ControllerBase
    {
        private readonly IConfiguration _config;

        public NatureOfBusinessController(IConfiguration config)
        {
            _config = config;
        }

        [Route("api/CommonnatureOfBusiness")]
        [HttpPost]
        public IActionResult GetNatureOfBusiness(NatureOfBusiness natureofbusiness)
        {
            try
            {
                int result = 0;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                List<NatureOfBusiness> natureOfBusinesses = new List<NatureOfBusiness>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    using (MySqlCommand command = new MySqlCommand("usp_commonnatureofbusiness", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@_Id", natureofbusiness.Id);
                        command.Parameters.AddWithValue("@_Name", natureofbusiness.Name);
                        command.Parameters.AddWithValue("@_MobileNo", natureofbusiness.MobileNo);
                        command.Parameters.AddWithValue("@_IsActive", natureofbusiness.IsActive);
                        command.Parameters.AddWithValue("@_SpType", natureofbusiness.SpType);
                        command.Parameters.Add("@_Result", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    NatureOfBusiness natureOfBusiness = new NatureOfBusiness
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString("Name"),
                                        MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString("MobileNo"),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean("IsActive"),
                                        //IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate")
                                    };

                                    natureOfBusinesses.Add(natureOfBusiness);
                                }
                            }
                        }

                        if (!int.TryParse(command.Parameters["@_Result"].Value.ToString(), out result))
                        {
                            result = 0;
                        }
                    }
                }

                if (natureofbusiness.SpType == "R")
                {
                    //return Ok(new { Result = natureOfBusinesses });
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = natureOfBusinesses;
                    return Ok(apiResult);
                }
                else
                {
                    //return NotFound(new { Result = "Invalid spType" });
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, "Invalid SpType");
                    apiResult.data = new { };
                    return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }

        }

        [Route("api/NatureOfBusiness")]
        [HttpPost]
        public IActionResult GetNatureofBusiness()
        {
            try
            {
                List<NatureOfBusiness> natureOfBusinessList = new List<NatureOfBusiness>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("usp_getnatureofbusiness", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var natureOfBusiness = new NatureOfBusiness
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString("Name"),
                            IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean("IsActive"),
                            MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? string.Empty : reader.GetString("MobileNo"),
                            CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate")
                        };

                        natureOfBusinessList.Add(natureOfBusiness);
                    }
                }
            }

                if (natureOfBusinessList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = natureOfBusinessList;
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
