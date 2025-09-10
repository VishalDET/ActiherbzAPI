using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class Franchise
    {
        public int Id { get; set; }
        public string FranchiseName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public bool IsGSTIN { get; set; }
        public string GSTNumber { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string Taluka { get; set; }
        public bool IsActive { get; set; }
        public string SpType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UserPassword { get; set; }
        public bool IsLoggedIn { get; set; }
    }
    public class FranchiseView : Franchise
    {

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
    public class FranchiseProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string MainImage { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int FranchiseId { get; set; }
        public string FranchiseName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Admin
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string MobileNo { get; set; }
        public string UserPassword { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
    }
    public class Adminlogin
    {
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string MobileNo { get; set; }
        public string userpassword { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public int result { get; set; }

    }
    public class Franchiselogin
    {
        public int Id { get; set; }
        public string FranchiseName { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string MobileNo { get; set; }
        public string userpassword { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public int result { get; set; }

    }

    //public class SalesUser
    //{
    //    public int Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Username { get; set; }
    //    public string MobileNo { get; set; }
    //    public string EmailId { get; set; }
    //    public string UserPassword { get; set; }
    //    public bool IsActive { get; set; }
    //    public bool IsLoggedIn { get; set; }
    //}

    public class SalesUserLogin
    {
            public int Id { get; set; }
            public string SalesUserName { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string username { get; set; }
            public string MobileNo { get; set; }
            public string userpassword { get; set; }
            public string Password { get; set; }

            public bool IsActive { get; set; }
            public bool IsLoggedIn { get; set; }
            public int result { get; set; }
    
    }

}
