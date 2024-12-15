namespace SatGetsin2.Service.Dtos.User
{
    public class PaymentDto
    {
        public string PaymentMethodId { get; set; }
        public string? UserId { get; set; }
        public double Amount { get; set; }
    }
}
