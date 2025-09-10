using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SynfoShopAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class RmaController : ControllerBase
    {
        private readonly IConfiguration _config;

        public RmaController(IConfiguration config)
        {
            _config = config;
        }

        [Route("api/AddRMA")]
        [HttpPost]
        public IActionResult AddRMA(AddRma addRma)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("usp_addrma", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_Id", addRma.Id);
                    command.Parameters.AddWithValue("_OrderId", addRma.OrderId);
                    command.Parameters.AddWithValue("_MacId", addRma.MacId);
                    command.Parameters.AddWithValue("_ProductFault", addRma.ProductFault);
                    command.Parameters.AddWithValue("_Description", addRma.Description);
                    command.Parameters.AddWithValue("_SupportTicket", addRma.SupportTicket);
                    command.Parameters.Add("_Result", MySqlDbType.Int32);
                    command.Parameters["_Result"].Direction = ParameterDirection.Output;
                    //int resultValue = Convert.ToInt32(command.Parameters["_Result"].Value);
                    command.ExecuteNonQuery();
                    int result = Convert.ToInt32(command.Parameters["_Result"].Value);

                    if (result >= 0)
                    {

                        SMSController smsC = new SMSController(_config);

                        //smsC.UserRMARequestMSG();
                    }
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = result;

                    return Ok(apiResult);
                }

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/GetRMAData")]
        [HttpPost]
        public IActionResult GetRMAData(int FranchiseId, int rmaId, int rmaStatus)
        {
            List<RmaView> rmaList = new List<RmaView>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getrma", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_FranchiseId", FranchiseId);
                    command.Parameters.AddWithValue("_rmaId", rmaId);
                    command.Parameters.AddWithValue("_rmaStatus", rmaStatus);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RmaView rma = new RmaView
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Fullname = reader["Fullname"].ToString(),
                                OrderId = Convert.ToInt32(reader["UserId"]),

                                MacId = reader["MacId"].ToString(),
                                ProductFault = reader["ProductFault"] == DBNull.Value ? "0" : Convert.ToString(reader["ProductFault"]),
                                Description = reader["Description"] == DBNull.Value ? "0" : Convert.ToString(reader["Description"]),
                                SupportTicket = reader["SupportTicket"] == DBNull.Value ? "0" : Convert.ToString(reader["SupportTicket"]),
                                BookedDate = reader["BookDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["BookDate"]),
                                ReceievedDate = reader["ReceivedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ReceivedDate"]),
                                DeliveredDate = reader["DeliveredDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DeliveredDate"]),
                                RMAStatus = reader["RMAStatus"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RMAStatus"]),
                                CnNumber = reader["CnNumber"] == DBNull.Value ? "0" : Convert.ToString(reader["CnNumber"]),
                                LRPath = reader["LRPath"] == DBNull.Value ? "0" : Convert.ToString(reader["LRPath"]),
                                LRNumber = reader["LRNumber"] == DBNull.Value ? "0" : Convert.ToString(reader["LRNumber"]),
                                RefundDate = reader["RefundDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["RefundDate"]),
                                NewMacId = reader["NewMacId"].ToString(),
                                FranchiseName = reader["FranchiseName"] == DBNull.Value ? "0" : Convert.ToString(reader["FranchiseName"]),
                                OrderDate = reader["BookDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["BookDate"]),
                                //CreatedDate =  Convert.ToDateTime(reader["CreatedDate"]),
                                //ModifiedDate =  Convert.ToDateTime(reader["ModifiedDate"]),

                            };
                            rmaList.Add(rma);
                        }

                    }
                }
                if (rmaList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = rmaList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<RmaView>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = null;
                return NotFound(apiResult);
            }
        }


        [Route("api/GetRMAEnquiryData")]
        [HttpPost]
        public IActionResult GetRMAEnquiryData(int FranchiseId, int rmaId, int rmaStatus, int SalesUserId)
        {
            List<RmaView> rmaList = new List<RmaView>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getrmaforenquiry", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_FranchiseId", FranchiseId);
                    command.Parameters.AddWithValue("_rmaId", rmaId);
                    command.Parameters.AddWithValue("_rmaStatus", rmaStatus);
                    command.Parameters.AddWithValue("_SalesUserId", SalesUserId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RmaView rma = new RmaView
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Fullname = reader["Fullname"].ToString(),
                                OrderId = Convert.ToInt32(reader["UserId"]),

                                MacId = reader["MacId"].ToString(),
                                ProductFault = reader["ProductFault"] == DBNull.Value ? "0" : Convert.ToString(reader["ProductFault"]),
                                Description = reader["Description"] == DBNull.Value ? "0" : Convert.ToString(reader["Description"]),
                                SupportTicket = reader["SupportTicket"] == DBNull.Value ? "0" : Convert.ToString(reader["SupportTicket"]),
                                BookedDate = reader["BookDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["BookDate"]),
                                ReceievedDate = reader["ReceivedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ReceivedDate"]),
                                DeliveredDate = reader["DeliveredDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DeliveredDate"]),
                                RMAStatus = reader["RMAStatus"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RMAStatus"]),
                                CnNumber = reader["CnNumber"] == DBNull.Value ? "0" : Convert.ToString(reader["CnNumber"]),
                                LRPath = reader["LRPath"] == DBNull.Value ? "0" : Convert.ToString(reader["LRPath"]),
                                LRNumber = reader["LRNumber"] == DBNull.Value ? "0" : Convert.ToString(reader["LRNumber"]),
                                RefundDate = reader["RefundDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["RefundDate"]),
                                NewMacId = reader["NewMacId"].ToString(),
                                FranchiseName = reader["FranchiseName"] == DBNull.Value ? "0" : Convert.ToString(reader["FranchiseName"]),
                                //CreatedDate =  Convert.ToDateTime(reader["CreatedDate"]),
                                //ModifiedDate =  Convert.ToDateTime(reader["ModifiedDate"]),

                            };
                            rmaList.Add(rma);
                        }

                    }
                }
                if (rmaList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = rmaList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<RmaView>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = null;
                return NotFound(apiResult);
            }
        }


        [Route("api/UpdateRmaDetails")]
        [HttpPost]
        public IActionResult UpdateRmaDetails(int rmaId, int rmaStatus)
        {
            try
            {
                int result = 0;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_updatermastatus", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_rmaId", rmaId);
                    command.Parameters.AddWithValue("_rmaStatus", rmaStatus);
                    //command.Parameters.AddWithValue("_ModifiedBy", modifiedBy);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters["_Result"].Value);
                }

                if (result == 1)
                {

                    UpdateRma updateRma = new UpdateRma();
                    //updateRma = GetRMAData(0, rmaId, 0);

                    if (rmaStatus == 2)// Order Confirmed
                    {
                        List<UpdateRma> updateRmas = new List<UpdateRma>();
                        // updateRmas = Od.GetOrderDetailsForsms(rmaId);
                        string updateRm = string.Empty;

                        foreach (UpdateRma item in updateRmas)
                        {
                            if (updateRm == string.Empty)
                            {
                                updateRm = item.RMAStatus.ToString();
                            }

                        }

                    }
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = result;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = 0;
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UpdateRmaRefundorRretun")]
        [HttpPost]
        public IActionResult UpdateRmaRefundorRretun(int rmaId, int rmaStatus, string CnNumber, string LRPath, string LRNumber, string MacId)
        {
            try
            {
                int result = 0;

                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_updatermareturnorrefund", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("_rmaId", rmaId);
                    command.Parameters.AddWithValue("_rmaStatus", rmaStatus);
                    command.Parameters.AddWithValue("_CnNumber", CnNumber);
                    command.Parameters.AddWithValue("_LRPath", LRPath);
                    command.Parameters.AddWithValue("_LRNumber", LRNumber);
                    command.Parameters.AddWithValue("_MacId", MacId);


                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters["_Result"].Value);
                }

                if (result == 1)
                {

                    UpdateRma updateRma = new UpdateRma();
                    //updateRma = GetRMAData(0, rmaId, 0);

                    if (rmaStatus == 2)// Order Confirmed
                    {
                        List<UpdateRma> updateRmas = new List<UpdateRma>();
                        // updateRmas = Od.GetOrderDetailsForsms(rmaId);
                        string updateRm = string.Empty;

                        foreach (UpdateRma item in updateRmas)
                        {
                            if (updateRm == string.Empty)
                            {
                                updateRm = item.RMAStatus.ToString();
                            }

                        }

                    }
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = result;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = 0;
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }
        [Route("api/GetSupportData")]
        [HttpPost]
        public IActionResult GetSupportData(int UserId)
        {
            List<Support> supportList = new List<Support>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_getsupport", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_UserId", UserId);

                    DataTable dtData = new DataTable();
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(ds);

                    DataTable dtSupport = new DataTable();
                    dtSupport = ds.Tables[0];
                    dtData = ds.Tables[1];

                    foreach (DataRow dr in dtSupport.Rows)
                    {
                        DataTable _dtNew = new DataTable();
                        _dtNew = dtData.AsEnumerable().Where(p => p["MacId"].ToString() == dr["MacId"].ToString() && Convert.ToInt32(p["UserId"]) == Convert.ToInt32(dr["UserId"])).CopyToDataTable();


                        SupportData _supportData = new SupportData
                        {
                            Id = Convert.ToInt32(_dtNew.Rows[0]["Id"]),
                            MacId = _dtNew.Rows[0]["MacId"].ToString(),
                            OrderId = Convert.ToInt32(_dtNew.Rows[0]["OrderId"]),
                            RMAStatus = Convert.ToInt32(_dtNew.Rows[0]["RMAStatus"].ToString()),
                            InvoiceNumber = _dtNew.Rows[0]["InvoiceNumber"].ToString(),
                            OrderDate = _dtNew.Rows[0]["OrderDate"].ToString(),
                            //Received = _dtNew.Rows[0]["Received"].ToString(),
                            //Shipped = _dtNew.Rows[0]["Shipped"].ToString(),
                            //Submited = _dtNew.Rows[0]["Submited"].ToString(),
                            UserId = Convert.ToInt32(_dtNew.Rows[0]["UserId"]),
                            WarrantyExpiryDate = _dtNew.Rows[0]["WarrantyExpiryDate"].ToString(),

                            IsPending = Convert.ToInt32(_dtNew.Rows[0]["IsPending"].ToString()),
                            SubmitedDate = _dtNew.Rows[0]["SubmitedDate"].ToString(),
                            IsToBeReceived = Convert.ToInt32(_dtNew.Rows[0]["IsToBeReceived"].ToString()),
                            IsReceived = Convert.ToInt32(_dtNew.Rows[0]["IsReceived"].ToString()),
                            ReceivedDate = _dtNew.Rows[0]["ReceivedDate"].ToString(),
                            IsRefund = Convert.ToInt32(_dtNew.Rows[0]["IsRefund"].ToString()),
                            RefundDate = _dtNew.Rows[0]["RefundDate"].ToString(),
                            IsReturn = Convert.ToInt32(_dtNew.Rows[0]["IsReturn"].ToString()),
                            DeliveredDate = _dtNew.Rows[0]["DeliveredDate"].ToString(),

                        };

                        Support _support = new Support()
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            MacId = dr["MacId"].ToString(),
                            MainImage = dr["MainImage"].ToString(),
                            BookedDate = dr["BookDate"].ToString(),
                            Status = dr["Status"].ToString(),
                            ProductTitle = dr["ProductTitle"].ToString(),
                            RMAStatus = Convert.ToInt32(dr["RMAStatus"]),
                            SalePrice = Convert.ToDouble(dr["SalePrice"]),
                            SupportTicket = dr["SupportTicket"].ToString(),
                            supportData = _supportData,

                        };
                        supportList.Add(_support);
                    }
                }

                if (supportList.Count > 0)
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                    apiResult.data = supportList;
                    return Ok(apiResult);
                }
                else
                {
                    ServiceRequestProcessor processor = new ServiceRequestProcessor();
                    APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                    apiResult.data = new List<Support>();
                    return NotFound(apiResult);
                }
            }
            catch (Exception ex)
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                apiResult.data = null;
                return NotFound(apiResult);
            }
        }


    }
}
