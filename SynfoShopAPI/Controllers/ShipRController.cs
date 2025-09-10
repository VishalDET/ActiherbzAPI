using MailKit.Search;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using SynfoShopAPI.DAL;
using SynfoShopAPI.Models;
using System;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Twilio.Http;
using Twilio.Jwt.AccessToken;
using static SynfoShopAPI.Models.ServiceRequestProcessor;
using static System.Net.WebRequestMethods;
using HttpClient = System.Net.Http.HttpClient;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SynfoShopAPI.Controllers
{

    [ApiController]
    public class ShipRController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://api.rocketbox.in/api";
        private readonly string EmailId = string.Empty;

        private readonly string Password = string.Empty;
        public ShipRController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            EmailId = _config["ShipRocket:emailId"];
            Password = _config["ShipRocket:Password"];
            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
        }

        [Route("api/GetRocketBoxToken")]
        [HttpPost]
        public async Task<string> GetRocketBoxToken()
        {
            string token = string.Empty;
            // Token to be refreshed
            // string refreshToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ0b2tlbl90eXBlIjoicmVmcmVzaCIsImV4cCI6MTcyNzk0NjUwNCwiaWF0IjoxNjk2NDEwNTA0LCJqdGkiOiI4NWM1Zjg0ZmQ5MzA0OTA4ODQ0MWNlZDU5ZmVjMDg0OCIsInVzZXJfaWQiOjE1Mzk2fQ.jPmLPCi6KsijaoQHMahpBInaWVQmaCt06y1Zqo30T3Y";
            string refreshToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ0b2tlbl90eXBlIjoiYWNjZXNzIiwiZXhwIjoxNzAyNTQyOTQ3LCJpYXQiOjE3MDI0NTQ5ODIsImp0aSI6IjE3ZTZiZjhjNzM1ODQ0NDk4YjgyY2EyYWFkMjcyZjg1IiwidXNlcl9pZCI6NTU3Mn0.e7QX7LJHjjO6xqeagWhZXuwi-uYzVc43qsOu10sWhOg";

            // Set the request headers
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + refreshToken);

            // Prepare the request data
            var requestData = new
            {
                refresh = refreshToken
            };

            // Convert the request data to JSON
            var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Send the POST request
            HttpResponseMessage response = await _httpClient.PostAsync($"{ApiBaseUrl}/token/refresh/", content);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read and display the response content
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<ShipRock>(responseBody);
                return token = responseObject.access;

            }
            else
            {
                return token;
            }
        }


        [Route("api/CreateRocketBoxOrder")]
        [HttpPost]
        public async Task<IActionResult> CreateRocketBoxOrder(RocketBoxShippingDetails order)
        {
            OrderController _orderC = new OrderController(_config, _hostEnvironment);
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getuseraddressbyorderId", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_OrderId", Utilities.validateInt(order.client_order_id));

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DestWareAddress _DestWareAddress = new DestWareAddress
                            {
                                Pincode = reader["Pincode"].ToString(),
                                AddressLine1 = reader["ShippingAddress"].ToString(),
                                AddressLine2 = "",
                                DestinationWarehouseName = reader["Fullname"].ToString(),
                                RecipientContactPersonName = reader["Fullname"].ToString(),
                                RecipientContactPersonEmail = reader["EmailId"].ToString(),
                                RecipientContactPersonContactNo = reader["MobileNo"].ToString(),
                                Country = reader["Country"].ToString(),
                                State = reader["State"].ToString(),
                                City = reader["City"].ToString(),
                                RecipientGST = reader["GSTNumber"].ToString()

                            };

                            order.destination_warehouse_name = _DestWareAddress.DestinationWarehouseName;
                            order.destination_address_line1 = _DestWareAddress.AddressLine1;
                            order.destination_address_line2 = _DestWareAddress.AddressLine2;
                            order.destination_pincode = _DestWareAddress.Pincode;
                            order.destination_city = _DestWareAddress.City;
                            order.destination_state = _DestWareAddress.State;
                            order.recipient_contact_person_name = _DestWareAddress.RecipientContactPersonName;
                            order.recipient_contact_person_email = _DestWareAddress.RecipientContactPersonEmail;
                            order.recipient_contact_person_contact_no = _DestWareAddress.RecipientContactPersonContactNo;
                            order.recipient_GST = _DestWareAddress.RecipientGST;
                            // order.
                        }

                    }
                }



                var requestData = new
                {
                    no_of_packages = 2,
                    approx_weight = "50.0",
                    is_insured = false,
                    is_to_pay = false,
                    to_pay_amount = (decimal?)null,
                    source_warehouse_name = "Chaitanya 1",
                    source_address_line1 = "Test 1",
                    source_address_line2 = "MALL ROAD null",
                    source_pincode = "110019",
                    source_city = "Delhi",
                    source_state = "Delhi",
                    sender_contact_person_name = "HEMANT TEDROS",
                    sender_contact_person_email = "sender_contact_person_email@gmail.com",
                    sender_contact_person_contact_no = "7777766660",
                    destination_warehouse_name = "Chetan 2",
                    destination_address_line1 = "Food Chain,24/2 Chikkahullhr village, k",
                    destination_address_line2 = "asaba hobli ,shidlagatta road, hosakote null",
                    destination_pincode = "110017",
                    destination_city = "Delhi",
                    destination_state = "Delhi",
                    recipient_contact_person_name = "Piyush Zalkey",
                    recipient_contact_person_email = "recipient_contact_person_email@gmail.com",
                    recipient_contact_person_contact_no = "6666677770",
                    client_id = 6488,
                    packaging_unit_details = new[]
                   {
                    new
                    {
                        units = 1,
                        weight = 25.0,
                        length = 1.0,
                        height = 1.0,
                        width = 1.0,
                        display_in = "cm"
                    },
                    new
                    {
                        units = 1,
                        weight = 25.0,
                        length = 1.0,
                        height = 1.0,
                        width = 1.0,
                        display_in = "cm"
                    }
                },
                    recipient_GST = (string)null,
                    volumetric_weight = "50.0",
                    supporting_docs = new string[] { },
                    shipment_type = "forward",
                    is_cod = false,
                    cod_amount = (decimal?)null,
                    mode_name = "surface",
                    source = "API",
                    client_order_id = "51BT230000EL"
                };
                string token = await GetRocketBoxToken();

                var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }

                // Add Authorization header with the new bearer token
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage response = await _httpClient.PostAsync($"{ApiBaseUrl}/external/order_creation/", content);

                // Prepare the request data



                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {


                    //Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    RocketBoxOrderResponse _rocketboxorderresponse = JsonConvert.DeserializeObject<RocketBoxOrderResponse>(responseBody);
                    using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("usp_insertshiporder", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_OrderData", jsonRequest);
                        command.Parameters.AddWithValue("_OrderId", Utilities.validateInt(order.client_order_id));
                        command.Parameters.AddWithValue("_Success", _rocketboxorderresponse.success);
                        command.Parameters.AddWithValue("_OrderIdResponse", _rocketboxorderresponse.order_id);
                        command.Parameters.AddWithValue("_FromWarehouseId", _rocketboxorderresponse.from_warehouse_id);
                        command.Parameters.AddWithValue("_ToWarehouseId", _rocketboxorderresponse.to_warehouse_id);
                        command.Parameters.AddWithValue("_Mode", _rocketboxorderresponse.mode);
                        command.Parameters.AddWithValue("_ModeId", _rocketboxorderresponse.mode_id);
                        command.Parameters.AddWithValue("_DeliveryPartnerName", _rocketboxorderresponse.delivery_partner_name);
                        command.Parameters.AddWithValue("_DeliveryPartnerId", _rocketboxorderresponse.delivery_partner_id);
                        command.Parameters.AddWithValue("_TransportarId", _rocketboxorderresponse.transportar_id);

                        command.ExecuteNonQuery();
                    }

                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _rocketboxorderresponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = null;
                    apiResult.message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = null;
                apiResult.message = $"Exception: {ex.Message}";
                return Ok(apiResult);
            }
        }

        [Route("api/ShipmentCreationRocketBox")]
        [HttpPost]
        public async Task<IActionResult> ShipmentCreationRocketBox(ShipmentOrderRequest _shipmentOrderRequest)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {
                var payload = new
                {
                    client_id = 6488,
                    order_id = 213246,
                    remarks = "Order remark 1048",
                    recipient_GST = (object)null,
                    to_pay_amount = "0",
                    mode_id = 16,
                    delivery_partner_id = 11,
                    pickup_date_time = "2023-12-12 19:07:55",
                    eway_bill_no = "361009176397",
                    invoice_value = 6000.0,
                    invoice_number = "51BT230000EK",
                    invoice_date = "2023-12-12",
                    supporting_docs = (object)null

                };

                string token = await GetRocketBoxToken();

                var jsonRequest = JsonConvert.SerializeObject(_shipmentOrderRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }

                // Add Authorization header with the new bearer token
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage response = await _httpClient.PostAsync($"{ApiBaseUrl}/order_shipment_association/", content);

                if (response.IsSuccessStatusCode)
                {
                    //Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    DeliveryResponse _rocketboxorderresponse = JsonConvert.DeserializeObject<DeliveryResponse>(responseBody);

                    using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("usp_insertshipment", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_ShipData", jsonRequest);//ShipData LONGTEXT, _ResponseData LONGTEXT, _OrderId
                        command.Parameters.AddWithValue("_ResponseData", responseBody );
                        command.Parameters.AddWithValue("_OrderId", _rocketboxorderresponse.order_id);

                        command.ExecuteNonQuery();
                    }
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _rocketboxorderresponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = null;
                    apiResult.message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = null;
                apiResult.message = $"Exception: {ex.Message}";
                return Ok(apiResult);
            }
        }

        [Route("api/GetRocketBoxShipmentDetails")]
        [HttpGet]
        public async Task<IActionResult> GetRocketBoxShipmentDetails(int ShipmentId)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;

            try
            {
                string token = await GetRocketBoxToken();
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiBaseUrl}/external/get_shipment/" + ShipmentId + "/");

                if (response.IsSuccessStatusCode)
                {
                    //Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    RocketBoxShipmentDetailsRes _rocketboxorderresponse = JsonConvert.DeserializeObject<RocketBoxShipmentDetailsRes>(responseBody);
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _rocketboxorderresponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = null;
                    apiResult.message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return Ok(apiResult);

                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = null;
                apiResult.message = $"Exception: {ex.Message}";
                return Ok(apiResult);
            }
        }


        [Route("api/GetRocketBoxOrderDetails")]
        [HttpGet]
        public async Task<IActionResult> GetRocketBoxOrderDetails(int OrderId)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;

            try
            {
                string token = await GetRocketBoxToken();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiBaseUrl}external/get_order/" + OrderId + "/");

                if (response.IsSuccessStatusCode)
                {
                    //Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    RocketBoxOrderDetails _rocketboxorderresponse = JsonConvert.DeserializeObject<RocketBoxOrderDetails>(responseBody);
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _rocketboxorderresponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = null;
                    apiResult.message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return Ok(apiResult);

                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = null;
                apiResult.message = $"Exception: {ex.Message}";
                return Ok(apiResult);
            }
        }


        [Route("api/ShipmentChargesCalculatorRocketBox")]
        [HttpPost]
        public async Task<IActionResult> ShipmentChargesCalculator(RocketBoxShipmentDetailsRequest _rocketBoxShipmentDetailsRequest)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {
                var payload = new
                {
                    from_pincode = "400076",
                    from_city = "Mumbai",
                    from_state = "Maharashtra",
                    to_pincode = "110017",
                    to_city = "New Delhi",
                    to_state = "Delhi",
                    quantity = 2,
                    invoice_value = 1111,
                    calculator_page = "true",
                    packaging_unit_details = new[]
                    {
                        new
                        {
                            units = 2,
                            length = 11,
                            height = 11,
                            weight = 12,
                            width = 11,
                            unit = "cm"
                        }
                    }
                };

                string token = await GetRocketBoxToken();

                var jsonRequest = JsonConvert.SerializeObject(_rocketBoxShipmentDetailsRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync($"{ApiBaseUrl}/order_shipment_association/", content);

                if (response.IsSuccessStatusCode)
                {
                    //Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    RBAdvantagesurface _rocketboxorderresponse = JsonConvert.DeserializeObject<RBAdvantagesurface>(responseBody);
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _rocketboxorderresponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = null;
                    apiResult.message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = null;
                apiResult.message = $"Exception: {ex.Message}";
                return Ok(apiResult);
            }
        }

        [Route("api/WarehouseCreationRocketBox")]
        [HttpPost]
        public async Task<IActionResult> WarehouseCreationRocketBox(WarehouseRequest _warehouserequest)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {
                WarehouseRequest warehouseRequest = new WarehouseRequest
                {
                    Name = "Big Yellow Basket",
                    ClientId = 6488,
                    Address = new WareAddress
                    {
                        AddressLine1 = "Demo address 1",
                        AddressLine2 = "Demo address 2",
                        Pincode = "452003",
                        City = "Indore",
                        State = "Madhya Pradesh",
                        Country = "India"
                    },
                    WarehouseCode = "dl222",
                    ContactPersonName = "person demo demo",
                    ContactPersonEmail = "contact_person_email@gmail.com",
                    ContactPersonContactNo = "7777766660"
                };
                string token = await GetRocketBoxToken();

                var jsonRequest = JsonConvert.SerializeObject(warehouseRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // _httpClient.DefaultRequestHeaders.Add("accept", "application/json, text/plain, */*");
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage response = await _httpClient.PostAsync($"{ApiBaseUrl}/warehouses/", content);

                if (response.IsSuccessStatusCode)
                {
                    //Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    WarehouseCreationResponse _rocketboxorderresponse = JsonConvert.DeserializeObject<WarehouseCreationResponse>(responseBody);
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _rocketboxorderresponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = null;
                    apiResult.message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = null;
                apiResult.message = $"Exception: {ex.Message}";
                return Ok(apiResult);
            }
        }

        [Route("api/GetWarehousesRocketBox")]
        [HttpGet]
        public async Task<IActionResult> GetWarehousesRocketBox(int page)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;

            try
            {
                string token = await GetRocketBoxToken();

                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }

                // Add Authorization header with the new bearer token
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiBaseUrl}/warehouses/?page=" + page + "");

                if (response.IsSuccessStatusCode)
                {
                    //Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    WarehouseList _rocketboxorderresponse = JsonConvert.DeserializeObject<WarehouseList>(responseBody);
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _rocketboxorderresponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = null;
                    apiResult.message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return Ok(apiResult);

                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = null;
                apiResult.message = $"Exception: {ex.Message}";
                return Ok(apiResult);
            }
        }

        [Route("api/WarehouseUpdationRockietBox")]
        [HttpPut]
        public async Task<IActionResult> WarehouseUpdationRockietBox(WarehouseUpdateRequest _warehouseupdaterequest)
        {
            ServiceRequestProcessor processor = new ServiceRequestProcessor();
            APIResult apiResult;
            try
            {
                WarehouseRequest warehouseRequest = new WarehouseRequest
                {
                    Name = _warehouseupdaterequest.name,
                    ClientId = _warehouseupdaterequest.client_id,
                    Address = new WareAddress
                    {
                        AddressLine1 = _warehouseupdaterequest.address.AddressLine1,
                        AddressLine2 = _warehouseupdaterequest.address.AddressLine2,
                        Pincode = _warehouseupdaterequest.address.Pincode,
                        City = _warehouseupdaterequest.address.City,
                        State = _warehouseupdaterequest.address.State,
                        Country = _warehouseupdaterequest.address.Country
                    },
                    WarehouseCode = _warehouseupdaterequest.warehouse_code,
                    ContactPersonName = _warehouseupdaterequest.contact_person_name,
                    ContactPersonEmail = _warehouseupdaterequest.contact_person_email,
                    ContactPersonContactNo = _warehouseupdaterequest.contact_person_contact_no
                };
                string token = await GetRocketBoxToken();

                var jsonRequest = JsonConvert.SerializeObject(warehouseRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiBaseUrl}/warehouses/" + _warehouseupdaterequest.WarehouseId + "/", content);

                if (response.IsSuccessStatusCode)
                {
                    //Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Warehouse _rocketboxorderresponse = JsonConvert.DeserializeObject<Warehouse>(responseBody);
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = _rocketboxorderresponse;
                    return Ok(apiResult);
                }
                else
                {
                    apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.InsertUpdateFailed, ServiceRequestProcessor.StatusCode.InsertUpdateFailed.ToString());
                    apiResult.data = null;
                    apiResult.message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.ServerError, ServiceRequestProcessor.StatusCode.ServerError.ToString());
                apiResult.data = null;
                apiResult.message = $"Exception: {ex.Message}";
                return Ok(apiResult);
            }
        }


        [Route("api/UpdateShipRocketPickUpAddress")]
        [HttpPatch]
        public async Task<string> PostPickupRequest(ShipRPickupRequestModel pickupRequest)
        {
            try
            {
                string token = await GetShipRocketTokenM();

                var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Patch, $"{ApiBaseUrl}orders/address/pickup");
                string jsonContent = JsonConvert.SerializeObject(pickupRequest);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions if any
                return $"Exception: {ex.Message}";
            }
        }



        [Route("api/GetShipRocketTokenM")]
        [HttpPost]
        public async Task<string> GetShipRocketTokenM()
        {
            ShipLoginRequest loginRequest = new ShipLoginRequest()
            {
                email = EmailId,
                password = Password
            };
            string token = string.Empty;
            string jsonRequest = JsonConvert.SerializeObject(loginRequest);

            //_httpClient.BaseAddress = new Uri("");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

            var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{ApiBaseUrl}auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                // Deserialize the response content and extract the authentication token
                var responseObject = JsonConvert.DeserializeObject<ShiprocketAuthResponse>(responseContent);
                return token = responseObject?.Token;
            }
            else
            {
                // Handle authentication failure
                return token;
            }
        }
    }

}


