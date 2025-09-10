using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SynfoShopAPI.DAL;
using SynfoShopAPI.Models;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using MailKit.Search;
using MySqlX.XDevAPI.Common;
using System.Data;

namespace SynfoShopAPI.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    public class MobileNotificationController : ControllerBase
    {
        private readonly IConfiguration _config;
        public MobileNotificationController(IConfiguration config)
        {
            _config = config;
        }


        [Route("api/SendNoti")]
        [HttpPost]
        public async Task<IActionResult> SendNoti(NotificationModel notificationModel)
          {
            try
            {
                int result = 0;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_commonfcmtoken", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    //command.Parameters.AddWithValue("_UserId", userId);
                    command.Parameters.AddWithValue("_UserId",notificationModel.UserId);
                    command.Parameters.AddWithValue("_TokenValue", notificationModel.TokenValue);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters["_Result"].Value);
                }

                PushNotificationM ut = new PushNotificationM(_config);
                await ut.SendNotification(notificationModel);


                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Sent";
                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }

         [Route("api/SendNoti")]
        [HttpGet]
        public  async Task<IActionResult> GetDetails(int UserId, string DeviceId, string Message)
        {
            try
            {
                int result = 0;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_commonfcmtoken", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    //command.Parameters.AddWithValue("_UserId", userId);
                    command.Parameters.AddWithValue("_UserId", UserId);
                    command.Parameters.AddWithValue("_TokenValue", DeviceId);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters["_Result"].Value);
                }

                

                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Sent";
                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }
    }
}
