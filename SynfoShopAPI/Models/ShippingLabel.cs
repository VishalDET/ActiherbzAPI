using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace GOKURTISAPI.Models
{
    public class ShippingLabelResponse
    {
        public int status_code { get; set; }
        public string message { get; set; }
        public PackagesResponse data { get; set; }
    }

    public class Package
    {
        public string origin { get; set; }
        public object invoice_reference { get; set; }
        public double shipment_width { get; set; }
        public int pin { get; set; }
        public string cl { get; set; }
        public object intl { get; set; }
        public string origin_state_code { get; set; }
        public DateTime cd { get; set; }
        public List<object> ewbn { get; set; }
        public string rph { get; set; }
        public double shipment_length { get; set; }
        public string snm { get; set; }
        public string barcode { get; set; }
        public string origin_city { get; set; }
        public double weight { get; set; }
        public string pt { get; set; }
        public double rs { get; set; }
        public string destination { get; set; }
        public string si { get; set; }
        public string destination_city { get; set; }
        public string hsn_code { get; set; }
        public string tin { get; set; }
        public string contact { get; set; }
        public string origin_state { get; set; }
        public string fm_ucid { get; set; }
        public string sid { get; set; }
        public string cst { get; set; }
        public string prd { get; set; }
        public string rcty { get; set; }
        public object consignee_gst_tin { get; set; }
        public string cnph { get; set; }
        public string sadd { get; set; }
        public string oid_barcode { get; set; }
        public string oid { get; set; }
        public string customer_state { get; set; }
        public string mot { get; set; }
        public string radd { get; set; }
        public string customer_state_code { get; set; }
        public string address { get; set; }
        public string rst { get; set; }
        public string qty { get; set; }
        public object seller_gst_tin { get; set; }
        public double shipment_height { get; set; }
        public DateTime pdd { get; set; }
        public string product_type { get; set; }
        public string name { get; set; }
        public object is_sdc { get; set; }
        public string st_code { get; set; }
        public string cl_logo { get; set; }
        public string st { get; set; }
        public string client_gst_tin { get; set; }
        public string etc { get; set; }
        public string delhivery_logo { get; set; }
        public string client_type { get; set; }
        public double cod { get; set; }
        public string wbn { get; set; }
        public string sort_code { get; set; }
        public int rpin { get; set; }
        public string pdf_download_link { get; set; } // Only present when isPdf is true
        public string pdf_encoding { get; set; } // Only present when isPdf is true
    }

    public class PackagesResponse
    {
        public List<Package> packages { get; set; }
        public int packages_found { get; set; }
    }
}
