using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Twilio.TwiML.Voice;

namespace SynfoShopAPI.Controllers
{

    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnvironment;

        public OrderController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            _hostEnvironment = hostEnvironment;
        }

        [Route("api/CreateOrder")]
        [HttpPost]
        public IActionResult CreateOrder(InsertOrder order)
        {
            SMSController smsC = new SMSController(_config);
            EmailerController EmailC = new EmailerController(_config, _hostEnvironment);

            List<InsertOrder> _order_ = new List<InsertOrder>();
            int orderId = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    string spName1 = order.IsBuyNow == 0 ? "usp_getcartbyfranchise" : "usp_getcartbyfranchisebuynow";
                    using (MySqlCommand command = new MySqlCommand(spName1, connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_UserId", order.UserId);
                        command.Parameters.AddWithValue("_ProductId", order.ProductId);
                        command.Parameters.AddWithValue("_PackageId", order.PackageId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InsertOrder order_ = new InsertOrder
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["TotalAmount"]),
                                    DiscountAmount = reader["DiscountAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["DiscountAmount"]),
                                    GrandTotal = reader["GrandTotal"] == DBNull.Value ? 0 : Convert.ToDouble(reader["GrandTotal"]),
                                    RewardRedeemed = 0,
                                    RewardEarned = 0,
                                    IncentiveEarned = 0,
                                    RewardOnhold = 0,
                                    PayStatus = 1,
                                    Referrer = "",
                                    Payment = 0,
                                    PaymentAmount = 0,
                                    PaymentTrans = "",
                                    PaymentType = order.PaymentType,
                                    DiscountCode = "",
                                    SelfPickup = 0,
                                    PickupFrom = "",
                                    FranchiseId = reader["FranchiseId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FranchiseId"]),
                                    ShippingAddress = order.ShippingAddress,
                                    State = order.State,
                                    City = order.City,
                                    Pincode = order.Pincode
                                };
                                _order_.Add(order_);
                            }

                        }
                    }
                    foreach (InsertOrder inorder in _order_)
                    {
                        string spName = order.IsBuyNow == 0 ? "usp_createorder" : "usp_createbuynoworder";
                        using (MySqlCommand command = new MySqlCommand(spName, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("_GroupId", inorder.Id);
                            command.Parameters.AddWithValue("_UserId", inorder.UserId);
                            command.Parameters.AddWithValue("_TotalAmount", inorder.TotalAmount);
                            command.Parameters.AddWithValue("_DiscountAmount", inorder.DiscountAmount);
                            command.Parameters.AddWithValue("_GrandTotal", inorder.GrandTotal);
                            command.Parameters.AddWithValue("_RewardRedeemed", inorder.RewardRedeemed);
                            command.Parameters.AddWithValue("_RewardEarned", inorder.RewardEarned);
                            command.Parameters.AddWithValue("_IncentiveEarned", inorder.IncentiveEarned);
                            command.Parameters.AddWithValue("_RewardOnhold", inorder.RewardOnhold);
                            command.Parameters.AddWithValue("_PayStatus", inorder.PayStatus);
                            command.Parameters.AddWithValue("_Referrer", inorder.Referrer);
                            command.Parameters.AddWithValue("_Payment", inorder.Payment);
                            command.Parameters.AddWithValue("_PaymentAmount", inorder.PaymentAmount);
                            command.Parameters.AddWithValue("_PaymentTrans", inorder.PaymentTrans);
                            command.Parameters.AddWithValue("_PaymentType", inorder.PaymentType);
                            command.Parameters.AddWithValue("_DiscountCode", inorder.DiscountCode);
                            command.Parameters.AddWithValue("_SelfPickup", inorder.SelfPickup);
                            command.Parameters.AddWithValue("_PickupFrom", inorder.PickupFrom);
                            command.Parameters.AddWithValue("_FranchiseId", inorder.FranchiseId);
                            command.Parameters.AddWithValue("_ShippingAddress", order.ShippingAddress);
                            command.Parameters.AddWithValue("_ProductId", order.ProductId);
                            command.Parameters.AddWithValue("_PackageId", order.PackageId);
                            command.Parameters.AddWithValue("_State", order.State);
                            command.Parameters.AddWithValue("_City", order.City);
                            command.Parameters.AddWithValue("_Pincode", order.Pincode);

                            command.Parameters.Add("_Result", MySqlDbType.Int32);
                            command.Parameters["_Result"].Direction = ParameterDirection.Output;

                            command.ExecuteNonQuery();

                            orderId = Convert.ToInt32(command.Parameters["_Result"].Value);
                            if (orderId > 0)
                            {
                                Order orders = new Order();
                                orders = GetOrderData(orderId, 0);
                                //smsC.UserOrderPlacedMSG(orders.MobileNo, orders.FirstName, orderId);
                                EmailC.SendOrderPlaced_Email(order.UserId, orderId);
                            }

                        }
                    }
                }
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = orderId;
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/CreateOrderWithBuyNow")]
        [HttpPost]
        public IActionResult CreateOrderWithNow(BuyNowOrder order)
        {
            SMSController smsC = new SMSController(_config);
            EmailerController EmailC = new EmailerController(_config, _hostEnvironment);

            List<BuyNowOrder> _order_ = new List<BuyNowOrder>();
            int orderId = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    using (MySqlCommand command = new MySqlCommand("usp_getcartbyfranchise", connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_UserId", order.UserId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BuyNowOrder order_ = new BuyNowOrder
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["TotalAmount"]),
                                    DiscountAmount = reader["DiscountAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["DiscountAmount"]),
                                    GrandTotal = reader["GrandTotal"] == DBNull.Value ? 0 : Convert.ToDouble(reader["GrandTotal"]),
                                    RewardRedeemed = 0,
                                    RewardEarned = 0,
                                    IncentiveEarned = 0,
                                    RewardOnhold = 0,
                                    PayStatus = 1,
                                    Referrer = "",
                                    Payment = 0,
                                    PaymentAmount = 0,
                                    PaymentTrans = "",
                                    PaymentType = "",
                                    DiscountCode = "",
                                    SelfPickup = 0,
                                    PickupFrom = "",
                                    FranchiseId = reader["FranchiseId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FranchiseId"]),
                                    ShippingAddress = order.ShippingAddress,
                                    State = order.State,
                                    City = order.City,
                                    Pincode = order.Pincode
                                };
                                _order_.Add(order_);
                            }

                        }
                    }
                    foreach (BuyNowOrder inorder in _order_)
                    {
                        using (MySqlCommand command = new MySqlCommand("usp_createorderwithbuynow", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("_GroupId", inorder.Id);
                            command.Parameters.AddWithValue("_UserId", inorder.UserId);
                            command.Parameters.AddWithValue("_TotalAmount", inorder.TotalAmount);
                            command.Parameters.AddWithValue("_DiscountAmount", inorder.DiscountAmount);
                            command.Parameters.AddWithValue("_GrandTotal", inorder.GrandTotal);
                            command.Parameters.AddWithValue("_RewardRedeemed", inorder.RewardRedeemed);
                            command.Parameters.AddWithValue("_RewardEarned", inorder.RewardEarned);
                            command.Parameters.AddWithValue("_IncentiveEarned", inorder.IncentiveEarned);
                            command.Parameters.AddWithValue("_RewardOnhold", inorder.RewardOnhold);
                            command.Parameters.AddWithValue("_PayStatus", inorder.PayStatus);
                            command.Parameters.AddWithValue("_Referrer", inorder.Referrer);
                            command.Parameters.AddWithValue("_Payment", inorder.Payment);
                            command.Parameters.AddWithValue("_PaymentAmount", inorder.PaymentAmount);
                            command.Parameters.AddWithValue("_PaymentTrans", inorder.PaymentTrans);
                            command.Parameters.AddWithValue("_PaymentType", inorder.PaymentType);
                            command.Parameters.AddWithValue("_DiscountCode", inorder.DiscountCode);
                            command.Parameters.AddWithValue("_SelfPickup", inorder.SelfPickup);
                            command.Parameters.AddWithValue("_PickupFrom", inorder.PickupFrom);
                            command.Parameters.AddWithValue("_FranchiseId", inorder.FranchiseId);
                            command.Parameters.AddWithValue("_ShippingAddress", order.ShippingAddress);
                            command.Parameters.AddWithValue("_ProductId", order.ProductId);
                            command.Parameters.AddWithValue("_PackageId", order.PackageId);
                            command.Parameters.AddWithValue("_State", order.State);
                            command.Parameters.AddWithValue("_City", order.City);
                            command.Parameters.AddWithValue("_Pincode", order.Pincode);

                            command.Parameters.Add("_Result", MySqlDbType.Int32);
                            command.Parameters["_Result"].Direction = ParameterDirection.Output;

                            command.ExecuteNonQuery();

                            orderId = Convert.ToInt32(command.Parameters["_Result"].Value);
                            if (orderId > 0)
                            {
                                Order orders = new Order();
                                orders = GetOrderData(orderId, 0);
                                //smsC.UserOrderPlacedMSG(orders.MobileNo, orders.FirstName, orderId);
                                EmailC.SendOrderPlaced_Email(order.UserId, orderId);
                            }

                        }
                    }
                }
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = orderId;
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/GetOrderData")]

        [HttpPost]
        public Order GetOrderData(int OrderId, int orderStatus)
        {
            Order order = null;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getorder", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_OrderId", OrderId);
                    command.Parameters.AddWithValue("_OrderStatus", orderStatus);
                    command.Parameters.AddWithValue("_SalesUserId", 0);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order = new Order
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                ShippingAddress = reader["ShippingAddress"].ToString(),
                                CountryId = reader["CountryId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CountryId"]),
                                StateId = reader["StateId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StateId"]),
                                CityId = reader["CityId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CityId"]),
                                TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["TotalAmount"]),
                                DiscountAmount = reader["DiscountAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["DiscountAmount"]),
                                GrandTotal = reader["GrandTotal"] == DBNull.Value ? 0 : Convert.ToDouble(reader["GrandTotal"]),
                                RewardRedeemed = reader["RewardRedeemed"] == DBNull.Value ? 0 : Convert.ToDouble(reader["RewardRedeemed"]),
                                RewardEarned = reader["RewardEarned"] == DBNull.Value ? 0 : Convert.ToDouble(reader["RewardEarned"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                OrderStatus = reader["OrderStatus"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OrderStatus"]),
                                EmailId = reader["EmailId"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                IncentiveEarned = reader["IncentiveEarned"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["IncentiveEarned"]),
                                RewardOnhold = reader["RewardOnhold"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["RewardOnhold"]),
                                PayStatus = reader["PayStatus"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PayStatus"]),
                                CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDate"]),
                                ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedDate"]),
                                CreatedBy = reader["CreatedBy"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CreatedBy"]),
                                ModifiedBy = reader["ModifiedBy"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ModifiedBy"]),
                                Referrer = reader["Referrer"]?.ToString(),
                                Payment = reader["Payment"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Payment"]),
                                PaymentAmount = reader["PaymentAmount"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["PaymentAmount"]),
                                PaymentTrans = reader["PaymentTrans"]?.ToString(),
                                PaymentType = reader["PaymentType"]?.ToString(),
                                IsRefund = reader["IsRefund"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsRefund"]),
                                RefundData = reader["RefundData"]?.ToString(),
                                RefundAmount = reader["RefundAmount"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["RefundAmount"]),
                                DiscountCode = reader["DiscountCode"]?.ToString(),
                                SelfPickup = reader["SelfPickup"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SelfPickup"]),
                                //City = reader["FranCitychiseName"].ToString(),
                                State = reader["State"].ToString(),
                                Pincode = reader["Pincode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Pincode"]),
                                PickupFrom = reader["PickupFrom"]?.ToString(),
                            };
                        }

                    }
                }
                return order;
            }
            catch (Exception ex)
            {
                return order;
            }
        }

        [HttpPost("api/GetOrders")]
        public IActionResult GetOrders([FromBody] OrderRequestModel request)
        {
            try
            {
                List<Order> orderList = new List<Order>();

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getorder", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_OrderId", request.OrderId);
                    command.Parameters.AddWithValue("_OrderStatus", request.OrderStatus);
                    command.Parameters.AddWithValue("_SalesUserId", request.SalesUserId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                SynfoInvoiceUrl = reader["SynfoInvoiceUrl"].ToString(),
                                ShippingAddress = reader["ShippingAddress"].ToString(),
                                CountryId = reader["CountryId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CountryId"]),
                                StateId = reader["StateId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StateId"]),
                                CityId = reader["CityId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CityId"]),
                                TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["TotalAmount"]),
                                DiscountAmount = reader["DiscountAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["DiscountAmount"]),
                                GrandTotal = reader["GrandTotal"] == DBNull.Value ? 0 : Convert.ToDouble(reader["GrandTotal"]),
                                RewardRedeemed = reader["RewardRedeemed"] == DBNull.Value ? 0 : Convert.ToDouble(reader["RewardRedeemed"]),
                                RewardEarned = reader["RewardEarned"] == DBNull.Value ? 0 : Convert.ToDouble(reader["RewardEarned"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                OrderStatus = reader["OrderStatus"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OrderStatus"]),
                                EmailId = reader["EmailId"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                GSTAmount = reader["GSTAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["GSTAmount"]),
                                IncentiveEarned = reader["IncentiveEarned"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["IncentiveEarned"]),
                                RewardOnhold = reader["RewardOnhold"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["RewardOnhold"]),
                                PayStatus = reader["PayStatus"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PayStatus"]),
                                CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDate"]),
                                ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ModifiedDate"]),
                                CreatedBy = reader["CreatedBy"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CreatedBy"]),
                                ModifiedBy = reader["ModifiedBy"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ModifiedBy"]),
                                Referrer = reader["Referrer"]?.ToString(),
                                Payment = reader["Payment"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Payment"]),
                                PaymentAmount = reader["PaymentAmount"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["PaymentAmount"]),
                                PaymentTrans = reader["PaymentTrans"]?.ToString(),
                                PaymentType = reader["PaymentType"]?.ToString(),
                                IsRefund = reader["IsRefund"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsRefund"]),
                                RefundData = reader["RefundData"]?.ToString(),
                                RefundAmount = reader["RefundAmount"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["RefundAmount"]),
                                DiscountCode = reader["DiscountCode"]?.ToString(),
                                SelfPickup = reader["SelfPickup"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SelfPickup"]),
                                PickupFrom = reader["PickupFrom"]?.ToString(),
                                SalesUserId = reader["SalesUserId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SalesUserId"]),
                                SalesUserName = reader["FullName"].ToString(),
                                FranchiseName = reader["FranchiseName"].ToString(),
                                City = reader["City"].ToString(),
                                State = reader["State"].ToString(),
                                Ewaybill = reader["Ewaybill"].ToString(),
                                Pincode = reader["Pincode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Pincode"]),
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

        [Route("api/getCustomerOrderApp")]
        [HttpGet]
        public IActionResult getCustomerOrderApp(int UserId)
        {
            try
            {
                List<CustomerOrderView> OrdersList = new List<CustomerOrderView>();
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("usp_getcustomerorderApp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_UserId", UserId);


                        DataSet ds = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(command);
                        da.Fill(ds);

                        DataTable dtOrder = new DataTable();
                        dtOrder = ds.Tables[0];

                        DataTable dtOrderDetails = new DataTable();
                        dtOrderDetails = ds.Tables[1];

                        foreach (DataRow dr in dtOrder.Rows)
                        {
                            List<List<CustomerOrderDetailsView>> franchiseOrderDetailLists = new List<List<CustomerOrderDetailsView>>();

                            if (dtOrderDetails.AsEnumerable().Where(p => p["OrderId"].ToString() == dr["Id"].ToString()).AsDataView().Count > 0)
                            {
                                DataTable _dtNew = new DataTable();
                                _dtNew = dtOrderDetails.AsEnumerable().Where(p => p["OrderId"].ToString() == dr["Id"].ToString()).CopyToDataTable();
                                List<CustomerOrderDetailsView> _orderDetailsList = new List<CustomerOrderDetailsView>();

                                foreach (DataRow dr1 in _dtNew.Rows)
                                {



                                    var orderDetailsModel = new CustomerOrderDetailsView
                                    {
                                        OrderId = dr1.IsNull("OrderId") ? 0 : Convert.ToInt32(dr1["OrderId"]),
                                        UserId = dr1.IsNull("UserId") ? 0 : Convert.ToInt32(dr1["UserId"]),
                                        FranchiseId = dr1.IsNull("FranchiseId") ? 0 : Convert.ToInt32(dr1["FranchiseId"]),
                                        ProductTitle = dr1.IsNull("ProductTitle") ? null : (dr1["ProductTitle"].ToString()),
                                        MainImage = dr1.IsNull("MainImage") ? null : (dr1["MainImage"].ToString()),
                                        Quantity = dr1.IsNull("Quantity") ? 0 : Convert.ToInt32(dr1["Quantity"]),
                                        InvoiceNumber = dr1.IsNull("InvoiceNumber") ? null : (dr1["InvoiceNumber"].ToString()),
                                        InvoiceNumberUrl = dr1.IsNull("InvoiceNumberUrl") ? null : (dr1["InvoiceNumberUrl"].ToString()),
                                        EwayNumber = dr1.IsNull("EwayNumber") ? null : (dr1["EwayNumber"].ToString()),
                                        EwayNumberUrl = dr1.IsNull("EwayNumberUrl") ? null : (dr1["EwayNumberUrl"].ToString()),
                                        LRNumber = dr1.IsNull("LRNumber") ? null : (dr1["LRNumber"].ToString()),
                                        LRNumberUrl = dr1.IsNull("LRNumberUrl") ? null : (dr1["LRNumberUrl"].ToString()),

                                    };
                                    _orderDetailsList.Add(orderDetailsModel);


                                }
                                // OrderDetailRepository repository = new OrderDetailRepository();

                                List<CustomerOrderDetailsView> orderDetailsList = _orderDetailsList.ToList();

                                var groupedOrderDetails = orderDetailsList.GroupBy(o => o.FranchiseId);


                                foreach (var group in groupedOrderDetails)
                                {
                                    var franchiseOrderDetails = group.ToList();
                                    franchiseOrderDetailLists.Add(franchiseOrderDetails);
                                }
                            }

                            var orderModel = new CustomerOrderView
                            {
                                Id = dr.IsNull("Id") ? 0 : Convert.ToInt32(dr["Id"]),
                                UserId = dr.IsNull("UserId") ? 0 : Convert.ToInt32(dr["UserId"]),
                                City = dr.IsNull("City") ? null : (dr["City"].ToString()),
                                OrderStatus = dr.IsNull("OrderStatus") ? null : (dr["OrderStatus"].ToString()),
                                GroupId = dr.IsNull("GroupId") ? null : (dr["GroupId"].ToString()),
                                OrderDate = dr.IsNull("OrderDate") ? null : (dr["OrderDate"].ToString()),
                                ETA = dr.IsNull("ETA") ? null : (dr["ETA"].ToString()),
                                MacIds = dr.IsNull("MacIds") ? null : (dr["MacIds"].ToString()),
                                InvoiceNo = dr.IsNull("InvoiceNo") ? null : (dr["InvoiceNo"].ToString()),
                                GrandTotal = dr.IsNull("GrandTotal") ? 0.0 : Convert.ToDouble(dr["GrandTotal"]),
                                OrderDetails = franchiseOrderDetailLists.ToList()
                            };

                            OrdersList.Add(orderModel);
                        }

                        ServiceRequestProcessor processor = new ServiceRequestProcessor();
                        APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                        apiResult.data = OrdersList;
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

        [Route("api/getCustomerOrderDetailsApp")]
        [HttpGet]
        public IActionResult getCustomerOrderDetailsApp(int OrderId)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                CustomerDOrderView OrdersList = new CustomerDOrderView();
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("usp_getcustomerorderDetailsApp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_OrderId", OrderId);


                        DataSet ds = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(command);
                        da.Fill(ds);

                        DataTable dtOrder = new DataTable();
                        dtOrder = ds.Tables[0];

                        DataTable dtOrderDetails = new DataTable();
                        dtOrderDetails = ds.Tables[1];


                        List<CustomerDOrderDetailsView> _orderDetailsList = new List<CustomerDOrderDetailsView>();

                        foreach (DataRow dr1 in dtOrderDetails.Rows)
                        {
                            var orderDetailsModel = new CustomerDOrderDetailsView
                            {
                                ProductTitle = dr1.IsNull("ProductTitle") ? null : dr1["ProductTitle"].ToString(),
                                MainImage = dr1.IsNull("MainImage") ? null : (dr1["MainImage"].ToString()),
                                Quantity = dr1.IsNull("Quantity") ? 0 : Convert.ToInt32(dr1["Quantity"]),
                                TSalePrice = dr1.IsNull("SalePrice") ? 0 : Convert.ToDouble(dr1["SalePrice"]),
                                TPrice = dr1.IsNull("Price") ? 0 : Convert.ToDouble(dr1["Price"]),

                                IsPlaced = dr1.IsNull("IsPlaced") ? 0 : Convert.ToInt32(dr1["IsPlaced"]),
                                PlacedDate = dr1.IsNull("PlacedDate") ? null : (dr1["PlacedDate"].ToString()),
                                IsConfirmed = dr1.IsNull("IsConfirmed") ? 0 : Convert.ToInt32(dr1["IsConfirmed"]),
                                ConfirmedDate = dr1.IsNull("ConfirmedDate") ? null : (dr1["ConfirmedDate"].ToString()),
                                IsProcessing = dr1.IsNull("IsProcessing") ? 0 : Convert.ToInt32(dr1["IsProcessing"]),
                                ProcessedDate = dr1.IsNull("ProcessedDate") ? null : (dr1["ProcessedDate"].ToString()),
                                IsDispatched = dr1.IsNull("IsDispatched") ? 0 : Convert.ToInt32(dr1["IsDispatched"]),
                                DispatchedDate = dr1.IsNull("DispatchedDate") ? null : (dr1["DispatchedDate"].ToString()),
                                IsDelivered = dr1.IsNull("IsDelivered") ? 0 : Convert.ToInt32(dr1["IsDelivered"]),
                                DeliveredDate = dr1.IsNull("DeliveredDate") ? null : (dr1["DeliveredDate"].ToString())
                            };
                            _orderDetailsList.Add(orderDetailsModel);
                        }

                        foreach (DataRow dr in dtOrder.Rows)
                        {
                            OrdersList = new CustomerDOrderView
                            {
                                Id = dr.IsNull("Id") ? 0 : Convert.ToInt32(dr["Id"]),
                                OrderStatus = dr.IsNull("OrderStatus") ? null : dr["OrderStatus"].ToString(),
                                OrderDate = dr.IsNull("OrderDate") ? null : dr["OrderDate"].ToString(),
                                DispatchedDate = dr.IsNull("DispatchedDate") ? null : dr["DispatchedDate"].ToString(),
                                IsDelivered = dr.IsNull("IsDelivered") ? 0 : Convert.ToInt32(dr["IsDelivered"]),
                                OrderDetails = _orderDetailsList,
                                InvoiceNumberUrl = dtOrderDetails.Rows[0]["InvoiceNumberUrl"] != null ? dtOrderDetails.Rows[0]["InvoiceNumberUrl"].ToString() : null,
                                EwayNumberUrl = dtOrderDetails.Rows[0]["EwayNumberUrl"] != null ? dtOrderDetails.Rows[0]["EwayNumberUrl"].ToString() : null,
                                LRNumberUrl = dtOrderDetails.Rows[0]["LRNumberUrl"] != null ? dtOrderDetails.Rows[0]["LRNumberUrl"].ToString() : null
                            };
                        }
                        if (OrdersList != null)
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                            apiResult.data = OrdersList;
                            return Ok(apiResult);
                        }
                        else
                        {
                            apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                            apiResult.data = new { };
                            return NotFound(apiResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/UpdateOrderStatus")]
        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, int orderStatus, int modifiedBy)
        {
            EmailerController EmailC = new EmailerController(_config, _hostEnvironment);
            try
            {
                SMSController smsC = new SMSController(_config);
                int result = 0;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_updateorderstatus", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    //command.Parameters.AddWithValue("_UserId", userId);
                    command.Parameters.AddWithValue("_OrderId", orderId);
                    command.Parameters.AddWithValue("_OrderStatus", orderStatus);
                    command.Parameters.AddWithValue("_ModifiedBy", modifiedBy);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters["_Result"].Value);
                }

                if (result == 1)
                {

                    Order orders = new Order();
                    orders = GetOrderData(orderId, 0);
                    OrderDetailsController Od = new OrderDetailsController(_config);


                    if (orderStatus == 2)// Order Confirmed
                    {
                        List<OrderDetails> orderDetailsList = new List<OrderDetails>();
                        orderDetailsList = Od.GetOrderDetailsForsms(orderId);
                        string productDetails = string.Empty;

                        foreach (OrderDetails item in orderDetailsList)
                        {
                            if (productDetails == string.Empty)
                            {
                                productDetails = item.ProductTitle;
                            }
                            //else
                            //{
                            //    productDetails = productDetails + " and " + item.ProductTitle;
                            //}
                        }

                       // EmailC.SendOrderConfirmed_Email(orders.UserId, orderId, "Order Confirmed");
                    }
                    else if (orderStatus == 3) // Order Processed
                    {
                       // EmailC.SendOrderConfirmed_Email(orders.UserId, orderId, "Order Processed");
                    }
                    else if (orderStatus == 4) // Order Dispatched
                    {
                        //EmailC.SendOrderConfirmed_Email(orders.UserId, orderId, "Order Dispatched");
                    }
                    else if (orderStatus == 5) // Order Delivered
                    {
                        //EmailC.SendOrderConfirmed_Email(orders.UserId, orderId, "Order Delivered");
                    }
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UpdateOrderDetails")]
        [HttpPost]
        public IActionResult UpdateOrderDetails(UpdateOrderDetails updateorderdetails)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_updateorderdetails", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", updateorderdetails.id);
                    command.Parameters.AddWithValue("_OrderId", updateorderdetails.orderId);
                    command.Parameters.AddWithValue("_InvoiceNumber", updateorderdetails.invoiceNumber);
                    command.Parameters.AddWithValue("_TransportNumber", updateorderdetails.TransportNumber);
                    command.Parameters.AddWithValue("_EwayNumber", updateorderdetails.ewayNumber);
                    command.Parameters.AddWithValue("_EwayNumberUrl", updateorderdetails.ewayNumberUrl);
                    command.Parameters.AddWithValue("_LRNumber", updateorderdetails.lrNumber);
                    command.Parameters.AddWithValue("_LRNumberUrl", updateorderdetails.lrNumberUrl);

                    command.ExecuteNonQuery();
                }

                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UpdateOrderDetailsStatus")]
        [HttpPost]
        public IActionResult UpdateOrderDetailsStatus(int id, int orderId, int orderStatus)
        {
            try
            {
                int result = 0;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_updateorderdetailsstatus", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", id);
                    command.Parameters.AddWithValue("_OrderId", orderId);
                    command.Parameters.AddWithValue("_OrderStatus", orderStatus);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();

                    result = Convert.ToInt32(command.Parameters["_Result"].Value);
                }

                if (result == 1)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new { result = result };
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }
        [Route("api/CreateOrderIntermediate")]
        [HttpPost]
        public IActionResult CreateOrderIntermediate(InsertOrderIntermediateRequest request)
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    using (MySqlCommand command = new MySqlCommand("InsertOrderIntermediate", connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("p_OrderData", request.OrderData);
                        command.Parameters.AddWithValue("p_OrderDetailsData", request.OrderDetailsData);
                        command.Parameters.AddWithValue("p_UserId", request.UserId);
                        command.ExecuteNonQuery();
                    }
                }
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = 1;
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


    }
}
