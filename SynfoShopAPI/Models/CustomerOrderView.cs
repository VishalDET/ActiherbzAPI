using System;
using System.Collections.Generic;
using static iTextSharp.tool.xml.html.table.TableRowElement;

namespace SynfoShopAPI.Models
{
    public class CustomerOrderView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderStatus { get; set; }
        public string City { get; set; }
        public double GrandTotal { get; set; }
        public string OrderDate { get; set; }
        public string GroupId { get; set; }
        public string ETA { get; set; }
        public string MacIds { get; set; }
        public string InvoiceNo { get; set; }
        public string PlaceofDispatch { get; set; }

        
        public List<List<CustomerOrderDetailsView>> OrderDetails { get; set;}
    }
    public class CustomerOrderDetailsView
    {
        public int OrderId  { get; set; }
        public int UserId { get; set; }
        public int FranchiseId { get; set; }
        public string ProductTitle { get; set; }
        public string MainImage { get; set; }
        public int Quantity { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceNumberUrl { get; set; }
        public string EwayNumber { get; set; }
        public string EwayNumberUrl { get; set; }
        public string LRNumber { get; set; }
        public string LRNumberUrl { get; set; }
        


    }
    public class CustomerDOrderView
    {
        public int Id { get; set; }
        public double IsDelivered { get; set; }
        public string DispatchedDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderDate { get; set; }
        public List<CustomerDOrderDetailsView> OrderDetails { get; set; }
        public string InvoiceNumberUrl { get; set; }
        public string EwayNumberUrl { get; set; }
        public string LRNumberUrl { get; set; }
    }
    public class CustomerDOrderDetailsView
    {
        public string ProductTitle { get; set; }
        public string MainImage { get; set; }
        public int Quantity { get; set; }
        public double TPrice { get; set; }
        public double TSalePrice { get; set; }
        public int IsPlaced { get; set; }
        public string PlacedDate { get; set; }
        public int IsConfirmed { get; set; }
        public string ConfirmedDate { get; set; }
        public int IsProcessing { get; set; }
        public string ProcessedDate { get; set; }
        public int IsDispatched { get; set; }
        public string DispatchedDate { get; set; }
        public int IsDelivered { get; set; }
        public string DeliveredDate { get; set; }
    }
}
