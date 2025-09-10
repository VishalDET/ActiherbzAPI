using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynfoShopAPI.Models
{
    //public class UpdateOrderDetails
    //{
   
    //    public int id { get; set; }
    //    public int orderId { get; set; }
    //    public string invoiceNumber { get; set; }
    //    public string invoiceNumberUrl { get; set; }
    //    public string ewayNumber { get; set; }
    //    public string ewayNumberUrl { get; set; }
    //    public string lrNumber { get; set; }
    //    public string lrNumberUrl { get; set; }

    //}

    public class UpdateOrderDetails
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public string invoiceNumber { get; set; }
        public string invoiceNumberUrl { get; set; }
        public string ewayNumber { get; set; }
        public string ewayNumberUrl { get; set; }
        public string lrNumber { get; set; }
        public string lrNumberUrl { get; set; }
        public string TransportNumber { get; set; }
        //public int FranchiseId { get; set; }
        //public int OrderStatus { get; set; }


    }

    public class InsertOrderIntermediateRequest
    {
        public string OrderData { get; set; }
        public string OrderDetailsData { get; set; }
        public int UserId { get; set; }
    }

}
