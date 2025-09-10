using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using SynfoShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SynfoShopAPI.Controllers
{

  [ApiController]
  public class CheckLoginController : ControllerBase
  {
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _hostEnvironment;

    public CheckLoginController(IConfiguration config, IWebHostEnvironment hostEnvironment)
    {
      _config = config;
      _hostEnvironment = hostEnvironment;
    }


    [Route("IsUserRegistered")]
    [HttpPost]
    public IActionResult IsUserRegistered(User user)
    {
      int result;

      using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
      {
        connection.Open();
        MySqlCommand command = new MySqlCommand("usp_Isuserregistered", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("_MobileNo", user.MobileNo);

        MySqlParameter resultParam = new MySqlParameter("_Result", MySqlDbType.Int32);
        resultParam.Direction = ParameterDirection.Output;
        command.Parameters.Add(resultParam);

        command.ExecuteNonQuery();

        result = Convert.ToInt32(command.Parameters["_Result"].Value);
      }

      ServiceRequestProcessor processor = new ServiceRequestProcessor();
      APIResult apiResult;
      if (result == 0)
      {
        apiResult = (APIResult)processor.onUserNotFound();
      }
      else if (result == 1)
      {

        apiResult = (APIResult)processor.customeMessge(200, "User is registered.");
        apiResult.data = new { Id = result };
        return Ok(apiResult);
      }
      else
      {
        apiResult = (APIResult)processor.onError("Unknown error occurred.");
      }

      return Ok(apiResult);
    }

    public static string GenerateOTP(int length)
    {
      Random random = new Random();
      const string chars = "0123456789";
      return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [Route("api/SendOTP")]
    [HttpPost]
    public IActionResult SendOTP(User user)
    {
      EmailerController EmailC = new EmailerController(_config, _hostEnvironment);
      try
      {
        if (user.MobileNo != null)
        {
          int userId = GetUserID(user.MobileNo);
          ServiceRequestProcessor processor = new ServiceRequestProcessor();
          APIResult apiResult;
          // Find your Account SID and Auth Token at twilio.com/console
          // and set the environment variables. See http://twil.io/secure

          string accountSid = "ACb3ecda69867f25c48bea2a0550afe110";
          string authToken = "cde2a818e29c002551d72aa6dc7f08cf";

          TwilioClient.Init(accountSid, authToken);

          string otp = GenerateOTP(4);

          int re = GetUserDetails(user.MobileNo, otp);

          var message = MessageResource.Create(
                     body: "Hi User, " + otp + " is your verification number to login on synfo.shop. This will be valid for the next 300 seconds.",
                     from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                     to: new Twilio.Types.PhoneNumber("whatsapp:+91" + user.MobileNo)
                    );
          //apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
          //apiResult.data = apiResult;
          //return Ok(message.Sid);

          if (userId > 0)
          {
            EmailC.SendOTP_Email(userId, Convert.ToInt32(otp));
          }


          apiResult = (APIResult)processor.customeMessge(100, "OTP Send Successfully");
          apiResult.data = new { OTP = otp };
          return Ok(apiResult);

        }
        else
        {
          ServiceRequestProcessor processor = new ServiceRequestProcessor();
          APIResult apiResult;
          apiResult = (APIResult)processor.customeMessge(200, "Invalid Mobile Number");
          apiResult.data = new { };
          return Ok(apiResult);
        }
      }
      catch (Exception ex)
      {
        ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
        return BadRequest(oServiceRequestProcessor.onError(ex.Message));

      }

    }



    [Route("api/VerifyOTP")]
    [HttpPost]
    public IActionResult VerifyOTP(User user)
    {
      try
      {
        ServiceRequestProcessor processor = new ServiceRequestProcessor();
        APIResult apiResult;
        int result = IsOTPVerify(user);
        //Verify OTP 
        if (result == 1)
        {

          // OTP is correct. Fetch user details from the database.
          User userDetails = GetUserDetails(user.MobileNo);

          if (userDetails != null)
          {
            apiResult = (APIResult)processor.customeMessge(100, "User details fetched successfully.");
            apiResult.data = userDetails;
            return Ok(apiResult);
          }
          else
          {
            apiResult = (APIResult)processor.customeMessge(500, "User details not found.");
            apiResult.data = new { };
            return Ok(apiResult);
          }
        }
        else
        {
          string Msg = string.Empty;
          int number = result;
          switch (number)
          {
            case 0:
              Msg = "OTP Expired!";
              break;

            case -1:
              Msg = "Invalid OTP.";
              break;

            case -2:
              Msg = "Invalid Mobile No";
              break;

            default:
              Msg = "Invalid Mobile No";
              break;
          }
          apiResult = (APIResult)processor.customeMessge(204, Msg);
          apiResult.data = new { };
          return Ok(apiResult);
        }
      }
      catch (Exception ex)
      {
        ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
        return BadRequest(oServiceRequestProcessor.onError(ex.Message));

      }

    }

    private int IsOTPVerify(User user)
    {
      int result = -2;
      try
      {
        if (user != null)
        {
          if (user.MobileNo != null || user.OTP != null)
          {
            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
              connection.Open();
              MySqlCommand command = new MySqlCommand("SELECT MobileNo, Password  FROM tbluser where MobileNo = @MobileNo order by Id Desc LIMIT 1 ;", connection);
              command.Parameters.AddWithValue("@MobileNo", user.MobileNo);

              using (MySqlDataReader reader = command.ExecuteReader())
              {
                if (reader.Read())
                {
                  UserOTPView _userOTPView = new UserOTPView
                  {
                    MobileNo = reader["MobileNo"].ToString(),
                    Password = reader["Password"].ToString(),
                  };

                  //if ("1234" == _userOTPView.OTP && user.MobileNo == _userOTPView.MobileNo)
                  if (user.Password == _userOTPView.Password  && user.MobileNo == _userOTPView.MobileNo)
                  {
                    
                      return result = 1;// correct otp within time
                    
                    
                  }
                  else
                  {
                    return result = -1;// Invalid OTP
                  }
                }
                else
                {
                  return result = -2;// invalid mobile No
                }
              }
            }
          }
          else
          {
            return result;
          }
        }
        else
        {
          return result;
        }
      }
      catch (Exception)
      {
        return result;
      }
    }

    private int GetUserDetails(string mobileNo, string OTP)
    {
      int result = 0;
      try
      {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
          connection.Open();
          MySqlCommand command = new MySqlCommand("Insert Into tblotptracker(MobileNo, OTP, CreatedDate) Values('" + mobileNo + "', '" + OTP + "', '" + DateTime.Now + "');", connection);
          result = command.ExecuteNonQuery();
        }
        return result;
      }
      catch (Exception)
      {
        return result;
      }


    }

    private int GetUserID(string mobileNo)
    {
      int result = 0;
      try
      {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
          connection.Open();
          MySqlCommand command = new MySqlCommand("Select Id from tbluser  where MobileNo = '" + mobileNo + "' limit 1;", connection);
          result = Convert.ToInt32(command.ExecuteScalar());
        }
        return result;
      }
      catch (Exception)
      {
        return result;
      }


    }
    private User GetUserDetails(string mobileNo)
    {
      using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
      {
        connection.Open();
        MySqlCommand command = new MySqlCommand("SELECT U.Id, U.Fullname, U.CompanyName, U.MobileNo, N.Name as NatureOfBusiness, U.EmailId, U.IsGSTIN, U.GSTNumber, U.ShippingAddress, U.StateId, S.State StateName, U.CityId, C.City CityName, U.Pincode, U.IsActive FROM tbluser U Left Join tblstate S on U.StateId = S.Id  Left Join tblcity C on U.CityId = C.Id Left Join tblnatureofbusiness N on U.NatureOfBusiness = N.Id WHERE U.MobileNo = @MobileNo", connection);
        command.Parameters.AddWithValue("@MobileNo", mobileNo);

        using (MySqlDataReader reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            User userDetails = new User
            {
              Id = reader.GetInt32(reader.GetOrdinal("Id")),
              Fullname = reader["Fullname"].ToString(),
              MobileNo = reader["MobileNo"].ToString(),
              NatureOfBusiness = reader["NatureOfBusiness"].ToString(),
              EmailId = reader["EmailId"].ToString(),
              IsGSTIN = reader["IsGSTIN"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["IsGSTIN"]),
              GSTNumber = reader["GSTNumber"].ToString(),
              ShippingAddress = reader["ShippingAddress"].ToString(),
              CompanyName = reader["CompanyName"].ToString(),
              CityId = reader.GetInt32(reader.GetOrdinal("CityId")),
              CityName = reader.GetOrdinal("CityName").ToString(),
              ShowAddress = reader["CityName"].ToString() + " - " + reader["Pincode"].ToString(),
              StateId = reader.GetInt32(reader.GetOrdinal("StateId")),
              IsActive = reader.GetInt32(reader.GetOrdinal("IsActive"))
            };

            return userDetails;
          }
        }
      }

      return null;
    }

  }
}
