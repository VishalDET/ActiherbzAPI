using System.Collections.Generic;
using System;

namespace SynfoShopAPI.Models
{
    public class ShipRock
    {
        public string access { get; set; }
    }
    public class ShiprocketAuthResponse
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public int Company_id { get; set; }
        public DateTime Created_at { get; set; }
        public string Token { get; set; }
    }
    public class ShipLoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }


    public class ShipRPickupRequestModel
    {
        public int[] OrderId { get; set; }
        public string PickupLocation { get; set; }
    }

    public class RocketBoxOrderResponse
    {
        public bool success { get; set; }
        public int order_id { get; set; }
        public int from_warehouse_id { get; set; }
        public int to_warehouse_id { get; set; }
        public string mode { get; set; }
        public int mode_id { get; set; }
        public string delivery_partner_name { get; set; }
        public int delivery_partner_id { get; set; }
        public string transportar_id { get; set; }
    }


    public class RocketBoxShippingDetails
    {
        public int no_of_packages { get; set; }
        public string approx_weight { get; set; }
        public bool is_insured { get; set; }
        public bool is_to_pay { get; set; }
        public decimal? to_pay_amount { get; set; }
        public string source_warehouse_name { get; set; }
        public string source_address_line1 { get; set; }
        public string source_address_line2 { get; set; }
        public string source_pincode { get; set; }
        public string source_city { get; set; }
        public string source_state { get; set; }
        public string sender_contact_person_name { get; set; }
        public string sender_contact_person_email { get; set; }
        public string sender_contact_person_contact_no { get; set; }
        public string destination_warehouse_name { get; set; }
        public string destination_address_line1 { get; set; }
        public string destination_address_line2 { get; set; }
        public string destination_pincode { get; set; }
        public string destination_city { get; set; }
        public string destination_state { get; set; }
        public string recipient_contact_person_name { get; set; }
        public string recipient_contact_person_email { get; set; }
        public string recipient_contact_person_contact_no { get; set; }
        public int client_id { get; set; }
        public List<RocketBoxPackagingUnitDetails> packaging_unit_details { get; set; }
        public string recipient_GST { get; set; }
        public string volumetric_weight { get; set; }
        public string[] supporting_docs { get; set; }
        public string shipment_type { get; set; }
        public bool is_cod { get; set; }
        public decimal? cod_amount { get; set; }
        public string mode_name { get; set; }
        public string source { get; set; }
        public string client_order_id { get; set; }
    }

    public class RocketBoxPackagingUnitDetails
    {
        public int units { get; set; }
        public decimal weight { get; set; }
        public decimal length { get; set; }
        public decimal height { get; set; }
        public decimal width { get; set; }
        public string display_in { get; set; }
    }


    public class ShipmentOrderRequest
    {
        public int client_id { get; set; }
        public int order_id { get; set; }
        public string remarks { get; set; }
        public string recipient_gst { get; set; }  // change the type accordingly if it's not always null
        public string to_pay_amount { get; set; }
        public int mode_id { get; set; }
        public int delivery_partner_id { get; set; }
        public string pickup_date_time { get; set; }
        public string eway_bill_no { get; set; }
        public double invoice_value { get; set; }
        public string invoice_number { get; set; }
        public string invoice_date { get; set; }
        public List<string> supporting_docs { get; set; }
    }

    public class PackagingUnitDetails
    {
        public int units { get; set; }
        public double weight { get; set; }
        public double length { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public string unit { get; set; }
    }

    public class DeliveryPartner
    {
        public int id { get; set; }
        public string name { get; set; }
        public string common_name { get; set; }
        public bool is_pickup_ready { get; set; }
        public bool is_public { get; set; }
        public bool is_shipment_ready { get; set; }
        public bool is_insurance_available { get; set; }
        public string cut_off_time { get; set; }
        public bool is_to_pay_available { get; set; }
        public bool is_cheque_available { get; set; }
        public string logo { get; set; }
        public bool is_disable { get; set; }
        public bool is_listing_ready { get; set; }
    }

    public class Mode
    {
        public int id { get; set; }
        public DeliveryPartner delivery_partner { get; set; }
        public string mode_name { get; set; }
        public string common_name { get; set; }
        public bool is_active { get; set; }
    }

    public class CreatedBy
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
    }

    public class UpdatedBy
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
    }

    public class DeliveryResponse
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public int from_warehouse_id { get; set; }
        public string sender_contact_person_name { get; set; }
        public string sender_contact_person_contact_no { get; set; }
        public string sender_contact_person_email { get; set; }
        public DateTime pickup_date_time { get; set; }
        public string approx_weight { get; set; }
        public List<PackagingUnitDetails> packaging_unit_details { get; set; }
        public Mode mode { get; set; }
        public string invoice_value { get; set; }
        public int to_warehouse_id { get; set; }
        public string recipient_contact_person_name { get; set; }
        public string recipient_contact_person_email { get; set; }
        public string recipient_contact_person_contact_no { get; set; }
        public int no_of_units { get; set; }
        public string eway_bill_no { get; set; }
        public string remarks { get; set; }
        public bool is_insured { get; set; }
        public object recipient_GST { get; set; }
        public string invoice_number { get; set; }
        public object package_content { get; set; }
        public object shipment_purpose { get; set; }
        public DeliveryPartner delivery_partner { get; set; }
        public int from_warehouse { get; set; }
        public int to_warehouse { get; set; }
        public CreatedBy created_by { get; set; }
        public UpdatedBy updated_by { get; set; }
        public int order_id { get; set; }
        public int order { get; set; }
        public bool is_to_pay { get; set; }
        public string to_pay_amount { get; set; }
        public bool is_cod { get; set; }
        public object cod_amount { get; set; }
        public string cod_payment_mode { get; set; }
        public List<string> supporting_docs { get; set; }
        public string invoice_date { get; set; }
        public object delivery_slots { get; set; }
        public object cheque_in_favor_of { get; set; }
        public string waybill_no { get; set; }
        public string lrn { get; set; }
        public List<string> child_waybill_nos { get; set; }
        public object appointment_date { get; set; }
        public bool is_appointment_taken { get; set; }
        public bool tracking_status { get; set; }
        public string source { get; set; }
        public object api_error { get; set; }
        public object other_details { get; set; }
        public string label_url { get; set; }
        public object secure_shipment_detail { get; set; }
    }
    public class RocketBoxShipmentResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int FromWarehouseId { get; set; }
        public string SenderContactPersonName { get; set; }
        public string SenderContactPersonContactNo { get; set; }
        public string SenderContactPersonEmail { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string ApproxWeight { get; set; }
        public List<RocketBoxPackageDetails> PackagingUnitDetails { get; set; }
        public RocketBoxModeInfo Mode { get; set; }
        public double InvoiceValue { get; set; }
        public int ToWarehouseId { get; set; }
        public string RecipientContactPersonName { get; set; }
        public string RecipientContactPersonEmail { get; set; }
        public string RecipientContactPersonContactNo { get; set; }
        public int NoOfUnits { get; set; }
        public string EwayBillNo { get; set; }
        public string Remarks { get; set; }
        public bool IsInsured { get; set; }
        public object RecipientGST { get; set; } // Change the type accordingly if it's not always null
        public string InvoiceNumber { get; set; }
        public object PackageContent { get; set; } // Change the type accordingly
        public object ShipmentPurpose { get; set; } // Change the type accordingly
        public RocketBoxDeliveryPartnerInfo DeliveryPartner { get; set; }
        public int FromWarehouse { get; set; }
        public int ToWarehouse { get; set; }
        public RocketBoxCreatedByInfo CreatedBy { get; set; }
        public RocketBoxUpdatedByInfo UpdatedBy { get; set; }
        public int OrderId { get; set; }
        public bool IsToPay { get; set; }
        public double ToPayAmount { get; set; }
        public bool IsCOD { get; set; }
        public object CODAmount { get; set; } // Change the type accordingly
        public string CODPaymentMode { get; set; }
        public List<string> SupportingDocs { get; set; }
        public DateTime InvoiceDate { get; set; }
        public object DeliverySlots { get; set; } // Change the type accordingly
        public object ChequeInFavorOf { get; set; } // Change the type accordingly
        public string WaybillNo { get; set; }
        public string LRN { get; set; }
        public List<string> ChildWaybillNos { get; set; }
        public object AppointmentDate { get; set; } // Change the type accordingly
        public bool IsAppointmentTaken { get; set; }
        public bool TrackingStatus { get; set; }
        public string Source { get; set; }
        public object APIError { get; set; } // Change the type accordingly
        public object OtherDetails { get; set; } // Change the type accordingly
        public string LabelURL { get; set; }
        public object SecureShipmentDetail { get; set; } // Change the type accordingly
    }

    public class RocketBoxPackageDetails
    {
        public int Units { get; set; }
        public double Weight { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Unit { get; set; }
    }

    public class RocketBoxModeInfo
    {
        public int Id { get; set; }
        public RocketBoxDeliveryPartnerInfo DeliveryPartner { get; set; }
        public string ModeName { get; set; }
        public string CommonName { get; set; }
        public bool IsActive { get; set; }
    }

    public class RocketBoxDeliveryPartnerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CommonName { get; set; }
        public bool IsPickupReady { get; set; }
        public bool IsPublic { get; set; }
        public bool IsShipmentReady { get; set; }
        public bool IsInsuranceAvailable { get; set; }
        public string CutOffTime { get; set; }
        public bool IsToPayAvailable { get; set; }
        public bool IsChequeAvailable { get; set; }
        public string Logo { get; set; }
        public bool IsDisable { get; set; }
        public bool IsListingReady { get; set; }
    }

    public class RocketBoxCreatedByInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class RocketBoxUpdatedByInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class RocketBoxClientInfo
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int RbPlanName { get; set; }
        public bool IsCourierReassignmentAllowed { get; set; }
        public object ClientSpoc { get; set; }
    }

    public class RocketBoxShipmentDetailsRes
    {
        public int Id { get; set; }
        public string WaybillNo { get; set; }
        public List<string> ChildWaybillNos { get; set; }
        public string Status { get; set; }
        public string StatusDp { get; set; }
        public DateTime PickupDateTime { get; set; }
        public DateTime? OriginalEdd { get; set; }
        public DateTime? Edd { get; set; }
        public string Add { get; set; }
        public RocketBoxModeInfo Mode { get; set; }
        public string ShipmentPurpose { get; set; }
        public int NoOfUnits { get; set; }
        public int? NoOfUnitsCarrier { get; set; }
        public string ApproxWeight { get; set; }
        public double InvoiceValue { get; set; }
        public double? InvoiceValueFromCarrier { get; set; }
        public bool IsInsured { get; set; }
        public string Remarks { get; set; }
        public object OtherDetails { get; set; }
        public string EwayBillNo { get; set; }
        public string InvoiceNumber { get; set; }
        public object ChargeableWeightCarrier { get; set; }
        public RocketBoxWarehouseInfo FromWarehouse { get; set; }
        public string SenderContactPersonName { get; set; }
        public bool IsToPay { get; set; }
        public string SenderContactPersonEmail { get; set; }
        public string SenderContactPersonContactNo { get; set; }
        public double ToPayAmount { get; set; }
        public RocketBoxWarehouseInfo ToWarehouse { get; set; }
        public string RecipientContactPersonName { get; set; }
        public string RecipientContactPersonEmail { get; set; }
        public string RecipientContactPersonContactNo { get; set; }
        public RocketBoxDeliveryPartnerInfo DeliveryPartner { get; set; }
        public RocketBoxClientInfo Client { get; set; }
        public RocketBoxCreatedByInfo CreatedBy { get; set; }
        public RocketBoxUpdatedByInfo UpdatedBy { get; set; }
        public List<RocketBoxPackageDetails> PackagingUnitDetails { get; set; }
        public object RecipientGST { get; set; }
        public string LabelURL { get; set; }
        public object PodURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public object ApiError { get; set; }
        public int OrderId { get; set; }
        public string Source { get; set; }
        public object PackageContent { get; set; }
        public object VolumetricWeight { get; set; }
        public string LRN { get; set; }
        public DateTime? ShipDate { get; set; }
        public List<string> SupportingDocs { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ShipmentType { get; set; }
        public object ChargeableWeightGarud { get; set; }
        public object DeliverySlots { get; set; }
        public object AppointmentDate { get; set; }
        public object NewAppointmentDate { get; set; }
        public bool IsDirect { get; set; }
        public object Pickup { get; set; }
        public bool IsAppointmentTaken { get; set; }
        public object Charges { get; set; }
        public List<string> SupportingDocLinks { get; set; }
        public object PoNo { get; set; }
        public object AppointmentEndDate { get; set; }
        public bool IsCOD { get; set; }
        public object CODAmount { get; set; }
        public string CODPaymentMode { get; set; }
        public object ChequeInFavorOf { get; set; }
        public bool TrackingStatus { get; set; }
        public string ClientOrderId { get; set; }
        public RocketBoxShipmentStatusDate ShipmentStatusDate { get; set; }
        public object WeightChargedByCarrierToRb { get; set; }
        public object IsWeightFreezed { get; set; }
        public int ChargedCft { get; set; }
        public object NewAppointmentEndDate { get; set; }
        public object PickupId { get; set; }
        public object RtoWaybillNo { get; set; }
        public object RtoShipmentId { get; set; }
    }

    public class RocketBoxWarehouseInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonContactNo { get; set; }
        public RocketBoxAddressInfo Address { get; set; }
    }

    public class RocketBoxShipmentStatusDate
    {
        public object OfpAt { get; set; }
        public object PickedUpAt { get; set; }
        public object OfdAt { get; set; }
        public object RadAt { get; set; }
        public object RtoInitiatedAt { get; set; }
        public object UndeliveredAt { get; set; }
        public object InTransitAt { get; set; }
        public object ShippedAt { get; set; }
        public object RtoDeliveredAt { get; set; }
        public object CancelledAt { get; set; }
        public object DeliveredAt { get; set; }
        public object DeliveryAttemptCount { get; set; }
        public object PickupAttemptCount { get; set; }
        public object Shipment { get; set; }
    }
    public class RocketBoxAddressInfo
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }


    public class RocketBoxOrderDetails
    {
        public int Id { get; set; }
        public DateTime PickupDateTime { get; set; }
        public int NoOfUnits { get; set; }
        public string Source { get; set; }
        public string ApproxWeight { get; set; }
        public double InvoiceValue { get; set; }
        public object VolumetricWeight { get; set; }
        public object InvoiceNumber { get; set; }
        public RocketBoxWarehouseInfo FromWarehouse { get; set; }
        public string SenderContactPersonName { get; set; }
        public string SenderContactPersonEmail { get; set; }
        public string SenderContactPersonContactNo { get; set; }
        public RocketBoxWarehouseInfo ToWarehouse { get; set; }
        public string RecipientContactPersonName { get; set; }
        public string RecipientContactPersonEmail { get; set; }
        public string RecipientContactPersonContactNo { get; set; }
        public RocketBoxClientInfo Client { get; set; }
        public RocketBoxCreatedByInfo CreatedBy { get; set; }
        public RocketBoxUpdatedByInfo UpdatedBy { get; set; }
        public string Status { get; set; }
        public List<RocketBoxPackageDetails> PackagingUnitDetails { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public object RecipientGST { get; set; }
        public bool IsToPay { get; set; }
        public bool IsCOD { get; set; }
        public object CODAmount { get; set; }
        public string CODPaymentMode { get; set; }
        public object ChequeInFavorOf { get; set; }
        public bool IsInsured { get; set; }
        public List<object> SupportingDocs { get; set; }
        public object InvoiceDate { get; set; }
        public List<object> DeliverySlots { get; set; }
        public object AppointmentDate { get; set; }
        public bool IsAppointmentTaken { get; set; }
        public bool IsAutoAssign { get; set; }
        public object SupportingDocLinks { get; set; }
        public string ModeName { get; set; }
        public object Pickup { get; set; }
        public int AutoSelectedModeId { get; set; }
        public object SecureShipmentDetail { get; set; }
        public string ClientOrderId { get; set; }
    }

    public class RocketBoxShipmentDetailsRequest
    {
        public string FromPincode { get; set; }
        public string FromCity { get; set; }
        public string FromState { get; set; }
        public string ToPincode { get; set; }
        public string ToCity { get; set; }
        public string ToState { get; set; }
        public int Quantity { get; set; }
        public double InvoiceValue { get; set; }
        public bool CalculatorPage { get; set; }
        public List<RocketBoxPackagingUnitDetails> PackagingUnitDetails { get; set; }
    }

    public class RBAdvantageRates
    {
        public string Logo { get; set; }
        public int Id { get; set; }
        public string CommonName { get; set; }
        public bool IsPublic { get; set; }
        public bool IsShipmentReady { get; set; }
        public bool IsPickupReady { get; set; }
        public string ModeName { get; set; }
        public int ModeId { get; set; }
        public string DeliveryPartner { get; set; }
        public bool IsRocketboxAccount { get; set; }
        public int Rates { get; set; }

        public RBAdvantageWorking Working { get; set; }

        public string TransporterId { get; set; }
        public string TransporterName { get; set; }
        public int Tat { get; set; }
        public string AvgDeliveryDays { get; set; }
        public string TotalShipment { get; set; }
    }

    public class RBAdvantageWorking
    {
        // Properties representing the working section
        // Add all the properties inside the "working" object here

        // Example:
        public string SenderCity { get; set; }
        public string SenderZone { get; set; }
        // ... other properties ...
        public double GrandTotal { get; set; }
        public string SecuredShipment { get; set; }
    }

    // Usage example:
    public class RBAdvantagesurface
    {
        public RBAdvantageRates RBAdvantageSurface { get; set; }
        public RBAdvantageRates RBAdvantageAir { get; set; }
    }
    public class WarehouseRequest
    {
        public string Name { get; set; }
        public int ClientId { get; set; }
        public WareAddress Address { get; set; }
        public string WarehouseCode { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonContactNo { get; set; }
    }

    public class WareAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
    public class DestWareAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string DestinationWarehouseName { get; set; }
        public string RecipientContactPersonName { get; set; }
        public string RecipientContactPersonEmail { get; set; }
        public string RecipientContactPersonContactNo { get; set; }
        public string RecipientGST { get; set; }
    }

    // Usage example:
    public class WarehouseCreationResponse
    {
        public WarehouseRequest WarehouseDetails { get; set; }
    }


    public class WarehouseListResponse
    {
        public string Next { get; set; }
        public string Previous { get; set; }
        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public List<GetWarehouse> Results { get; set; }
    }

    public class GetWarehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WareClientInfo Client { get; set; }
        public WareAddressInfo Address { get; set; }
        public bool ForOneTimeUse { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonContactNo { get; set; }
        public string WarehouseCode { get; set; }
        public WareCreatedByInfo CreatedBy { get; set; }
        public object UpdatedBy { get; set; }
    }

    public class WareClientInfo
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
    }

    public class WareAddressInfo
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Landmark { get; set; }
    }

    public class WareCreatedByInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    public class WarehouseList
    {
        public string Next { get; set; }
        public string Previous { get; set; }
        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public List<Warehouse> Results { get; set; }
    }

    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Client Client { get; set; }
        public WareAddress Address { get; set; }
        public bool ForOneTimeUse { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonContactNo { get; set; }
        public string DeliveryTimings { get; set; }
        public string WarehouseCode { get; set; }
        public CreatedBy CreatedBy { get; set; }
        public object UpdatedBy { get; set; }
    }

    public class Client
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
    }

    public class WarehouseUpdateRequest
    {
        public int WarehouseId { get; set; }
        public string name { get; set; }
        public int client_id { get; set; }
        public WareAddress address { get; set; }
        public string warehouse_code { get; set; }
        public string contact_person_name { get; set; }
        public string contact_person_email { get; set; }
        public string contact_person_contact_no { get; set; }
    }


}
