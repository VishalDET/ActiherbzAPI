using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System.Collections.Generic;
using System;

namespace SynfoShopAPI.Controllers
{
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PaymentController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            _hostEnvironment = hostEnvironment;
        }

        [Route("api/SavePayDetails")]
        [HttpPost]
        public IActionResult SavePayDetails(PaymentResponse payment)
        {
            try
            {
                int result = 0;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                if (payment != null && payment.OrderId > 0)
                {


                    using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("usp_addpaymentdetails", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("_OrderId", payment.OrderId);
                        command.Parameters.AddWithValue("_TransactionId", payment.TransactionId);
                        command.Parameters.AddWithValue("_PayAmount", payment.PayAmount);
                        command.Parameters.AddWithValue("_Status", payment.Status);

                        MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                        resultParam.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(resultParam);

                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);

                        if (result != 0)
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = new { Status = result };
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                            apiResult.data = new { Id = result };
                            return Ok(apiResult);
                        }
                    }
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = new { Id = result };
                    return Ok(apiResult);
                }

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetPaymentDetails")]
        [HttpGet]
        public IActionResult GetPaymentDetails(int OrderId)
        {
            try
            {
                List<PaymentData> AdminList = new List<PaymentData>();
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                if (OrderId > 0)
                {
                    using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("usp_getpaymentdetails", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("_OrderId", OrderId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    var paymentData = new PaymentData
                                    {
                                        //Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id")
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        OrderId = reader.IsDBNull(reader.GetOrdinal("OrderId")) ? 0 : reader.GetInt32("OrderId"),
                                        PayAmount = reader.IsDBNull(reader.GetOrdinal("PayAmount")) ? 0 : reader.GetDouble("PayAmount"),
                                        TransactionId = reader.IsDBNull(reader.GetOrdinal("TransactionId")) ? null : reader.GetString("TransactionId"),
                                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString("Status")
                                    };
                                    AdminList.Add(paymentData);
                                }
                            }
                        }
                    }
                    if (AdminList.Count != 0)
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = AdminList;
                        return Ok(apiResult);
                    }
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        return Ok(apiResult);
                    }
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
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
