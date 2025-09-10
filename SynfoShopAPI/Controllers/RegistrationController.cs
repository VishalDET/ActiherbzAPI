using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit.Encodings;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Twilio.Http;
using static System.Net.WebRequestMethods;

namespace SynfoShopAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string GSTClientId = string.Empty;
        private readonly string GSTClientSecret = string.Empty;

        public RegistrationController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            _hostEnvironment = hostEnvironment;
            GSTClientId = _config["AppSeettings:GSTClientId"];
            GSTClientSecret = _config["AppSeettings:GSTclient_secret"];
        }

        [Route("api/Registration")]
        [HttpPost]
        public IActionResult CommonUser(Registration registration)
        {
            SMSController smsC = new SMSController(_config);
            EmailerController EmailC = new EmailerController(_config, _hostEnvironment);
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", registration.Id);
                    command.Parameters.AddWithValue("_Fullname", registration.Fullname);
                    command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
                    command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
                    command.Parameters.AddWithValue("_EmailId", registration.EmailId);
                    command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
                    command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
                    command.Parameters.AddWithValue("_BillingAddress", registration.BillingAddress);
                    command.Parameters.AddWithValue("_StateId", registration.StateId);
                    command.Parameters.AddWithValue("_CityId", registration.CityId);
                    command.Parameters.AddWithValue("_Pincode", registration.Pincode);
                    command.Parameters.AddWithValue("_SpType", registration.SpType);
                    command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
                    command.Parameters.AddWithValue("_IsActive", registration.IsActive);
                    command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (registration.SpType == "C" || registration.SpType == "U")
                    {
                        command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.Parameters["_Result"].Value);


                        if (result != 0)
                        {
                            if (registration.SpType == "C")
                            {
                                smsC.UserRegistrationMSG(registration.MobileNo, registration.Fullname);
                                EmailC.SendRegister_Email(registration.Id);
                            }
                            else if (registration.IsActive == 1 && registration.SpType == "U")
                            {
                                EmailC.SendWelcome_Email(result);
                            }
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
                    else if (registration.SpType == "E" || registration.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Registration> registrationList = new List<Registration>();

                                while (reader.Read())
                                {
                                    var registrationModel = new Registration
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                        CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString("CompanyName"),
                                        MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString("MobileNo"),
                                        NatureOfBusiness = reader.IsDBNull(reader.GetOrdinal("NatureOfBusiness")) ? 0 : reader.GetInt32("NatureOfBusiness"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId"),
                                        IsGSTIN = reader.GetBoolean(reader.GetOrdinal("IsGSTIN")),
                                        GSTNumber = reader.IsDBNull(reader.GetOrdinal("GSTNumber")) ? null : reader.GetString("GSTNumber"),
                                        ShippingAddress = reader.IsDBNull(reader.GetOrdinal("ShippingAddress")) ? null : reader.GetString("ShippingAddress"),
                                        StateName = reader.IsDBNull(reader.GetOrdinal("StateName")) ? null : reader.GetString("StateName"),
                                        StateId = reader.IsDBNull(reader.GetOrdinal("StateId")) ? 0 : reader.GetInt32("StateId"),
                                        CityName = reader.IsDBNull(reader.GetOrdinal("CityName")) ? null : reader.GetString("CityName"),
                                        CityId = reader.IsDBNull(reader.GetOrdinal("CityId")) ? 0 : reader.GetInt32("CityId"),
                                        Pincode = reader.IsDBNull(reader.GetOrdinal("Pincode")) ? 0 : reader.GetInt32("Pincode"),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? 0 : reader.GetInt32("IsActive"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                                    };

                                    registrationList.Add(registrationModel);
                                }


                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = registrationList;
                                return Ok(apiResult);

                                //return Ok(registrationList);
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


        [Route("api/RegistrationWithSalesUser")]
        [HttpPost]
        public IActionResult CommonUserWithSales(RegistrationWithSalesUser registration)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuserwithsalesuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", registration.Id);
                    command.Parameters.AddWithValue("_Fullname", registration.Fullname);
                    command.Parameters.AddWithValue("_CompanyName", registration.CompanyName);
                    command.Parameters.AddWithValue("_MobileNo", registration.MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", registration.NatureOfBusiness);
                    command.Parameters.AddWithValue("_EmailId", registration.EmailId);
                    command.Parameters.AddWithValue("_Password", registration.Password);
                    command.Parameters.AddWithValue("_IsGSTIN", registration.IsGSTIN);
                    command.Parameters.AddWithValue("_GSTNumber", registration.GSTNumber);
                    command.Parameters.AddWithValue("_ShippingAddress", registration.ShippingAddress);
                    command.Parameters.AddWithValue("_StateId", registration.StateId);
                    command.Parameters.AddWithValue("_CityId", registration.CityId);
                    command.Parameters.AddWithValue("_SpType", registration.SpType);
                    command.Parameters.AddWithValue("_CreatedDate", registration.CreatedDate);
                    command.Parameters.AddWithValue("_IsActive", registration.IsActive);
                    command.Parameters.AddWithValue("_SalesUserId", registration.SalesUserId);
                    command.Parameters.AddWithValue("_Taluka", registration.Taluka);
                    command.Parameters.AddWithValue("_BillingAddress", registration.BillingAddress);
                    command.Parameters.AddWithValue("_Pincode", registration.Pincode);
                    command.Parameters.AddWithValue("_ModifiedDate", registration.ModifiedDate);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    if (registration.SpType == "C" || registration.SpType == "U")
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
                    else if (registration.SpType == "E" || registration.SpType == "R")
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<RegistrationWithSalesUser> registrationList = new List<RegistrationWithSalesUser>();

                                while (reader.Read())
                                {
                                    var registrationModel = new RegistrationWithSalesUser
                                    {
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        Fullname = reader.IsDBNull(reader.GetOrdinal("Fullname")) ? null : reader.GetString("Fullname"),
                                        CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString("CompanyName"),
                                        MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString("MobileNo"),
                                        NatureOfBusinessName = reader.IsDBNull(reader.GetOrdinal("NatureOfBusiness")) ? null : reader.GetString("NatureOfBusiness"),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId"),
                                        Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString("Password"),
                                        IsGSTIN = reader.GetBoolean(reader.GetOrdinal("IsGSTIN")),
                                        GSTNumber = reader.IsDBNull(reader.GetOrdinal("GSTNumber")) ? null : reader.GetString("GSTNumber"),
                                        ShippingAddress = reader.IsDBNull(reader.GetOrdinal("ShippingAddress")) ? null : reader.GetString("ShippingAddress"),
                                        BillingAddress = reader.IsDBNull(reader.GetOrdinal("BillingAddress")) ? null : reader.GetString("BillingAddress"),
                                        StateName = reader.IsDBNull(reader.GetOrdinal("StateName")) ? null : reader.GetString("StateName"),
                                        StateId = reader.IsDBNull(reader.GetOrdinal("StateId")) ? 0 : reader.GetInt32("StateId"),
                                        CityName = reader.IsDBNull(reader.GetOrdinal("CityName")) ? null : reader.GetString("CityName"),
                                        CityId = reader.IsDBNull(reader.GetOrdinal("CityId")) ? 0 : reader.GetInt32("CityId"),
                                        Pincode = reader.IsDBNull(reader.GetOrdinal("Pincode")) ? 0 : reader.GetInt32("Pincode"),
                                        CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                        IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? 0 : reader.GetInt32("IsActive"),
                                        SalesUserId = reader.IsDBNull(reader.GetOrdinal("SalesUserId")) ? 0 : reader.GetInt32("SalesUserId"),
                                        SalesFullName = reader.IsDBNull(reader.GetOrdinal("SalesFullName")) ? null : reader.GetString("SalesFullName"),
                                        Taluka = reader.IsDBNull(reader.GetOrdinal("Taluka")) ? null : reader.GetString("Taluka"),
                                        ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                                    };

                                    registrationList.Add(registrationModel);
                                }
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = registrationList;
                                return Ok(apiResult);
                                //return Ok(registrationList);
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

        [Route("api/GetGstToken")]
        [HttpPost]
        public async Task<string> GetGstToken()
        {
            // Define the endpoint URL
            string apiUrl = "https://production.deepvue.tech/v1/authorize";

            // Prepare the request data
            var requestData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", GSTClientId),
                new KeyValuePair<string, string>("client_secret", GSTClientSecret)
            });
            using (System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient())
            {
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, requestData);

                // Handle the response
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var deserializedObject = JsonConvert.DeserializeObject<GSTToken>(response.Content.ReadAsStringAsync().Result);
                    string Token = deserializedObject.access_token.ToString();
                    int result = 0;
                    using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("Insert Into tblgsttokentracker(access_token, token_type, expiry) Values('" + Token + "', '" + deserializedObject.token_type.ToString() + "', '" + deserializedObject.expiry.ToString() + "');", connection);
                        result = command.ExecuteNonQuery();
                    }
                    return Token;

                }
                else
                {
                    // Handle error response here
                    Console.WriteLine("Error: " + response.StatusCode);
                    return null;
                }
            };
            // Make the POST request


        }

        private GSTTokenView GetTokenDetails()
        {

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT *, CASE WHEN DATE_ADD(CreatedDate, INTERVAL 24 HOUR) >= current_time() THEN 1 ELSE 0 end as Validity  FROM tblgsttokentracker order by Id Desc ", connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        GSTTokenView gstToken = new GSTTokenView
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            access_token = reader["access_token"].ToString(),
                            token_type = reader["token_type"].ToString(),
                            expiry = reader["expiry"].ToString(),
                            Validity = reader.GetInt32(reader.GetOrdinal("Validity"))
                        };
                        if (gstToken.Validity == 0)
                        {
                            gstToken = null;
                        }
                        return gstToken;
                    }
                }
            }

            return null;
        }
        [Route("api/GSTDatBasedOnGST")]
        [HttpGet]
        public async Task<GSTApiResponseNew> GSTDatBasedOnGST(string GSTNumber)
        {
            GSTTokenView gSTTokenView = new GSTTokenView();
            GSTApiResponseNew re = new GSTApiResponseNew();
            //gSTTokenView = GetTokenDetails();
            string accessToken = string.Empty;
            //if (gSTTokenView != null)
            //{
            //    accessToken = gSTTokenView.access_token.ToString();
            //}
            //else
            //{
            //    accessToken = await GetGstToken();
            //}
            // Define the endpoint URL
            // string apiUrl = "https://production.deepvue.tech/v1/verification/gstinlite?gstin_number=" + GSTNumber;

            string apiUrl = "https://appyflow.in/api/verifyGST?key_secret=gLfKZh1gpMR8VsAaKngArBET8cf2&gstNo=" + GSTNumber;

            // Prepare the request data

            using (System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient())
            {
                //_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                // _httpClient.DefaultRequestHeaders.Add("x-api-key", GSTClientSecret);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                // Handle the response
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Root deserializedObject = JsonConvert.DeserializeObject<Root>(response.Content.ReadAsStringAsync().Result);
                    //string Token = deserializedObject.access_token[0].ToString();
                    re.Lgnm = deserializedObject.taxpayerInfo.lgnm;
                    re.Pncd = deserializedObject.taxpayerInfo.pradr.addr.pncd;
                    re.Bno = deserializedObject.taxpayerInfo.pradr.addr.bno;
                    re.Bnm = deserializedObject.taxpayerInfo.pradr.addr.bnm;
                    re.Loc = deserializedObject.taxpayerInfo.pradr.addr.loc;
                    re.St = deserializedObject.taxpayerInfo.pradr.addr.stcd;
                    re.Dst = deserializedObject.taxpayerInfo.pradr.addr.dst;

                    return re;

                }
                else
                {
                    // Handle error response here
                    Console.WriteLine("Error: " + response.StatusCode);
                    return null;
                }
            };
            // Make the POST request


        }

        [Route("api/GetNatureOfBusiness")]
        [HttpGet]
        public IActionResult GetNatureOfBusiness()
        {
            List<BusinessType> businessTypes = new List<BusinessType>();

            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (MySqlCommand command = new MySqlCommand("usp_getbusinessname", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BusinessType businessType = new BusinessType
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString()
                            };
                            businessTypes.Add(businessType);
                        }
                    }
                }
            }

            return Ok(businessTypes);
        }



        [Route("api/UserDeactivate")]
        [HttpPost]
        public IActionResult UserAccountDeativate(int Id, string MobileNo)
        {
            SMSController smsC = new SMSController(_config);
            EmailerController EmailC = new EmailerController(_config, _hostEnvironment);
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    string SpType = "D";
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_commonuser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", Id);
                    command.Parameters.AddWithValue("_Fullname", "");
                    command.Parameters.AddWithValue("_CompanyName", "");
                    command.Parameters.AddWithValue("_MobileNo", MobileNo);
                    command.Parameters.AddWithValue("_NatureOfBusiness", 0);
                    command.Parameters.AddWithValue("_EmailId", "");
                    command.Parameters.AddWithValue("_IsGSTIN", false);
                    command.Parameters.AddWithValue("_GSTNumber", "");
                    command.Parameters.AddWithValue("_ShippingAddress", "");
                    command.Parameters.AddWithValue("_IsActive", 0);
                    command.Parameters.AddWithValue("_StateId", 0);
                    command.Parameters.AddWithValue("_CityId", 0);
                    command.Parameters.AddWithValue("_Pincode", 0);
                    command.Parameters.AddWithValue("_BillingAddress", "");
                    command.Parameters.AddWithValue("_SpType", "D");

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
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
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }
    }
}
