using System.Collections.Generic;
using System;

namespace GOKURTISAPI.Models
{
    public class TrackModel
    {
    }
    public class ResponseShipmentData
    {
        public List<ShipmentD> ShipmentData { get; set; }
    }

    public class ShipmentD
    {
        public string AWB { get; set; }
        public decimal? CODAmount { get; set; }
        public decimal? ChargedWeight { get; set; }
        public Consignee Consignee { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? DestRecieveDate { get; set; }
        public string Destination { get; set; }
        public int DispatchCount { get; set; }
        public List<object> Ewaybill { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Extras { get; set; }
        public DateTime? FirstAttemptDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string OrderType { get; set; }
        public string Origin { get; set; }
        public DateTime? OriginRecieveDate { get; set; }
        public DateTime? OutDestinationDate { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime? PickedupDate { get; set; }
        public string PickupLocation { get; set; }
        public DateTime? PromisedDeliveryDate { get; set; }
        public string Quantity { get; set; }
        public DateTime? RTOStartedDate { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReturnPromisedDeliveryDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public bool ReverseInTransit { get; set; }
        public List<Scan> Scans { get; set; }
        public string SenderName { get; set; }
        public Statuses Status { get; set; }
    }

    public class Consignee
    {
        public List<object> Address1 { get; set; }
        public List<object> Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public int PinCode { get; set; }
        public string State { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
    }

    public class Scan
    {
        public ScanDetail ScanDetail { get; set; }
    }

    public class ScanDetail
    {
        public string Instructions { get; set; }
        public string Scan { get; set; }
        public DateTime ScanDateTime { get; set; }
        public string ScanType { get; set; }
        public string ScannedLocation { get; set; }
        public string StatusCode { get; set; }
        public DateTime StatusDateTime { get; set; }
    }

    public class Statuses
    {
        public string Instructions { get; set; }
        public string RecievedBy { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public DateTime StatusDateTime { get; set; }
        public string StatusLocation { get; set; }
        public string StatusType { get; set; }
    }
}
