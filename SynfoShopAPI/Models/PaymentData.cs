namespace SynfoShopAPI.Models
{
    public class PaymentResponse
    {
        public int OrderId { get; set; }
        public double PayAmount { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }

    public class PaymentData
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public double PayAmount { get; set; }
    }
}
