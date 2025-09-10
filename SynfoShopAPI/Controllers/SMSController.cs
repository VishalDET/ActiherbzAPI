using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
//using static Org.BouncyCastle.Math.EC.ECCurve;
using SynfoShopAPI.Models;
using System.Collections.Generic;
using System.Data;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Security.Principal;

namespace SynfoShopAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        public static string GenerateOTP(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private readonly IConfiguration _configuration;
        private readonly string accountSid = string.Empty;
        private readonly string authToken = string.Empty;
        public SMSController(IConfiguration configuration)
        {
            _configuration = configuration;

            accountSid = _configuration["AppSeettings:TwilioAccountSid"];
            authToken = _configuration["AppSeettings:TwilioAuthToken"];
        }

        [Route("api/SendOPTNEW")]
        [HttpGet]
        public IActionResult SendOTP(string MobileNo)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure

                string accountSid = "ACb3ecda69867f25c48bea2a0550afe110";
                string authToken = "cde2a818e29c002551d72aa6dc7f08cf";

                TwilioClient.Init(accountSid, authToken);
                string otp = GenerateOTP(4);
                var message = MessageResource.Create(
                    body: "Hi User, " + otp + " is your verification number to login on synfo.shop\r\nThis will be valid for the next 300 seconds.",
                    from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UserRegistrationMSG")]
        [HttpGet]
        public IActionResult UserRegistrationMSG(string MobileNo, string UserName)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                              body: "Dear " + UserName + ",\r\n\r\nThank you for signing up. \r\n\r\nWe're thrilled to have you on board in our Synfo.shop.\r\n\r\nWe've received your request and our backend team is reviewing it.\r\n\r\nWe're looking forward to seeing you in our shop:\r\n\r\nwww.synfo.shop",
                   from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/AccountApproved")]
        [HttpGet]
        public IActionResult AccountApproved(string MobileNo, string UserName, double bonus, string UserId)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;

                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                              body: "Congratulations " + UserName + "!\r\n\r\nYour UserID is " + UserId + "\r\n\r\nYour account has been approved and ready to use.\r\nYou can avail discounts, special offers and the best price across your requirements.\r\n\r\nWarm Regards,",
                   from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/UserOrderPlacedMSG")]
        [HttpGet]
        public IActionResult UserOrderPlacedMSG(string MobileNo, string Fullname, int OrderId)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure


                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                               body: "Thank you " + Fullname + ",\r\n\r\nWe appreciate your valuable order (" + OrderId + ") \r\nand our Synfo team will be processing it soon.\r\n\r\nIf you would like to view the status of your order visit,\r\nhttps://synfo.shop/myaccount#Orders\r\n\r\nFor any changes,\r\nplease reach out to us on 1-800-267-3866\r\n\r\nWarm regards,\r\nSynfocom Team.",
                   from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UserOrderConfirmedMSG")]
        [HttpGet]
        public IActionResult UserOrderConfirmedMSG(string MobileNo, int orderId, string productDetails)
        {
            //productDetails = "TEST";
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure


                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                              body: "Your order is confirmed.\r\nThank you for shopping with us.\r\nYour order number " + orderId + " has been shipped.Your Order no " + orderId + " is fulfilled with " + productDetails + "",
                  from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UserOrderProcessedMSG")]
        [HttpGet]
        public IActionResult UserOrderProcessedMSG(string MobileNo, string UserName)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure


                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                              body: "Congratulations! \r\n\r\nYour order ({{1}}) has been successfully processed.\r\n\r\nPlease check your email for attached invoice. \r\n\r\nYou can use the below link to check your order status: \r\nhttps://synfo.shop/myaccount#Orders \r\n\r\nFor any changes, \r\nkindly reach out to us on 1-800-267-3866 \"Congratulations!  " + UserName + ",\r\n\r\nYour order has been successfully processed.\r\n\r\nYou can use the below link to check your order status :\r\n\r\nhttps://synfo.shop/myaccount\r\n\r\nOr reach out to our support number 1-800-267-3866\r\n\r\nWarm regards,\r\nSynfocom Team.",
                  from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }


        [Route("api/UserOrderDispatchedMSG")]
        [HttpGet]
        public IActionResult UserOrderDispatchedMSG(string MobileNo, string UserName, int orderId)
        {

            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure


                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                              body: "Yay! " + UserName + "\r\n\r\nYour products has been dispatched.\r\n\r\nPlease find the tracking details below:\r\nhttps://synfo.shop/trackorder/" + orderId + "\r\n\r\nYou can check or manage your order using below link:\r\nhttps://synfo.shop/myaccount#Orders\r\n\r\nOr reach out to our support number 1-800-267-3866\r\n\r\nWarm regards,\r\nSynfocom Team.",
                  from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UserOrderDeliveredMSG")]
        [HttpGet]
        public IActionResult UserOrderDeliveredMSG(string MobileNo, string username)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure


                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                              body: "Congratulations {{1}},\r\n\r\nYour order (#4952) has been successfully Delivered.\r\n\r\nWarm regards,\r\nTeam synfo.shop",
                  from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UserOrderCancelledMSG")]
        [HttpGet]
        public IActionResult UserOrderCancelledMSG(string MobileNo, string OrderId)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure


                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                              body: "Congratulations {{1}},\r\n\r\nYour order (#4952) has been successfully Delivered.\r\n\r\nWarm regards,\r\nTeam synfo.shop",
                  from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

        [Route("api/UserRMARequestMSG")]
        [HttpGet]
        public IActionResult UserRMARequestMSG(string MobileNo, string username,string SupportTicket, string macId)
        {
            try
            {
                ServiceRequestProcessor processor = new ServiceRequestProcessor();
                APIResult apiResult;
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure


                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                              body: "Congratulations {{1}},\r\n\r\nYour order (#4952) has been successfully Delivered.\r\n\r\nWarm regards,\r\nTeam synfo.shop",
                  from: new Twilio.Types.PhoneNumber("whatsapp:+19016177692"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+91" + MobileNo)
                );
                apiResult = (APIResult)processor.customeMessge((int)ServiceRequestProcessor.StatusCode.Success, ServiceRequestProcessor.StatusCode.Success.ToString());
                apiResult.data = apiResult;
                return Ok(message.Sid);

            }
            catch (Exception ex)
            {
                ServiceRequestProcessor oServiceRequestProcessor = new ServiceRequestProcessor();
                return BadRequest(oServiceRequestProcessor.onError(ex.Message));
            }
        }

    }
}
