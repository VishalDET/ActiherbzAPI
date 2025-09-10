using System;

namespace SynfoShopAPI.Models
{
    public class AddRma
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string MacId { get; set; }
        public string ProductFault { get; set; }
        public string Description { get; set; }
        public string SupportTicket { get; set; }
        public DateTime BookedDate { get; set; }
        public DateTime ReceievedDate { get; set; }
        public int RMAStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public static implicit operator AddRma(RmaView v)
        {
            throw new NotImplementedException();
        }
    }

    public class RmaView
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public int OrderId { get; set; }
        public string MacId { get; set; }
        public string ProductFault { get; set; }
        public string Description { get; set; }
        public string SupportTicket { get; set; }
        public DateTime BookedDate { get; set; }
        public DateTime ReceievedDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public int RMAStatus { get; set; }
        public string CnNumber { get; set; }
        public string LRPath { get; set; }
        public string LRNumber { get; set; }
        public DateTime RefundDate { get; set; }
        public string NewMacId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string FranchiseName { get; set; }
        public string Warrantyexpiry { get; set; }
        public DateTime OrderDate { get; set; }
    }


    public class UpdateRma
    {
        public int RMAStatus { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Support
    {
        public int Id { get; set; }
        public string ProductTitle { get; set; }
        public string MacId { get; set; }
        public string MainImage { get; set; }
        public string Status { get; set; }
        public double SalePrice { get; set; }
        public string SupportTicket { get; set; }
        public string BookedDate { get; set; }
        public int RMAStatus { get; set; }
        public SupportData supportData { get; set; }
    }
    public class SupportData
    {//Id, MacId, OrderId, UserId, OrderDate, InvoiceNumber, WarrantyExpiryDate, Submited, Shipped, Received
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string MacId { get; set; }
        public string OrderDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string WarrantyExpiryDate { get; set; }
        //public string Submited { get; set; }
        //public string Shipped { get; set; }
        //public string Received { get; set; }
        public int IsPending { get; set; }
        public string SubmitedDate { get; set; }
        public int IsToBeReceived { get; set; }
        public int IsReceived { get; set; }
        public string ReceivedDate { get; set; }
        public int IsRefund { get; set; }
        public string RefundDate { get; set; }
        public int IsReturn { get; set; }
        public string DeliveredDate { get; set; }
        public int RMAStatus { get; set; }
    }
}
