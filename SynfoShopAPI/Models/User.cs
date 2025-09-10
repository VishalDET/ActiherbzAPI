using System;

namespace SynfoShopAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IsActive { get; set; }
        public string MobileNo { get; set; }
        public string Type { get; set; }
        public int Result { get; set; }
        public string OTP { get; set; }
        public string NatureOfBusiness { get; set; }
        public string EmailId { get; set; }
        public bool? IsGSTIN { get; set; }
        public string GSTNumber { get; set; }
        public string ShippingAddress { get; set; }
        public string CityName { get; set; }
        public string CompanyName { get; set; }
        public string ShowAddress { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        
            
    }
}
