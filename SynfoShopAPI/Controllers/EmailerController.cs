using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System.Collections.Generic;
using System.Data;
using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.Metrics;
using MySqlX.XDevAPI.Common;
using System.Diagnostics;
using MimeKit;
using Twilio.TwiML.Messaging;
using Microsoft.IdentityModel.Tokens;
using SynfoShopAPI.DAL;
using MailKit.Search;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Org.BouncyCastle.Ocsp;
using iTextSharp.text.html;
using iText.Html2pdf;
using iTextSharp.tool.xml;
//using iText.Kernel.Exceptions.PdfException;

namespace SynfoShopAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class EmailerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnvironment;
        private IConfiguration config;

        public EmailerController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            _hostEnvironment = hostEnvironment;
        }



        [Route("api/SendEmail")]
        [HttpGet]
        public IActionResult SendOTP_Email(int UserId, int OTP)
        {
            Registration registration = new Registration();
            SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getuserwithsalesuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Id", UserId);

                    SalesUserEmail registrationModel = new SalesUserEmail();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                registrationModel = new SalesUserEmail()
                                {
                                    Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                    Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                    EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId"),
                                    SalesEmailId = reader.IsDBNull(reader.GetOrdinal("SalesEmailId")) ? null : reader.GetString("SalesEmailId")
                                };
                            }
                        }
                    }

                    if (registrationModel != null)
                    {
                        var Emailers = "Emailer";
                        var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                        string filePath = Path.Combine(path, "otp.html");
                        //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                        StreamReader streader = new StreamReader(filePath);
                        string readFile = streader.ReadToEnd();
                        string StrContent = "";
                        StrContent = readFile;
                        StrContent = StrContent.Replace("[UserFullName]", registrationModel.Fullname);
                        StrContent = StrContent.Replace("[OTP]", OTP.ToString());

                        ut.SendMailInfo("LOGIN OTP", StrContent, registrationModel.EmailId);
                        if (registrationModel.SalesEmailId != string.Empty)
                        {
                            ut.SendMailInfo("LOGIN OTP", StrContent, registrationModel.SalesEmailId);
                        }

                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Sent";
                        return Ok(apiResult);
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }

        [Route("api/SendRegister")]
        [HttpGet]
        public IActionResult SendRegister_Email(int UserId)
        {
            Registration registration = new Registration();
            SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            string SpType = "E";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", UserId);
                    command.Parameters.AddWithValue("_Fullname", registration.Fullname);
                    command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
                    command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
                    command.Parameters.AddWithValue("_EmailId", registration.EmailId);
                    command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
                    command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
                    command.Parameters.AddWithValue("_StateId", registration.StateId);
                    command.Parameters.AddWithValue("_CityId", registration.CityId);
                    command.Parameters.AddWithValue("_SpType", SpType);
                    command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
                    command.Parameters.AddWithValue("_IsActive", registration.IsActive);
                    command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    Registration registrationModel = new Registration();
                    if (SpType == "E")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    registrationModel = new Registration()
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId")

                                    };
                                }
                            }
                        }
                    }
                    if (registrationModel != null)
                    {
                        var Emailers = "Emailer";
                        var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                        string filePath = Path.Combine(path, "signup.html");
                        //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                        StreamReader streader = new StreamReader(filePath);
                        string readFile = streader.ReadToEnd();
                        string StrContent = "";
                        StrContent = readFile;
                        StrContent = StrContent.Replace("[UserFullName]", registrationModel.Fullname);
                        //StrContent = StrContent.Replace("[OTP]", OTP.ToString());

                        ut.SendMailInfo("Register", StrContent, registrationModel.EmailId);

                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Sent";
                        return Ok(apiResult);
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }

        [Route("api/SendWelcome")]
        [HttpGet]
        public IActionResult SendWelcome_Email(int UserId)
        {
            Registration registration = new Registration();
            SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            string SpType = "E";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", UserId);
                    command.Parameters.AddWithValue("_Fullname", registration.Fullname);
                    command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
                    command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
                    command.Parameters.AddWithValue("_EmailId", registration.EmailId);
                    command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
                    command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
                    command.Parameters.AddWithValue("_StateId", registration.StateId);
                    command.Parameters.AddWithValue("_CityId", registration.CityId);
                    command.Parameters.AddWithValue("_SpType", SpType);
                    command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
                    command.Parameters.AddWithValue("_IsActive", registration.IsActive);
                    command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    Registration registrationModel = new Registration();
                    if (SpType == "E")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    registrationModel = new Registration()
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId")

                                    };
                                }
                            }
                        }
                    }
                    if (registrationModel != null)
                    {
                        var Emailers = "Emailer";
                        var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                        string filePath = Path.Combine(path, "welcome.html");
                        //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                        StreamReader streader = new StreamReader(filePath);
                        string readFile = streader.ReadToEnd();
                        string StrContent = "";
                        StrContent = readFile;
                        StrContent = StrContent.Replace("[UserFullName]", registrationModel.Fullname);
                        StrContent = StrContent.Replace("[UserID]", registrationModel.Id.ToString());

                        ut.SendMailInfo("Welcome", StrContent, registrationModel.EmailId);

                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Sent";
                        return Ok(apiResult);
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }

        [Route("api/SendThankyou")]
        [HttpGet]
        public IActionResult SendThankyou_Email(int UserId)
        {
            Registration registration = new Registration();
            SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            string SpType = "E";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", UserId);
                    command.Parameters.AddWithValue("_Fullname", registration.Fullname);
                    command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
                    command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
                    command.Parameters.AddWithValue("_EmailId", registration.EmailId);
                    command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
                    command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
                    command.Parameters.AddWithValue("_StateId", registration.StateId);
                    command.Parameters.AddWithValue("_CityId", registration.CityId);
                    command.Parameters.AddWithValue("_SpType", SpType);
                    command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
                    command.Parameters.AddWithValue("_IsActive", registration.IsActive);
                    command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    Registration registrationModel = new Registration();
                    if (SpType == "E")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    registrationModel = new Registration()
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId")

                                    };
                                }
                            }
                        }
                    }
                    if (registrationModel != null)
                    {
                        var Emailers = "Emailer";
                        var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                        string filePath = Path.Combine(path, "thankyou.html");
                        //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                        StreamReader streader = new StreamReader(filePath);
                        string readFile = streader.ReadToEnd();
                        string StrContent = "";
                        StrContent = readFile;
                        StrContent = StrContent.Replace("[UserFullName]", registrationModel.Fullname);


                        ut.SendMailInfo("Thank You !", StrContent, registrationModel.EmailId);

                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Sent";
                        return Ok(apiResult);
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }

        //[Route("api/SendAbandonCart")]
        //[HttpGet]
        //public IActionResult SendAbandonCart_Email(int UserId)
        //{
        //    Registration registration = new Registration();
        //    Utilities ut = new Utilities(_config);
        //    string SpType = "E";
        //    try
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        //        {
        //            connection.Open();
        //            MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
        //            command.CommandType = System.Data.CommandType.StoredProcedure;

        //            command.Parameters.AddWithValue("_Id", UserId);
        //            command.Parameters.AddWithValue("_Fullname", registration.Fullname);
        //            command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
        //            command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
        //            command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
        //            command.Parameters.AddWithValue("_EmailId", registration.EmailId);
        //            command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
        //            command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
        //            command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
        //            command.Parameters.AddWithValue("_StateId", registration.StateId);
        //            command.Parameters.AddWithValue("_CityId", registration.CityId);
        //            command.Parameters.AddWithValue("_SpType", SpType);
        //            command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
        //            command.Parameters.AddWithValue("_IsActive", registration.IsActive);
        //            command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

        //            MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
        //            resultParam.Direction = System.Data.ParameterDirection.Output;
        //            command.Parameters.Add(resultParam);
        //            Registration registrationModel = new Registration();
        //            if (SpType == "E")
        //            {
        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {

        //                        while (reader.Read())
        //                        {
        //                            registrationModel = new Registration()
        //                            {
        //                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
        //                                Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
        //                                EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId")

        //                            };
        //                        }
        //                    }
        //                }
        //            }
        //            if (registrationModel != null)
        //            {
        //                var Emailers = "Emailer";
        //                var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
        //                string filePath = Path.Combine(path, "thankyou.html");
        //                //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
        //                StreamReader streader = new StreamReader(filePath);
        //                string readFile = streader.ReadToEnd();
        //                string StrContent = "";
        //                StrContent = readFile;
        //                StrContent = StrContent.Replace("[UserFullName]", registrationModel.Fullname);


        //                ut.SendMailInfo("USER OTP", StrContent, registrationModel.EmailId);

        //                ServiceRequestProcessor processor = new ServiceRequestProcessor();
        //                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
        //                apiResult.data = "Sent";
        //                return Ok(apiResult);
        //            }
        //            else
        //            {
        //                ServiceRequestProcessor processor = new ServiceRequestProcessor();
        //                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
        //                apiResult.data = "Not Sent";
        //                return Ok(apiResult);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceRequestProcessor processor = new ServiceRequestProcessor();
        //        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
        //        apiResult.data = "Failed";
        //        return Ok(apiResult);
        //    }
        //}


        [Route("api/SendOrderPlaced")]
        [HttpGet]
        public IActionResult SendOrderPlaced_Email(int UserId, int orderId)
        {
            Registration registration = new Registration();
            SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            string SpType = "E";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", UserId);
                    command.Parameters.AddWithValue("_Fullname", registration.Fullname);
                    command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
                    command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
                    command.Parameters.AddWithValue("_EmailId", registration.EmailId);
                    command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
                    command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
                    command.Parameters.AddWithValue("_StateId", registration.StateId);
                    command.Parameters.AddWithValue("_CityId", registration.CityId);
                    command.Parameters.AddWithValue("_SpType", SpType);
                    command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
                    command.Parameters.AddWithValue("_IsActive", registration.IsActive);
                    command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    Registration registrationModel = new Registration();
                    if (SpType == "E")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    registrationModel = new Registration()
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId")

                                    };
                                }
                            }
                        }
                    }
                    if (registrationModel != null)
                    {

                        Order orders = new Order();

                        OrderController O = new OrderController(_config, _hostEnvironment);
                        orders = O.GetOrderData(orderId, 0);

                        OrderDetailsController Od = new OrderDetailsController(_config);
                        List<OrderDetails> orderDetailsList = new List<OrderDetails>();
                        orderDetailsList = Od.GetOrderDetailsForsms(orderId);
                        string productDetails = string.Empty;
                        string tblbody = string.Empty;

                        foreach (OrderDetails item in orderDetailsList)
                        {
                            tblbody =
                            " <tr >\r\n                        <td style=\"padding-top:10px;\">\r\n                           <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row-content\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #FFFFFF; color: #333; width: 500px;\" width=\"500\">\r\n                              <tbody>\r\n                                 <tr>\r\n                                    <td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\" width=\"30%\">\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"image_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"width:100%;padding-right:0px;padding-left:0px;padding-top:0px;padding-bottom:0px;\">\r\n                                                <div align=\"center\" style=\"line-height:10px;    float: right;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<span style=\"background: #f3f3f3;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\twidth: 10px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\theight: 10px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\tpadding: 5px 9px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\tborder-radius: 50px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\tfont-size: 12px;line-height: 40px!important;\">1</span>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<img alt=\"Image\" src=" + item.MainImage + " style=\"display: block; height: auto; border: 0;     width: 80px;\r\n    padding-left: 10px; max-width: 100%;float: right;\" title=\"Image\" width=\"130\"/></div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                    </td>\r\n                                    <td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-right: 1px dotted #E8E8E8; border-top: 0px; border-bottom: 0px; border-left: 0px;\" width=\"40%\">\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"padding-bottom:0px;padding-left:10px;padding-top:0px;\">\r\n                                                <div style=\"font-family: sans-serif\">\r\n                                                   <div style=\"font-size: 12px; font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #555555; line-height: 1.2;\">\r\n                                                      <p style=\"margin: 0; font-size: 12px; text-align: left;\"><span style=\"font-size:13px;color:#2190e3;\"><strong style=\"font-weight:400;\">" + item.ProductTitle + "</strong></span></p>\r\n                                                   </div>\r\n                                                </div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"padding-bottom:0px;padding-left:10px;\">\r\n                                                <div style=\"font-family: sans-serif\">\r\n                                                   <div style=\"font-size: 12px; font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #555555; line-height: 1.2;\">\r\n                                                      <p style=\"margin: 0; font-size: 11px; text-align: left;\">₹" + item.SalePrice + "x" + item.Quantity + "</p>\r\n                                                   </div>\r\n                                                </div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                    </td>\r\n                                    <td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-right: 1px dotted #E8E8E8; border-top: 0px; border-bottom: 0px; border-left: 0px;\" width=\"10%\">\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"padding-bottom:0px;padding-left:10px;padding-right:10px;padding-top:0px;\">\r\n                                                <div style=\"font-family: sans-serif\">\r\n                                                   <div style=\"font-size: 12px; font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #555555; line-height: 1.2;\">\r\n                                                      <p style=\"margin: 0; font-size: 14px; text-align: center;\"><span style=\"font-size:14px;\">" + item.Quantity + "</span></p>\r\n                                                   </div>\r\n                                                </div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                    </td>\r\n                                    <td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\" width=\"20%\">\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"padding-right:0px;padding-top:0px;padding-bottom:0px;\">\r\n                                                <div style=\"font-family: sans-serif\">\r\n                                                   <div style=\"font-size: 12px; font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #555555; line-height: 1.2;\">\r\n                                                      <p style=\"margin: 0; text-align: center; font-size: 12px;text-align:right;\"><span style=\"font-size:20px;\"><span style=\"font-size:14px;\">" + item.TotalPrice + "</span></span></p>\r\n                                                   </div>\r\n                                                </div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                    </td>\r\n                                 </tr>\r\n                              </tbody>\r\n                           </table>\r\n                        </td>\r\n                     </tr>";
                            if (productDetails == string.Empty)
                            {
                                productDetails = item.ProductTitle;
                            }
                            else
                            {
                                productDetails = productDetails + ", " + item.ProductTitle;
                            }
                        }

                        var Emailers = "Emailer";
                        var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                        string filePath = Path.Combine(path, "order-placed.html");
                        //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                        StreamReader streader = new StreamReader(filePath);
                        string readFile = streader.ReadToEnd();
                        string StrContent = "";
                        StrContent = readFile;
                        StrContent = StrContent.Replace("[UserFullName]", registrationModel.Fullname);
                        StrContent = StrContent.Replace("[OrderDetails]", tblbody);


                        StrContent = StrContent.Replace("[OrderId]", orders.Id.ToString());
                        StrContent = StrContent.Replace("[BankRef]", "");
                        StrContent = StrContent.Replace("[PaymentMode]", orders.PaymentType.ToString());
                        StrContent = StrContent.Replace("[RewardsEarned]", orders.RewardEarned.ToString());
                        StrContent = StrContent.Replace("[SubTotal]", orders.TotalAmount.ToString());
                        StrContent = StrContent.Replace("[Shipping]", "");
                        StrContent = StrContent.Replace("[Discount]", orders.DiscountAmount.ToString());
                        StrContent = StrContent.Replace("[NetTotal]", orders.TotalAmount.ToString());
                        StrContent = StrContent.Replace("[GST]", "");
                        StrContent = StrContent.Replace("[Total]", orders.GrandTotal.ToString());


                        ut.SendMailInfo("Order Placed", StrContent, registrationModel.EmailId);

                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Sent";
                        return Ok(apiResult);
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }

        [Route("api/SendOrderConfirmed")]
        [HttpGet]
        public IActionResult SendOrderConfirmed_Email(int UserId, int orderId, string subject)
        {
            Registration registration = new Registration();
            SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            string SpType = "E";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", UserId);
                    command.Parameters.AddWithValue("_Fullname", registration.Fullname);
                    command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
                    command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
                    command.Parameters.AddWithValue("_EmailId", registration.EmailId);
                    command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
                    command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
                    command.Parameters.AddWithValue("_StateId", registration.StateId);
                    command.Parameters.AddWithValue("_CityId", registration.CityId);
                    command.Parameters.AddWithValue("_SpType", SpType);
                    command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
                    command.Parameters.AddWithValue("_IsActive", registration.IsActive);
                    command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    Registration registrationModel = new Registration();
                    if (SpType == "E")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    registrationModel = new Registration()
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId")

                                    };
                                }
                            }
                        }
                    }
                    if (registrationModel != null)
                    {

                        Order orders = new Order();

                        OrderController O = new OrderController(_config, _hostEnvironment);
                        orders = O.GetOrderData(orderId, 0);

                        OrderDetailsController Od = new OrderDetailsController(_config);
                        List<OrderDetails> orderDetailsList = new List<OrderDetails>();
                        orderDetailsList = Od.GetOrderDetailsForsms(orderId);
                        string productDetails = string.Empty;
                        string tblbody = string.Empty;

                        foreach (OrderDetails item in orderDetailsList)
                        {
                            tblbody =
                            " <tr >\r\n                        <td style=\"padding-top:10px;\">\r\n                           <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row-content\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #FFFFFF; color: #333; width: 500px;\" width=\"500\">\r\n                              <tbody>\r\n                                 <tr>\r\n                                    <td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\" width=\"30%\">\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"image_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"width:100%;padding-right:0px;padding-left:0px;padding-top:0px;padding-bottom:0px;\">\r\n                                                <div align=\"center\" style=\"line-height:10px;    float: right;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<span style=\"background: #f3f3f3;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\twidth: 10px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\theight: 10px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\tpadding: 5px 9px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\tborder-radius: 50px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\tfont-size: 12px;line-height: 40px!important;\">1</span>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<img alt=\"Image\" src=" + item.MainImage + " style=\"display: block; height: auto; border: 0;     width: 80px;\r\n    padding-left: 10px; max-width: 100%;float: right;\" title=\"Image\" width=\"130\"/></div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                    </td>\r\n                                    <td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-right: 1px dotted #E8E8E8; border-top: 0px; border-bottom: 0px; border-left: 0px;\" width=\"40%\">\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"padding-bottom:0px;padding-left:10px;padding-top:0px;\">\r\n                                                <div style=\"font-family: sans-serif\">\r\n                                                   <div style=\"font-size: 12px; font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #555555; line-height: 1.2;\">\r\n                                                      <p style=\"margin: 0; font-size: 12px; text-align: left;\"><span style=\"font-size:13px;color:#2190e3;\"><strong style=\"font-weight:400;\">" + item.ProductTitle + "</strong></span></p>\r\n                                                   </div>\r\n                                                </div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"padding-bottom:0px;padding-left:10px;\">\r\n                                                <div style=\"font-family: sans-serif\">\r\n                                                   <div style=\"font-size: 12px; font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #555555; line-height: 1.2;\">\r\n                                                      <p style=\"margin: 0; font-size: 11px; text-align: left;\">₹" + item.SalePrice + "x" + item.Quantity + "</p>\r\n                                                   </div>\r\n                                                </div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                    </td>\r\n                                    <td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-right: 1px dotted #E8E8E8; border-top: 0px; border-bottom: 0px; border-left: 0px;\" width=\"10%\">\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"padding-bottom:0px;padding-left:10px;padding-right:10px;padding-top:0px;\">\r\n                                                <div style=\"font-family: sans-serif\">\r\n                                                   <div style=\"font-size: 12px; font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #555555; line-height: 1.2;\">\r\n                                                      <p style=\"margin: 0; font-size: 14px; text-align: center;\"><span style=\"font-size:14px;\">" + item.Quantity + "</span></p>\r\n                                                   </div>\r\n                                                </div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                    </td>\r\n                                    <td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\" width=\"20%\">\r\n                                       <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\r\n                                          <tr>\r\n                                             <td style=\"padding-right:0px;padding-top:0px;padding-bottom:0px;\">\r\n                                                <div style=\"font-family: sans-serif\">\r\n                                                   <div style=\"font-size: 12px; font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #555555; line-height: 1.2;\">\r\n                                                      <p style=\"margin: 0; text-align: center; font-size: 12px;text-align:right;\"><span style=\"font-size:20px;\"><span style=\"font-size:14px;\">" + item.TotalPrice + "</span></span></p>\r\n                                                   </div>\r\n                                                </div>\r\n                                             </td>\r\n                                          </tr>\r\n                                       </table>\r\n                                    </td>\r\n                                 </tr>\r\n                              </tbody>\r\n                           </table>\r\n                        </td>\r\n                     </tr>";
                            if (productDetails == string.Empty)
                            {
                                productDetails = item.ProductTitle;
                            }
                            else
                            {
                                productDetails = productDetails + ", " + item.ProductTitle;
                            }
                        }

                        var Emailers = "Emailer";
                        var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                        string filePath = Path.Combine(path, "order-status.html");
                        //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                        StreamReader streader = new StreamReader(filePath);
                        string readFile = streader.ReadToEnd();
                        string StrContent = "";
                        StrContent = readFile;
                        StrContent = StrContent.Replace("[UserFullName]", registrationModel.Fullname);
                        StrContent = StrContent.Replace("[OrderDetails]", tblbody);


                        StrContent = StrContent.Replace("[OrderId]", orders.Id.ToString());
                        StrContent = StrContent.Replace("[BankRef]", "");
                        StrContent = StrContent.Replace("[PaymentMode]", orders.PaymentType.ToString());
                        StrContent = StrContent.Replace("[RewardsEarned]", orders.RewardEarned.ToString());
                        StrContent = StrContent.Replace("[SubTotal]", orders.TotalAmount.ToString());
                        StrContent = StrContent.Replace("[Shipping]", "");
                        StrContent = StrContent.Replace("[Discount]", orders.DiscountAmount.ToString());
                        StrContent = StrContent.Replace("[NetTotal]", orders.TotalAmount.ToString());
                        StrContent = StrContent.Replace("[GST]", "");
                        StrContent = StrContent.Replace("[Total]", orders.GrandTotal.ToString());

                        StrContent = StrContent.Replace("[OrderDate]", orders.OrderDate.ToString());
                        StrContent = StrContent.Replace("[ShippingDate]", "");
                        StrContent = StrContent.Replace("[DeliveryDate]", "");
                        StrContent = StrContent.Replace("[OrderNumber]", "");


                        ut.SendMailInfo(subject, StrContent, registrationModel.EmailId);

                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Sent";
                        return Ok(apiResult);
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }

        [Route("api/SendOrderCancelled")]
        [HttpGet]
        public IActionResult SendOrderCancelled_Email(int UserId)
        {
            Registration registration = new Registration();
            SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            string SpType = "E";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", UserId);
                    command.Parameters.AddWithValue("_Fullname", registration.Fullname);
                    command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
                    command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
                    command.Parameters.AddWithValue("_EmailId", registration.EmailId);
                    command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
                    command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
                    command.Parameters.AddWithValue("_StateId", registration.StateId);
                    command.Parameters.AddWithValue("_CityId", registration.CityId);
                    command.Parameters.AddWithValue("_SpType", SpType);
                    command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
                    command.Parameters.AddWithValue("_IsActive", registration.IsActive);
                    command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    Registration registrationModel = new Registration();
                    if (SpType == "E")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    registrationModel = new Registration()
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId")

                                    };
                                }
                            }
                        }
                    }
                    if (registrationModel != null)
                    {
                        var Emailers = "Emailer";
                        var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                        string filePath = Path.Combine(path, "order-cancelled.html");
                        //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                        StreamReader streader = new StreamReader(filePath);
                        string readFile = streader.ReadToEnd();
                        string StrContent = "";
                        StrContent = readFile;
                        StrContent = StrContent.Replace("[UserFullName]", registrationModel.Fullname);
                        //StrContent = StrContent.Replace("[OTP]", OTP.ToString());

                        ut.SendMailInfo("Order Cancelled", StrContent, registrationModel.EmailId);

                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Sent";
                        return Ok(apiResult);
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }
        }

        [Route("api/SendOrderInvoice")]
        [HttpGet]
        public IActionResult SendOrderInvoice(int orderId)
        {
            SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getinvoicedata", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_OrderId", orderId);

                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(ds);

                    if (ds.Tables.Count == 2)
                    {
                        // Access values from the first table
                        DataTable table1 = ds.Tables[0];
                        DataTable table2 = ds.Tables[1];
                        if (table1.Rows.Count > 0)
                        {
                            DataRow row1 = table1.Rows[0];
                            //string valueFromTable1 = row1["ColumnName"].ToString();
                            //lblLabel1.Text = valueFromTable1;
                            var Emailers = "Emailer";
                            var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                            string filePath = Path.Combine(path, "Invoice.html");
                            //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                            StreamReader streader = new StreamReader(filePath);
                            string readFile = streader.ReadToEnd();
                            string StrContent = "";
                            StrContent = readFile;
                            StrContent = StrContent.Replace("[OrderId]", table1.Rows[0]["OrderId"].ToString());
                            StrContent = StrContent.Replace("[ProductTitle]", table1.Rows[0]["ProductTitle"].ToString());
                            StrContent = StrContent.Replace("[Quantity]", table1.Rows[0]["Quantity"].ToString());
                            StrContent = StrContent.Replace("[Price]", table1.Rows[0]["Price"].ToString());
                            StrContent = StrContent.Replace("[SalePrice]", table1.Rows[0]["SalePrice"].ToString());
                            StrContent = StrContent.Replace("[TotalPrice]", table1.Rows[0]["TotalPrice"].ToString());
                            StrContent = StrContent.Replace("[InvoiceNo]", table1.Rows[0]["InvoiceNumber"].ToString());
                            StrContent = StrContent.Replace("[EwayNo]", table1.Rows[0]["EwayNumber"].ToString());
                            StrContent = StrContent.Replace("[OrderId]", table1.Rows[0]["OrderId"].ToString());
                            StrContent = StrContent.Replace("[OrderId]", table1.Rows[0]["OrderId"].ToString());
                            StrContent = StrContent.Replace("[CompanyName]", table2.Rows[0]["CompanyName"].ToString());
                            StrContent = StrContent.Replace("[CompanyAddress]", table2.Rows[0]["ShippingAddress"].ToString());
                            StrContent = StrContent.Replace("[Pincode]", table2.Rows[0]["Pincode"].ToString());
                            StrContent = StrContent.Replace("[City]", table2.Rows[0]["City"].ToString());
                            StrContent = StrContent.Replace("[State]", table2.Rows[0]["State"].ToString());
                            StrContent = StrContent.Replace("[MobileNo]", table2.Rows[0]["MobileNo"].ToString());
                            StrContent = StrContent.Replace("[Fullname]", table2.Rows[0]["Fullname"].ToString());
                            StrContent = StrContent.Replace("[EmailId]", table2.Rows[0]["EmailId"].ToString());
                            StrContent = StrContent.Replace("[PaymentType]", table2.Rows[0]["PaymentType"].ToString());
                            StrContent = StrContent.Replace("[OrderDate]", table2.Rows[0]["OrderDate"].ToString());
                            StrContent = StrContent.Replace("[TotalAmount]", table2.Rows[0]["TotalAmount"].ToString());
                            StrContent = StrContent.Replace("[GrandTotal]", table2.Rows[0]["GrandTotal"].ToString());


                            var FolderPath = Path.Combine(this._hostEnvironment.WebRootPath, "Invoice_Letter", DateTime.Now.ToString("dd-MM-yyyy"));
                            if (!Directory.Exists(FolderPath))
                            {
                                Directory.CreateDirectory(FolderPath);
                            }

                            string attchPath = "~Invoice.pdf";
                            string savepath = "";
                            StringReader sb = new StringReader(StrContent);
                            Document doc = new Document(PageSize.A4, 10, 10, 42, 35);
                            //byte[] bytes;
                            //HTMLWorker htmlworker = new HTMLWorker(doc);
                            using (MemoryStream stream = new MemoryStream())
                            {
                                HtmlConverter.ConvertToPdf(StrContent, stream);
                                savepath = Path.Combine(this._hostEnvironment.WebRootPath, FolderPath, attchPath);
                                System.IO.File.WriteAllBytes(savepath, stream.ToArray());
                                //PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                                //doc.Open();
                                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sb);
                                //doc.Close();
                                ////bytes = stream.ToArray();
                                ////System.IO.File.WriteAllBytes(savepath, bytes);
                                ////logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter saved to the path : " + savepath);
                                ut.SendMailPDF("Order Placed", "Invoice", "chetan@digitaledgetech.in", stream);
                                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = "Sent";
                                return Ok(apiResult);
                            }
                        }
                        else
                        {
                            return Ok();
                        }


                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = "Failed";
                return Ok(apiResult);
            }


        }

        [Route("api/GetOrderInvoiceData")]
        [HttpGet]
        public IActionResult GetOrderInvoiceData(int orderId)
        {
            //SynfoShopAPI.DAL.Utilitiy ut = new Utilitiy(_config);
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getinvoicedata", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_OrderId", orderId);

                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(ds);

                    if (ds.Tables.Count == 2)
                    {
                        // Access values from the first table
                        DataTable table1 = ds.Tables[0];
                        DataTable table2 = ds.Tables[1];
                        if (table1.Rows.Count > 0)
                        {
                            DataRow row1 = table1.Rows[0];
                            //string valueFromTable1 = row1["ColumnName"].ToString();
                            //lblLabel1.Text = valueFromTable1;
                            var Emailers = "Emailer";
                            var path = Path.Combine(this._hostEnvironment.WebRootPath, Emailers);
                            string filePath = Path.Combine(path, "Invoice.html");
                            //logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Letter template path : " + filePath);
                            StreamReader streader = new StreamReader(filePath);
                            string readFile = streader.ReadToEnd();
                            string StrContent = "";
                            StrContent = readFile;
                            StrContent = StrContent.Replace("[OrderId]", table1.Rows[0]["OrderId"].ToString());
                            StrContent = StrContent.Replace("[ProductTitle]", table1.Rows[0]["ProductTitle"].ToString());
                            StrContent = StrContent.Replace("[Quantity]", table1.Rows[0]["Quantity"].ToString());
                            StrContent = StrContent.Replace("[Price]", table1.Rows[0]["Price"].ToString());
                            StrContent = StrContent.Replace("[SalePrice]", table1.Rows[0]["SalePrice"].ToString());
                            StrContent = StrContent.Replace("[TotalPrice]", table1.Rows[0]["TotalPrice"].ToString());
                            StrContent = StrContent.Replace("[InvoiceNo]", table1.Rows[0]["InvoiceNumber"].ToString());
                            StrContent = StrContent.Replace("[EwayNo]", table1.Rows[0]["EwayNumber"].ToString());
                            StrContent = StrContent.Replace("[OrderId]", table1.Rows[0]["OrderId"].ToString());
                            StrContent = StrContent.Replace("[OrderId]", table1.Rows[0]["OrderId"].ToString());
                            StrContent = StrContent.Replace("[CompanyName]", table2.Rows[0]["CompanyName"].ToString());
                            StrContent = StrContent.Replace("[CompanyAddress]", table2.Rows[0]["ShippingAddress"].ToString());
                            StrContent = StrContent.Replace("[Pincode]", table2.Rows[0]["Pincode"].ToString());
                            StrContent = StrContent.Replace("[City]", table2.Rows[0]["City"].ToString());
                            StrContent = StrContent.Replace("[State]", table2.Rows[0]["State"].ToString());
                            StrContent = StrContent.Replace("[MobileNo]", table2.Rows[0]["MobileNo"].ToString());
                            StrContent = StrContent.Replace("[Fullname]", table2.Rows[0]["Fullname"].ToString());
                            StrContent = StrContent.Replace("[EmailId]", table2.Rows[0]["EmailId"].ToString());
                            StrContent = StrContent.Replace("[PaymentType]", table2.Rows[0]["PaymentType"].ToString());
                            StrContent = StrContent.Replace("[OrderDate]", table2.Rows[0]["OrderDate"].ToString());
                            StrContent = StrContent.Replace("[TotalAmount]", table2.Rows[0]["TotalAmount"].ToString());
                            StrContent = StrContent.Replace("[GrandTotal]", table2.Rows[0]["GrandTotal"].ToString());


                            ServiceRequestProcessor processor = new ServiceRequestProcessor();
                            APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = StrContent;
                            return Ok(StrContent);

                        }
                        else
                        {
                            return Ok();
                        }
                    }
                    else
                    {
                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = "Not Sent";
                        return Ok(apiResult);
                    }
                }
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

