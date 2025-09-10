using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SynfoShopAPI.Models;
using System.Collections.Generic;
using GOKURTISAPI.Models;
using System.Text;

namespace GOKURTISAPI.Controllers
{
    [ApiController]
    public class DelhiveryController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _ApiKey;
        private readonly string _BaseUrl;
        // private const string BaseUrl = "https://staging-express.delhivery.com/c/api/pin-codes/json/";
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DelhiveryController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
            _ApiKey = _config["Delhivery:ApiKey"];
            _BaseUrl = _config["Delhivery:BaseUrl"];
        }


        [HttpGet]
        [Route("api/CheckServiceability")]
        public async Task<IActionResult> CheckServiceability(string pincode)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                if (string.IsNullOrEmpty(pincode))
                {
                    return BadRequest(new { message = "Pincode is required" });
                }

                string endpoint = $"{_BaseUrl}c/api/pin-codes/json/?filter_codes={pincode}";
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");



                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serviceabilityResponse = JsonConvert.DeserializeObject<ServiceabilityResponse>(responseBody);

                    var postalCodeInfo = serviceabilityResponse.delivery_codes[0]?.postal_code;

                    if (postalCodeInfo != null)
                    {
                        var remarks = postalCodeInfo.remarks;
                        if (remarks == "")
                        {
                            //using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                            //{
                            //    connection.Open();
                            //    MySqlCommand command = new MySqlCommand("usppincodeserviceability", connection);
                            //    command.CommandType = System.Data.CommandType.StoredProcedure;

                            //    command.Parameters.AddWithValue("_Pincode", postalCodeInfo.pin);
                            //    command.Parameters.AddWithValue("_City", postalCodeInfo.city);
                            //    command.Parameters.AddWithValue("_District", postalCodeInfo.district);
                            //    command.Parameters.AddWithValue("_StateCode", postalCodeInfo.state_code);
                            //    command.Parameters.AddWithValue("_COD", postalCodeInfo.cod == "Y");
                            //    command.Parameters.AddWithValue("_Prepaid", postalCodeInfo.pre_paid == "Y");
                            //    command.Parameters.AddWithValue("_PickupAvailable", postalCodeInfo.pickup == "Y");
                            //    command.Parameters.AddWithValue("_IsODA", postalCodeInfo.is_oda == "Y");

                            //    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                            //    resultParam.Direction = System.Data.ParameterDirection.Output;
                            //    command.Parameters.Add(resultParam);
                            //    command.ExecuteNonQuery();
                            //}
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = serviceabilityResponse.delivery_codes;
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = null;
                            return Ok(apiResult);
                        }
                        // Save to database if necessary
                        //return Ok(postalCodeInfo);
                    }
                    else
                    {
                        return NotFound(new { message = "Pincode not serviceable" });
                    }
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new { message = "Failed to fetch serviceability information" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }


        [HttpPost]
        [Route("api/CreateWarehouse")]
        public async Task<IActionResult> CreateWarehouse(WarehouseRequests warehouseRequest)
        {
            try
            {

                if (warehouseRequest == null)
                {
                    return BadRequest(new { message = "Invalid request payload" });
                }
                var jsonRequest = JsonConvert.SerializeObject(warehouseRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Clear();
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");

                HttpResponseMessage response = await _httpClient.PostAsync(_BaseUrl + "api/backend/clientwarehouse/create/", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseContent = JsonConvert.DeserializeObject(responseBody);

                    return Ok(new { success = true, data = responseContent });
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new { message = "Failed to create warehouse" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }


        [HttpGet]
        [Route("api/GetWaybill")]
        public async Task<IActionResult> GetWaybill([FromQuery] int count = 1)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {
                string client_name = "";
                string endpoint = $"{_BaseUrl}waybill/api/bulk/json/?cl={client_name}&count={count}";
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");
                //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var waybillResponse = JsonConvert.DeserializeObject(responseBody);

                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = waybillResponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = response.ToString();
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = ex.Message.ToString();
                return Ok(apiResult);
            }
        }


        [HttpGet]
        [Route("api/GetCharges")]
        public async Task<IActionResult> GetCharges(string md, string ss, string d_pin, string o_pin, int cgm, string pt, int cod)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {

                // Construct the query parameters
                string endpoint = $"{_BaseUrl}api/kinko/v1/invoice/charges/.json?md={md}&ss={ss}&d_pin={d_pin}&o_pin={o_pin}&cgm={cgm}&pt={pt}&cod={cod}";

                // Set the headers for the request
                _httpClient.DefaultRequestHeaders.Clear();

                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");
                //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                // Send the GET request to Delhivery API
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var chargesResponse = JsonConvert.DeserializeObject<List<CalculateShippingCostResponseModel>>(responseBody);

                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = chargesResponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = response.ToString();
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = ex.Message.ToString();
                return Ok(apiResult);
            }
        }


        [HttpPost]
        [Route("api/CreatePickup")]
        public async Task<IActionResult> CreatePickup(PickupRequest pickupRequest)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {
                if (pickupRequest == null)
                {
                    return BadRequest(new { message = "Invalid request payload" });
                }


                // Serialize the request data into the expected format
                var requestData = new
                {
                    data = pickupRequest
                };

                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                // Set headers for the request_httpClient.DefaultRequestHeaders.Clear();

                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");

                var jsonRequest = JsonConvert.SerializeObject(requestData);
                var newdata = jsonRequest.Replace("{\"data\":", "format=json&data=").Replace("}}", "}");
                var content = new StringContent(newdata, Encoding.UTF8, "application/json");


                //HttpResponseMessage response = await _httpClient.PostAsync("https://staging-express.delhivery.com/api/cmu/create.json", content);
                HttpResponseMessage response = await _httpClient.PostAsync("https://track.delhivery.com/api/cmu/create.json", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var pickupResponse = JsonConvert.DeserializeObject<ShipmentResponse>(responseBody);

                    var success = pickupResponse.success;
                    if (success == true)
                    {
                        string ewaybill = pickupResponse.packages[0].waybill.ToString();
                        int OrderId = SynfoShopAPI.DAL.Utilities.validateInt(pickupRequest.shipments[0].order.ToString().ToLower().Replace("go-", ""));
                        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                        {
                            connection.Open();
                            MySqlCommand command = new MySqlCommand("usp_updateshipmentdetails", connection);
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("_OrderId", OrderId);
                            command.Parameters.AddWithValue("_EwayBill", ewaybill);
                            command.Parameters.AddWithValue("_Shipmentdetails", responseBody);

                            MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                            resultParam.Direction = System.Data.ParameterDirection.Output;
                            command.Parameters.Add(resultParam);
                            command.ExecuteNonQuery();
                        }
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = pickupResponse;
                        return Ok(apiResult);
                    }
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = pickupResponse;
                        return Ok(apiResult);
                    }

                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = response.ToString();
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = ex.Message.ToString();
                return Ok(apiResult);
            }
        }

        [HttpGet]
        [Route("api/TrackingApi")]
        public async Task<IActionResult> TrackingApi(string waybill, string orderNumber)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                if (string.IsNullOrEmpty(waybill))
                {
                    return BadRequest(new { message = "Waybill is required" });
                }

                string endpoint = $"{_BaseUrl}api/v1/packages/json/?waybill={waybill}";
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");



                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serviceabilityResponse = JsonConvert.DeserializeObject<ResponseShipmentData>(responseBody);

                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = serviceabilityResponse;
                    return Ok(apiResult);

                }
                else
                {
                    return StatusCode((int)response.StatusCode, new { message = "Failed to fetch way bill information" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }


        [HttpGet]
        [Route("api/GetPackageDetails")]
        public async Task<IActionResult> GetPackageDetails(string waybill, string refIds = "")
        {
            try
            {
                if (string.IsNullOrEmpty(waybill))
                {
                    return BadRequest(new { message = "Waybill is required" });
                }

                string endpoint = $"{_BaseUrl}api/v1/packages/json/?waybill={waybill}";

                // Set headers for the request
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");

                // Send GET request to Delhivery API
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var packageResponse = JsonConvert.DeserializeObject(responseBody);

                    return Ok(new { success = true, data = packageResponse });
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, new { success = false, error = JsonConvert.DeserializeObject(errorResponse) });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }


        [HttpGet]
        [Route("api/GetPackingSlip")]
        public async Task<IActionResult> GetPackingSlip(string wbns, bool pdf = true)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {
                if (string.IsNullOrEmpty(wbns))
                {
                    return BadRequest(new { message = "Waybill number (wbns) is required" });
                }

                string endpoint = $"https://track.delhivery.com/api/p/packing_slip?wbns={wbns}&pdf=true";

                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    PackagesResponse _packages = JsonConvert.DeserializeObject<PackagesResponse>(responseBody);

                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _packages;
                    return Ok(apiResult);
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    PackagesResponse _packages = JsonConvert.DeserializeObject<PackagesResponse>(errorResponse);

                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = _packages;
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = ex.Message;
                return Ok(apiResult);
            }
        }


        [HttpPost]
        [Route("api/CreatePickupRequest")]
        public async Task<IActionResult> CreatePickupRequest([FromBody] PickupRequests pickupRequest)
        {
            try
            {
                if (pickupRequest == null)
                {
                    return BadRequest(new { message = "Invalid request payload" });
                }

                // Serialize the request data into JSON
                var requestContent = new StringContent(JsonConvert.SerializeObject(pickupRequest), Encoding.UTF8, "application/json");

                // Set headers for the request
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                // Send the POST request to Delhivery API
                HttpResponseMessage response = await _httpClient.PostAsync(_BaseUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var pickupResponse = JsonConvert.DeserializeObject(responseBody);

                    return Ok(new { success = true, data = pickupResponse });
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, new { success = false, error = JsonConvert.DeserializeObject(errorResponse) });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }


        [HttpPost]
        [Route("api/UpdatePackageStatus")]
        public async Task<IActionResult> UpdatePackageStatus([FromBody] PackageUpdateRequest packageUpdateRequest)
        {
            try
            {
                if (packageUpdateRequest == null || packageUpdateRequest.Data == null || packageUpdateRequest.Data.Count == 0)
                {
                    return BadRequest(new { message = "Invalid request payload" });
                }

                // Serialize the request data into JSON
                var requestContent = new StringContent(JsonConvert.SerializeObject(packageUpdateRequest), Encoding.UTF8, "application/json");

                // Set headers for the request
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_ApiKey}");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                // Send the POST request to Delhivery API
                HttpResponseMessage response = await _httpClient.PostAsync(_BaseUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var updateResponse = JsonConvert.DeserializeObject(responseBody);

                    return Ok(new { success = true, data = updateResponse });
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, new { success = false, error = JsonConvert.DeserializeObject(errorResponse) });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error occurred: {ex.Message}" });
            }
        }
    }
}


