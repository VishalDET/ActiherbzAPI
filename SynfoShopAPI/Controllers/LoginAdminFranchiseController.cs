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
    public class LoginAdminFranchiseController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LoginAdminFranchiseController(IConfiguration config)
        {
            _config = config;
        }

        
        [Route("api/LoginAdminFranchise")]
        [HttpPost]
        public IActionResult LoginAdminFranchisee(LoginAdminFranchise loginadminaranchise)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_loginadminfranchise", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_username", loginadminaranchise.Username);
                    command.Parameters.AddWithValue("p_password", loginadminaranchise.Password);

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters["_Result"].Value);


                    if (result == 1)
                    {
                       
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Adminlogin> AdminList = new List<Adminlogin>();

                                while (reader.Read())
                                {
                                    var adminModel = new Adminlogin
                                    {
                                        //Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id")
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        firstname = reader.IsDBNull(reader.GetOrdinal("firstname")) ? null : reader.GetString("firstname"),
                                        lastname = reader.IsDBNull(reader.GetOrdinal("lastname")) ? null : reader.GetString("lastname"),
                                        username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString("username"),
                                        MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString("MobileNo"),
                                        //userpassword = reader.IsDBNull(reader.GetOrdinal("userpassword")) ? null : reader.GetString("userpassword"),
                                        Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString("Password"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        IsLoggedIn = reader.GetBoolean(reader.GetOrdinal("IsLoggedIn")),
                                        result = result,
                                    };
                                    AdminList.Add(adminModel);
                                }

                                //return Ok(AdminList);
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = AdminList;
                                return Ok(apiResult);
                            }
                            else
                            {
                                //return NotFound();
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { result = result };
                                return NotFound(apiResult);
                            }
                        }
                    }
                    else if(result == 2)
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Franchiselogin> FranchiseList = new List<Franchiselogin>();

                                while (reader.Read())
                                {
                                    var franchiseModel = new Franchiselogin
                                    {
                                        //Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id")
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        FranchiseName = reader.IsDBNull(reader.GetOrdinal("FranchiseName")) ? null : reader.GetString("FranchiseName"),
                                        firstname = reader.IsDBNull(reader.GetOrdinal("firstname")) ? null : reader.GetString("firstname"),
                                        lastname = reader.IsDBNull(reader.GetOrdinal("lastname")) ? null : reader.GetString("lastname"),
                                        username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString("username"),
                                        Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString("Password"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        IsLoggedIn = reader.GetBoolean(reader.GetOrdinal("IsLoggedIn")),
                                        result = result
                                    };
                                    FranchiseList.Add(franchiseModel);
                                }

                                //return Ok(FranchiseList);
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = FranchiseList;
                                return Ok(apiResult);
                            }
                            else
                            {
                                //return NotFound();
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { result = result };
                                return NotFound(apiResult);
                            }
                        }
                    }

                    if (result == 3)
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                               List<SalesUserLogin> SalesUserList = new List<SalesUserLogin>();
                               
                                while (reader.Read())
                                {
                                    var salesuserModel = new SalesUserLogin
                                    {
                                        //Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id")
                                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        SalesUserName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString("FullName"),
                                        firstname = reader.IsDBNull(reader.GetOrdinal("firstname")) ? null : reader.GetString("firstname"),
                                        lastname = reader.IsDBNull(reader.GetOrdinal("lastname")) ? null : reader.GetString("lastname"),
                                        username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString("username"),
                                        //MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString("MobileNo"),
                                        //userpassword = reader.IsDBNull(reader.GetOrdinal("userpassword")) ? null : reader.GetString("userpassword"),
                                        Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString("Password"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        IsLoggedIn = reader.GetBoolean(reader.GetOrdinal("IsLoggedIn")),
                                        result = result,
                                    };
                                    SalesUserList.Add(salesuserModel);
                                }

                                //return Ok(AdminList);
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = SalesUserList;
                                return Ok(apiResult);
                            }
                            else
                            {
                                //return NotFound();
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { result = result };
                                return NotFound(apiResult);
                            }
                        }
                    }

                    else if (result == 0)
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = new { result = result };
                        return NotFound(apiResult);
                    }
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = new { result = result };
                        return NotFound(apiResult);
                        //return null;
                    }



                }
            }
            catch (Exception ex)
            {

                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UserLogin")]
        [HttpPost]
        public IActionResult UserLogin(LoginUser loginUser)
        {
            try
            {
                int result;
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("usp_UserLogin", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Email", loginUser.Username);
                    command.Parameters.AddWithValue("_Password", loginUser.Password);
                    command.Parameters.AddWithValue("_Type", "user");

                    MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters["_Result"].Value);

                    if (result == 1)
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Adminlogin> AdminList = new List<Adminlogin>();

                                while (reader.Read())
                                {
                                    var adminModel = new Adminlogin
                                    {
                                        //Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id")
                                        //Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32("Id"),
                                        //firstname = reader.IsDBNull(reader.GetOrdinal("firstname")) ? null : reader.GetString("firstname"),
                                        //lastname = reader.IsDBNull(reader.GetOrdinal("lastname")) ? null : reader.GetString("lastname"),
                                        username = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString("EmailId"),
                                        //type = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString("username"),
                                        //MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? null : reader.GetString("MobileNo"),
                                        //userpassword = reader.IsDBNull(reader.GetOrdinal("userpassword")) ? null : reader.GetString("userpassword"),
                                        Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString("Password"),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        //IsLoggedIn = reader.GetBoolean(reader.GetOrdinal("IsLoggedIn")),
                                        result = result,
                                    };
                                    AdminList.Add(adminModel);
                                }

                                //return Ok(AdminList);
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                                apiResult.data = AdminList;
                                return Ok(apiResult);
                            }
                            else
                            {
                                //return NotFound();
                                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                                apiResult.data = new { result = result };
                                return NotFound(apiResult);
                            }
                        }
                    }
                    else if (result == 0)
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = new { result = result };
                        return NotFound(apiResult);
                    }
                    else
                    {
                        apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.DataNotFound, ServiceRequestProcessor.StatusCode.DataNotFound.ToString());
                        apiResult.data = new { result = result };
                        return NotFound(apiResult);
                        //return null;
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
