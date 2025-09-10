using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FranchiseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShippingAddress { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double GrandTotal { get; set; }
        public double RewardRedeemed { get; set; }
        public double RewardEarned { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public double GSTAmount { get; set; }
        public double IncentiveEarned { get; set; }
        public double RewardOnhold { get; set; }
        public int PayStatus { get; set; }
        public string Referrer { get; set; }
        public int Payment { get; set; }
        public double PaymentAmount { get; set; }
        public string PaymentTrans { get; set; }
        public string PaymentType { get; set; }
        public string DiscountCode { get; set; }
        public int SelfPickup { get; set; }
        public string PickupFrom { get; set; }
        public bool IsRefund { get; set; }
        public string RefundData { get; set; }
        public double RefundAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SalesUserId { get; set; }
        public string SalesUserName { get; set; }
        public string FranchiseName { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public int Result { get; set; }
        public int Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string SynfoInvoiceUrl { get; set; }
        public string Ewaybill { get; set; }
    }

    public class OrderRequestModel
    {
        public int OrderId { get; set; }
        public int OrderStatus { get; set; }
        public int SalesUserId { get; set; }
    }

    public class InsertOrder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double GrandTotal { get; set; }
        public double RewardRedeemed { get; set; }
        public double RewardEarned { get; set; }
        public double IncentiveEarned { get; set; }
        public double RewardOnhold { get; set; }
        public int PayStatus { get; set; }
        public string Referrer { get; set; }
        public int Payment { get; set; }
        public double PaymentAmount { get; set; }
        public string PaymentTrans { get; set; }
        public string PaymentType { get; set; }
        public string DiscountCode { get; set; }
        public int SelfPickup { get; set; }
        public string PickupFrom { get; set; }
        public int Result { get; set; }
        public int FranchiseId { get; set; }
        public string ShippingAddress { get; set; }
        public int IsBuyNow { get; set; }
        public int ProductId { get; set; }
        public int PackageId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
    }


    public class BuyNowOrder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double GrandTotal { get; set; }
        public double RewardRedeemed { get; set; }
        public double RewardEarned { get; set; }
        public double IncentiveEarned { get; set; }
        public double RewardOnhold { get; set; }
        public int PayStatus { get; set; }
        public string Referrer { get; set; }
        public int Payment { get; set; }
        public double PaymentAmount { get; set; }
        public string PaymentTrans { get; set; }
        public string PaymentType { get; set; }
        public string DiscountCode { get; set; }
        public int SelfPickup { get; set; }
        public string PickupFrom { get; set; }
        public int Result { get; set; }
        public int FranchiseId { get; set; }
        public int PackageId {  get; set; }
        public double TotalPrice {  get; set; }
        public double DiscountPrice { get; set;}
        public double GrandTotalPrice { get;set; }
        public int ProductId {  get; set; }
        public string ShippingAddress { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
    }


}
