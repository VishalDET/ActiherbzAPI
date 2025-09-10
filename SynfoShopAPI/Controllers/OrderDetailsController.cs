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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrderDetailsController(IConfiguration config)
        {
            _config = config;
        }
        [Route("api/GetOrderDetails")]
        [HttpPost]
        public IActionResult GetOrderDetails(int orderId)
        {
            try
            {
                List<OrderDetails> orderDetailsList = new List<OrderDetails>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getorderdetails", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_OrderId", orderId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderDetails orderDetail = new OrderDetails
                            {
                                //Id = Convert.ToInt32(reader["Id"]),
                                //OrderId = Convert.ToInt32(reader["OrderId"]),
                                //FranchiseId = Convert.ToInt32(reader["FranchiseId"]),
                                //PackageId = Convert.ToInt32(reader["PackageId"]),
                                //Quantity = Convert.ToInt32(reader["Quantity"]),
                                //Price = Convert.ToDouble(reader["Price"]),
                                //SalePrice = Convert.ToDouble(reader["SalePrice"]),
                                //TotalPrice = Convert.ToDouble(reader["TotalPrice"])



                                Id = Convert.ToInt32(reader["Id"]),
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                FranchiseId = Convert.ToInt32(reader["FranchiseId"]),
                                PackageId = Convert.ToInt32(reader["PackageId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDouble(reader["Price"]),
                                SalePrice = Convert.ToDouble(reader["SalePrice"]),
                                TotalPrice = Convert.ToDouble(reader["TotalPrice"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                MainImage = reader["MainImage"].ToString(),
                                BrandName = reader["BrandName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString()
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

        [Route("api/GetOrderDetailsForsms")]
        [HttpPost]
        public List<OrderDetails> GetOrderDetailsForsms(int orderId)
        {
            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getorderdetails", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_OrderId", orderId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderDetails orderDetail = new OrderDetails
                            {

                                Id = Convert.ToInt32(reader["Id"]),
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                FranchiseId = Convert.ToInt32(reader["FranchiseId"]),
                                PackageId = Convert.ToInt32(reader["PackageId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDouble(reader["Price"]),
                                SalePrice = Convert.ToDouble(reader["SalePrice"]),
                                TotalPrice = Convert.ToDouble(reader["TotalPrice"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                MainImage = reader["MainImage"].ToString(),
                                BrandName = reader["BrandName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString()
                            };

                            orderDetailsList.Add(orderDetail);
                        }
                    }
                }
                return orderDetailsList;
            }
            catch (Exception ex)
            {
                return orderDetailsList;
            }
        }
        [Route("api/CreateOrderDetails")]
        [HttpPost]
        public IActionResult CreateOrderDetails(OrderDetails orderDetails)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_createorderdetails", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_OrderId", orderDetails.OrderId);
                    command.Parameters.AddWithValue("_FranchiseId", orderDetails.FranchiseId);
                    command.Parameters.AddWithValue("_PackageId", orderDetails.PackageId);
                    command.Parameters.AddWithValue("_Quantity", orderDetails.Quantity);
                    command.Parameters.AddWithValue("_Price", orderDetails.Price);
                    command.Parameters.AddWithValue("_SalePrice", orderDetails.SalePrice);
                    command.Parameters.AddWithValue("_TotalPrice", orderDetails.TotalPrice);

                    command.ExecuteNonQuery();

                    return Ok("Order details created successfully.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating order details: {ex.Message}");
            }
        }
    }
}
