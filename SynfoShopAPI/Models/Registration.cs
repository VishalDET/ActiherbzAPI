using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string CompanyName { get; set; }
        public string MobileNo { get; set; }
        public int NatureOfBusiness { get; set; }
        public string EmailId { get; set; }
        public bool? IsGSTIN { get; set; }
        public string GSTNumber { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public int Pincode { get; set; }
        public string CityName { get; set; }
        public string SpType { get; set; }
       
        public DateTime? CreatedDate { get; set; }
        public int? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class RegistrationWithSalesUser
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string CompanyName { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public int NatureOfBusiness { get; set; }
        public string NatureOfBusinessName { get; set; }
        public string EmailId { get; set; }
        public bool? IsGSTIN { get; set; }
        public string GSTNumber { get; set; }
        public string ShippingAddress { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string SpType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? IsActive { get; set; }
        public int SalesUserId { get; set; }
        public string BillingAddress { get; set; }
        public int Pincode { get; set; }
        public string SalesFullName { get; set; }
        public string Taluka { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    //public class RegistrationWithSalesUser
    //{
    //    public int Id { get; set; }
    //    public string FullName { get; set; }
    //    public string CompanyName { get; set; }
    //    public string MobileNo { get; set; }
    //    public string NatureofBusinessName { get; set; }
    //    public string EmailId { get; set; }
    //    public string IsGSTIN { get; set; }
    //    public string GSTNumber {  get; set; }
    //    public string ShippingAddress { get; set; }
    //    public string StateId { get; set; }
    //    public string StateName { get; set; }
    //    public string CityId { get; set; }
    //    public string CityName { get; set; }
    //    public string SpType { get; set; }
    //    public string CreatedDate { get; set; }
    //    public string IsActive { get; set; }
    //    public string SalesUserId { get; set; }
    //    public string BillingAddrss { get; set; }
    //    public string Pincode { get; set; }
    //    public string SalesFullName { get; set; }
    //    public string Taluka { get; set; }
    //    public DateTime? ModifiedDate { get; set; }

    //}

    public class RegistrationWithUserType: RegistrationWithSalesUser
    { 
        public int UserType { get; set; }
    }
    public class SalesUserEmail
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string CompanyName { get; set; }
        public string MobileNo { get; set; }
        public string NatureOfBusiness { get; set; }
        public string EmailId { get; set; }
        public string SalesEmailId { get; set; }
        public bool? IsGSTIN { get; set; }
        public string GSTNumber { get; set; }
        public string ShippingAddress { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string SpType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? IsActive { get; set; }
        public int SalesUserId { get; set; }
        public string SalesFullName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}



