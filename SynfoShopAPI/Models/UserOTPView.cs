using System;

namespace SynfoShopAPI.Models
{
    public class UserOTPView
    {
        public string MobileNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OTP { get; set; }
        public string Password { get; set; }
    }
}
