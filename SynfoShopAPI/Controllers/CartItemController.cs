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
    public class CartItemController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CartItemController(IConfiguration config)
        {
            _config = config;
        }


        [HttpPost]
        [Route("api/AddToCart")]
        public IActionResult AddToCart(AddCartItem addcartitem)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_productaddtocart", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_IsApp", addcartitem.IsApp);
                    command.Parameters.AddWithValue("_UserId", addcartitem.UserId);
                    command.Parameters.AddWithValue("_ProductId", addcartitem.ProductId);
                    command.Parameters.AddWithValue("_Quantity", addcartitem.Quantity);
                    command.Parameters.AddWithValue("_PackageId", addcartitem.PackageId);
                    command.Parameters.AddWithValue("_Price", addcartitem.Price);
                    command.Parameters.AddWithValue("_SalePrice", addcartitem.SalePrice);
                    command.Parameters.AddWithValue("_Color", addcartitem.Color);
                    command.Parameters.AddWithValue("_Size", addcartitem.Size);

                    command.ExecuteNonQuery();
                }

                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge(200, "Item added to cart successfully.");
                apiResult.data = addcartitem;
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [HttpPost]
        [Route("api/GetCartProduct")]
        public IActionResult GetCartProduct(int UserId)
        {
            try
            {
                List<CartProduct> cartProductlist = new List<CartProduct>();
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getcartproduct", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_UserId", UserId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartProduct cartProduct = new CartProduct
                            {
                                ItemType = reader["item"].ToString(),
                                MainImage = reader["MainImage"].ToString(),
                                Id = Convert.ToInt32(reader["Id"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                IsApp = Convert.ToInt32(reader["IsApp"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Units = Convert.ToInt32(reader["Units"]),
                                Quantity = Convert.ToDouble(reader["Quantity"]),
                                PackageId = reader["PackageId"] != DBNull.Value ? Convert.ToInt32(reader["PackageId"]) : 0,
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDouble(reader["Price"]) : 0.0,
                                DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(reader["DiscountPercent"]) : 0.0,
                                DiscountPrice = reader["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(reader["DiscountAmount"]) : 0.0,
                                SalePrice = reader["SalePrice"] != DBNull.Value ? Convert.ToDouble(reader["SalePrice"]) : 0.0,
                                TotalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToDouble(reader["TotalPrice"]) : 0.0,
                                GSTAmount = reader["GSTAmount"] != DBNull.Value ? Convert.ToDouble(reader["GSTAmount"]) : 0.0,
                                ConveniencePer = reader["ConveniencePer"] != DBNull.Value ? Convert.ToDouble(reader["ConveniencePer"]) : 0.0,
                                ConvenienceFee = reader["ConvenienceFee"] != DBNull.Value ? Convert.ToDouble(reader["ConvenienceFee"]) : 0.0,
                                GrandTotalWithoutShipping = reader["GrandTotalWithoutShipping"] != DBNull.Value ? Convert.ToDouble(reader["GrandTotalWithoutShipping"]) : 0.0,
                                ShippingCharges = reader["ShippingCharges"] != DBNull.Value ? Convert.ToDouble(reader["ShippingCharges"]) : 0.0,
                                GrandTotal = reader["GrandTotal"] != DBNull.Value ? Convert.ToDouble(reader["GrandTotal"]) : 0.0,
                                
                                DeliveryDate = reader["DeliveryDate"].ToString(),
                                Size = reader["Size"].ToString(),
                                Color = reader["Color"].ToString(),
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue
                               
                            };
                            cartProductlist.Add(cartProduct);
                        }
                    }
                }
                if (cartProductlist.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = cartProductlist;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Brand>();
                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult errorResult = (APIResult)processor.onError(ex.Message);

                return BadRequest(errorResult);
            }
        }


        [HttpPost]
        [Route("api/RemoveCartProduct")]

        public IActionResult RemoveCartProduct(RemoveCart removecart)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_removecartproduct", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_UserId", removecart.UserId);
                    command.Parameters.AddWithValue("_ProductId", removecart.ProductId);
                    command.Parameters.AddWithValue("_PackageId", removecart.PackageId);

                    command.ExecuteNonQuery();
                }

                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge(200, "Product removed from cart successfully.");
                apiResult.data = removecart;
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult errorResult = (APIResult)processor.onError(ex.Message);

                return BadRequest(errorResult);
            }
        }


        /////////////////////////////////////////////////// Wishlist ///////////////////////////////////////////////////////

        [HttpPost]
        [Route("api/AddToWishlist")]
        public IActionResult AddToWishlist(AddCartItem addcartitem)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_addtowishlist", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_IsApp", addcartitem.IsApp);
                    command.Parameters.AddWithValue("_UserId", addcartitem.UserId);
                    command.Parameters.AddWithValue("_ProductId", addcartitem.ProductId);
                    command.Parameters.AddWithValue("_Quantity", addcartitem.Quantity);
                    command.Parameters.AddWithValue("_PackageId", addcartitem.PackageId);
                    command.Parameters.AddWithValue("_Price", addcartitem.Price);
                    command.Parameters.AddWithValue("_SalePrice", addcartitem.SalePrice);
                    command.Parameters.AddWithValue("_Color", addcartitem.Color);
                    command.Parameters.AddWithValue("_Size", addcartitem.Size);
                    command.ExecuteNonQuery();
                }

                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge(200, "Item added to wishlist successfully.");
                apiResult.data = addcartitem;
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }



        [HttpPost]
        [Route("api/GetWishlistProduct")]
        public IActionResult GetWishlistProduct(int UserId)
        {
            try
            {
                List<CartProduct> cartProductlist = new List<CartProduct>();
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getwishlistproduct", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_UserId", UserId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartProduct cartProduct = new CartProduct
                            {
                                ItemType = reader["item"].ToString(),
                                MainImage = reader["MainImage"].ToString(),
                                Id = Convert.ToInt32(reader["Id"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                IsApp = Convert.ToInt32(reader["IsApp"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Units = Convert.ToInt32(reader["Units"]),
                                Quantity = Convert.ToDouble(reader["Quantity"]),
                                PackageId = reader["PackageId"] != DBNull.Value ? Convert.ToInt32(reader["PackageId"]) : 0,
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDouble(reader["Price"]) : 0.0,
                                DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(reader["DiscountPercent"]) : 0.0,
                                DiscountPrice = reader["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(reader["DiscountAmount"]) : 0.0,
                                SalePrice = reader["SalePrice"] != DBNull.Value ? Convert.ToDouble(reader["SalePrice"]) : 0.0,
                                TotalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToDouble(reader["TotalPrice"]) : 0.0,
                                GSTAmount = reader["GSTAmount"] != DBNull.Value ? Convert.ToDouble(reader["GSTAmount"]) : 0.0,
                                ConveniencePer = reader["ConveniencePer"] != DBNull.Value ? Convert.ToDouble(reader["ConveniencePer"]) : 0.0,
                                ConvenienceFee = reader["ConvenienceFee"] != DBNull.Value ? Convert.ToDouble(reader["ConvenienceFee"]) : 0.0,
                                GrandTotalWithoutShipping = reader["GrandTotalWithoutShipping"] != DBNull.Value ? Convert.ToDouble(reader["GrandTotalWithoutShipping"]) : 0.0,
                                GrandTotal = reader["GrandTotal"] != DBNull.Value ? Convert.ToDouble(reader["GrandTotal"]) : 0.0,
                                DeliveryDate = reader["DeliveryDate"].ToString(),
                                Size = reader["Size"].ToString(),
                                Color = reader["Color"].ToString(),
                                SizeId = Convert.ToInt32(reader["SizeId"]),
                                ColorId = Convert.ToInt32(reader["ColorId"]),
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue

                            };
                            cartProductlist.Add(cartProduct);
                        }
                    }
                }
                if (cartProductlist.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = cartProductlist;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Brand>();
                    return Ok(apiResult);
                }



            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult errorResult = (APIResult)processor.onError(ex.Message);

                return BadRequest(errorResult);
            }
        }


        [HttpPost]
        [Route("api/RemoveWishlistProduct")]
        public IActionResult RemoveWishlistProduct(RemoveCart removecart)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_removewishlistproduct", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_UserId", removecart.UserId);
                    command.Parameters.AddWithValue("_ProductId", removecart.ProductId);
                    command.Parameters.AddWithValue("_PackageId", removecart.PackageId);

                    command.ExecuteNonQuery();
                }

                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge(200, "Product removed from cart successfully.");
                apiResult.data = removecart;
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult errorResult = (APIResult)processor.onError(ex.Message);

                return BadRequest(errorResult);
            }
        }


        ///////////////////////////////////////////// BuyNow //////////////////////////////////////////////////////////////////
      


        [HttpPost]
        [Route("api/GetBuyNowProduct")]
        public IActionResult GetBuyNowProduct(int ProductId, int PackageId)
        {
            try
            {
                List<CartProduct> cartProductlist = new List<CartProduct>();
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getbuynowproduct", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_ProductId", ProductId);
                    command.Parameters.AddWithValue("_PackageId", PackageId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartProduct cartProduct = new CartProduct
                            {
                              
                                MainImage = reader["MainImage"].ToString(),
                                //Id = Convert.ToInt32(reader["Id"]),
                                ProductTitle = reader["ProductTitle"].ToString(),
                                //UserId = Convert.ToInt32(reader["UserId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                               // Units = Convert.ToInt32(reader["Units"]),
                                //Quantity = Convert.ToDouble(reader["Quantity"]),
                                PackageId = reader["PackageId"] != DBNull.Value ? Convert.ToInt32(reader["PackageId"]) : 0,
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDouble(reader["Price"]) : 0.0,
                                DiscountPercent = reader["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(reader["DiscountPercent"]) : 0.0,
                                DiscountPrice = reader["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(reader["DiscountAmount"]) : 0.0,
                                SalePrice = reader["SalePrice"] != DBNull.Value ? Convert.ToDouble(reader["SalePrice"]) : 0.0,
                                TotalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToDouble(reader["TotalPrice"]) : 0.0,
                                GSTAmount = reader["GSTAmount"] != DBNull.Value ? Convert.ToDouble(reader["GSTAmount"]) : 0.0,
                                ShippingCharges = reader["ShippingCharges"] != DBNull.Value ? Convert.ToDouble(reader["ShippingCharges"]) : 0.0,
                                //ConveniencePer = reader["ConveniencePer"] != DBNull.Value ? Convert.ToDouble(reader["ConveniencePer"]) : 0.0,
                                //ConvenienceFee = reader["ConvenienceFee"] != DBNull.Value ? Convert.ToDouble(reader["ConvenienceFee"]) : 0.0,
                                //GrandTotalWithoutShipping = reader["GrandTotalWithoutShipping"] != DBNull.Value ? Convert.ToDouble(reader["GrandTotalWithoutShipping"]) : 0.0,
                                GrandTotal = reader["GrandTotal"] != DBNull.Value ? Convert.ToDouble(reader["GrandTotal"]) : 0.0,
                                //ShippingCharges = reader["ShippingCharges"] != DBNull.Value ? Convert.ToDouble(reader["ShippingCharges"]) : 0.0,
                                
                                //DeliveryDate = reader["DeliveryDate"].ToString(),
                                //Size = reader["Size"].ToString(),
                               // Color = reader["Color"].ToString(),
                               // CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue

                            };
                            cartProductlist.Add(cartProduct);
                        }
                    }
                }
                if (cartProductlist.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = cartProductlist;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Brand>();
                    return Ok(apiResult);
                }



            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult errorResult = (APIResult)processor.onError(ex.Message);

                return BadRequest(errorResult);
            }
        }


        [HttpPost]
        [Route("api/GetPincode")]
        public IActionResult GetPincode(int Pincode)
        {
            try
            {
                List<PinCode> Pincodelist = new List<PinCode>();
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_getpincode", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_pincode", Pincode);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PinCode pincode = new PinCode
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Pincode = Convert.ToInt32(reader["pincode"]),
                                State = reader["state"].ToString(),
                                City = reader["city"].ToString(),
                                dates = reader["dates"].ToString(),
                            };
                            Pincodelist.Add(pincode);
                        }
                    }
                }
                if (Pincodelist.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = Pincodelist;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Brand>();
                    return Ok(apiResult);
                }



            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult errorResult = (APIResult)processor.onError(ex.Message);

                return BadRequest(errorResult);
            }
        }

    }
}
