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
    public class FranchiseOrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public FranchiseOrderController(IConfiguration config)
        {
            _config = config;
        }
        [Route("api/GetFranchiseOrder")]
        [HttpPost]
        public IActionResult GetFranchiseOrder(int franchiseId, int orderStatus)
        {
            try
            {
                List<Order> orderList = new List<Order>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getfranchiseorder", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_FranchiseId", franchiseId); 
                    command.Parameters.AddWithValue("_OrderStatus", orderStatus);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                FranchiseId = Convert.ToInt32(reader["FranchiseId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                ShippingAddress = reader["ShippingAddress"] == DBNull.Value ? string.Empty : reader["ShippingAddress"].ToString(),
                                CountryId = reader["CountryId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CountryId"]),
                                StateId = reader["StateId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StateId"]),
                                CityId = reader["CityId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CityId"]),
                                TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["TotalAmount"]),
                                DiscountAmount = reader["DiscountAmount"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountAmount"]),
                                GrandTotal = reader["GrandTotal"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["GrandTotal"]),
                                RewardRedeemed = reader["RewardRedeemed"] != DBNull.Value ? Convert.ToDouble(reader["RewardRedeemed"]) : 0.0,
                                RewardEarned = reader["RewardEarned"] != DBNull.Value ? Convert.ToDouble(reader["RewardEarned"]) : 0.0,
                                OrderDate = reader["OrderDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["OrderDate"]),
                                OrderStatus = Convert.ToInt32(reader["OrderStatus"]),
                                EmailId = reader["EmailId"].ToString(),
                                MobileNo = reader["MobileNo"].ToString(),
                                IncentiveEarned = reader["IncentiveEarned"] != DBNull.Value ? Convert.ToDouble(reader["IncentiveEarned"]) : 0.0,
                                RewardOnhold = reader["RewardOnhold"] != DBNull.Value ? Convert.ToDouble(reader["RewardOnhold"]) : 0.0,
                                PayStatus = reader["PayStatus"] != DBNull.Value ? Convert.ToInt32(reader["PayStatus"]) : 0,
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : DateTime.MinValue,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Convert.ToInt32(reader["CreatedBy"]) : 0,
                                ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(reader["ModifiedBy"]) : 0,
                                Referrer = reader["Referrer"] != DBNull.Value ? reader["Referrer"].ToString() : string.Empty,
                                Payment = reader["Payment"] != DBNull.Value ? Convert.ToInt32(reader["Payment"]) : 0,
                                PaymentAmount = reader["PaymentAmount"] != DBNull.Value ? Convert.ToDouble(reader["PaymentAmount"]) : 0.0,
                                PaymentTrans = reader["PaymentTrans"] != DBNull.Value ? reader["PaymentTrans"].ToString() : string.Empty,
                                PaymentType = reader["PaymentType"] != DBNull.Value ? reader["PaymentType"].ToString() : string.Empty,
                                IsRefund = reader["IsRefund"] != DBNull.Value && Convert.ToBoolean(reader["IsRefund"]),
                                RefundData = reader["RefundData"] != DBNull.Value ? reader["RefundData"].ToString() : string.Empty,
                                RefundAmount = reader["RefundAmount"] != DBNull.Value ? Convert.ToDouble(reader["RefundAmount"]) : 0.0,
                                DiscountCode = reader["DiscountCode"] != DBNull.Value ? reader["DiscountCode"].ToString() : string.Empty,
                                SelfPickup = reader["SelfPickup"] != DBNull.Value ? Convert.ToInt32(reader["SelfPickup"]) : 0,
                                PickupFrom = reader["PickupFrom"] != DBNull.Value ? reader["PickupFrom"].ToString() : string.Empty,

                            };

                            orderList.Add(order);
                        }
                    }
                }

                if (orderList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = orderList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Order>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        //OrderId, OrderDetailsId, MacId

        [Route("api/GetFranchiseOrderDetails")]
        [HttpPost]
        public IActionResult GetFranchiseOrderDetails(int orderId, int franchiseId)
        {
            try
            {
                List<OrderDetails> orderDetailsList = new List<OrderDetails>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getfranchiseorderdetails", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_OrderId", orderId);
                    command.Parameters.AddWithValue("_FranchiseId", franchiseId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderDetails orderDetail = new OrderDetails
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                FranchiseId = Convert.ToInt32(reader["FranchiseId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                MainImage = reader["MainImage"].ToString(),
                                BrandName = reader["BrandName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                PackageId = Convert.ToInt32(reader["PackageId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDouble(reader["Price"]),
                                SalePrice = Convert.ToDouble(reader["SalePrice"]),
                                TotalPrice = Convert.ToDouble(reader["TotalPrice"]),
                                MacId = reader["MacId"].ToString(),
                                CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate")
                            };

                            orderDetailsList.Add(orderDetail);
                        }
                    }
                }

                if (orderDetailsList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = orderDetailsList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<OrderDetails>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/GetFranchiseOrderDetailsWithMac")]
        [HttpPost]
        public IActionResult GetFranchiseOrderDetailsWithMac(int orderId, int franchiseId)
        {
            try
            {
                List<OrderDetailsMac> orderDetailsList = new List<OrderDetailsMac>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getfranchiseorderdetails", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_OrderId", orderId);
                    command.Parameters.AddWithValue("_FranchiseId", franchiseId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderDetailsMac orderDetail = new OrderDetailsMac
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                FranchiseId = Convert.ToInt32(reader["FranchiseId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                MainImage = reader["MainImage"].ToString(),
                                BrandName = reader["BrandName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                PackageId = Convert.ToInt32(reader["PackageId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDouble(reader["Price"]),
                                SalePrice = Convert.ToDouble(reader["SalePrice"]),
                                TotalPrice = Convert.ToDouble(reader["TotalPrice"]),
                                CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? DateTime.MinValue : reader.GetDateTime("CreatedDate"),
                                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? DateTime.MinValue : reader.GetDateTime("ModifiedDate"),
                                OrderMacIds = reader["MacId"].ToString()
                            };

                            orderDetailsList.Add(orderDetail);
                        }
                    }
                }

                if (orderDetailsList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = orderDetailsList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<OrderDetails>();
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
