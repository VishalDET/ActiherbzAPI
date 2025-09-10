using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GOKURTISAPI.Models
{
    public class Delhivery
    {
    }

    public class ServiceabilityResponse
    {
        public List<DeliveryCode> delivery_codes { get; set; }
    }

    public class DeliveryCode
    {
        public PostalCode postal_code { get; set; }
    }

    public class PostalCode
    {
        public string city { get; set; }
        public string cod { get; set; }
        public string inc { get; set; }
        public string district { get; set; }
        public int pin { get; set; }
        public double max_amount { get; set; }
        public string pre_paid { get; set; }
        public string cash { get; set; }
        public string state_code { get; set; }
        public double max_weight { get; set; }
        public string pickup { get; set; }
        public string repl { get; set; }
        public string covid_zone { get; set; }
        public string country_code { get; set; }
        public string is_oda { get; set; }
        public string remarks { get; set; }
        public string sort_code { get; set; }
        public bool sun_tat { get; set; }
        public List<Center> center { get; set; }
    }


    public class WarehouseRequests
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string pin { get; set; }
        public string return_address { get; set; }
        public string return_pin { get; set; }
        public string return_city { get; set; }
        public string return_state { get; set; }
        public string return_country { get; set; }
        public string[] BusinessDays { get; set; }
        public Dictionary<string, business_hour> business_hours { get; set; }
        public string Client { get; set; }
    }
    public class Center
    {
        public string code { get; set; }
        public string e { get; set; }
        public string cn { get; set; }
        public string s { get; set; }
        public string u { get; set; }
        public string ud { get; set; }
        public string sort_code { get; set; }
    }


    public class business_hour
    {
        public string start_time { get; set; }
        public string close_time { get; set; }
    }


    public class PickupRequest
    {
        public List<Shipment> shipments { get; set; }
        public PickupLocation pickup_location { get; set; }
    }

    public class PickupLocation
    {
        public string name { get; set; }
        public string add { get; set; }
        public string city { get; set; }
        public int pin_code { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
    }

    public class Shipment
    {
        public string name { get; set; }
        public string add { get; set; }
        public string pin { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string order { get; set; }
        public string payment_mode { get; set; }
        public string return_pin { get; set; }
        public string return_city { get; set; }
        public string return_phone { get; set; }
        public string return_add { get; set; }
        public string return_state { get; set; }
        public string return_country { get; set; }
        public string products_desc { get; set; }
        public string hsn_code { get; set; }
        public string cod_amount { get; set; }
        public string order_date { get; set; }
        public string total_amount { get; set; }
        public string seller_add { get; set; }
        public string seller_name { get; set; }
        public string seller_inv { get; set; }
        public string quantity { get; set; }
        public string waybill { get; set; }
        public string shipment_width { get; set; }
        public string shipment_height { get; set; }
        public string weight { get; set; }
        public string seller_gst_tin { get; set; }
        public string shipping_mode { get; set; }
        public string address_type { get; set; }
    }

    public class PickupRequests
    {
        public string PickupLocation { get; set; }
        public string ExpectedPackageCount { get; set; }
        public string PickupDate { get; set; } // Format: "YYYY-MM-DD"
        public string PickupTime { get; set; } // Format: "HH:MM:SS"
    }

    public class PackageUpdateRequest
    {
        public List<PackageData> Data { get; set; }
    }

    public class PackageData
    {
        public string Waybill { get; set; }
        public string Act { get; set; }
    }

    public class CalculateShippingCostResponseModel
    {
        public Dictionary<string, object> adhoc_data { get; set; }
        public decimal charge_AIR { get; set; }
        public decimal charge_AWB { get; set; }
        public decimal charge_CCOD { get; set; }
        public decimal charge_CNC { get; set; }
        public decimal charge_COD { get; set; }
        public decimal charge_COVID { get; set; }
        public decimal charge_CWH { get; set; }
        public decimal charge_DEMUR { get; set; }
        public decimal charge_DL { get; set; }
        public decimal charge_DOCUMENT { get; set; }
        public decimal charge_DPH { get; set; }
        public decimal charge_DTO { get; set; }
        public decimal charge_E2E { get; set; }
        public decimal charge_FOD { get; set; }
        public decimal charge_FOV { get; set; }
        public decimal charge_FS { get; set; }
        public decimal charge_FSC { get; set; }
        public decimal charge_INS { get; set; }
        public decimal charge_LABEL { get; set; }
        public decimal charge_LM { get; set; }
        public decimal charge_MPS { get; set; }
        public decimal charge_POD { get; set; }
        public decimal charge_QC { get; set; }
        public decimal charge_REATTEMPT { get; set; }
        public decimal charge_ROV { get; set; }
        public decimal charge_RTO { get; set; }
        public decimal charge_WOD { get; set; }
        public decimal charge_pickup { get; set; }
        public decimal charged_weight { get; set; }
        public decimal gross_amount { get; set; }
        public string status { get; set; }
        public TaxData tax_data { get; set; }
        public decimal total_amount { get; set; }
        public object wt_rule_id { get; set; }
        public object zonal_cl { get; set; }
        public string zone { get; set; }
    }

    public class TaxData
    {
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal krishi_kalyan_cess { get; set; }
        public decimal service_tax { get; set; }
        public decimal swacch_bharat_tax { get; set; }
    }

    public class ShipmentPackage
    {
        public string client { get; set; }
        public double cod_amount { get; set; }
        public string payment { get; set; }
        public string refnum { get; set; }
        public List<string> remarks { get; set; }
        public bool serviceable { get; set; }
        public string sort_code { get; set; }
        public string status { get; set; }
        public string waybill { get; set; }
    }

    public class ShipmentResponse
    {
        public double cash_pickups { get; set; }
        public double cash_pickups_count { get; set; }
        public double cod_amount { get; set; }
        public int cod_count { get; set; }
        public int package_count { get; set; }
        public List<ShipmentPackage> packages { get; set; }
        
        public int pickups_count { get; set; }
        public int prepaid_count { get; set; }
        public int replacement_count { get; set; }
        public bool success { get; set; }
        public string upload_wbn { get; set; }
        public string rmk { get; set; }
    }

}
