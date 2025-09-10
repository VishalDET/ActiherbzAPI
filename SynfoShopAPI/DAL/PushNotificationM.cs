using MimeKit;
using SynfoShopAPI.Models;
using System;
using static SynfoShopAPI.Models.GoogleNotification;
using System.Net.Http.Headers;
using System.Runtime;
using System.Threading.Tasks;
using System.Net.Http;
using CorePush.Google;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace SynfoShopAPI.DAL
{
    public class PushNotificationM
    {

        private readonly IConfiguration _config;
        public PushNotificationM(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (notificationModel.IsAndroiodDevice)
                {
                    /* FCM Sender (Android Device) */
                    string SenderId = _config["FcmNotification:SenderId"].ToString();
                    string ServerKey = _config["FcmNotification:ServerKey"].ToString();

                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = SenderId,
                        ServerKey = ServerKey
                    };

                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                    string deviceToken = notificationModel.TokenValue;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }
                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }
    }
}
