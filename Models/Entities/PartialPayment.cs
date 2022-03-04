namespace UserPaymentsDesktopApp.Models.Entities
{
    public partial class PaymentOfUser
    {
        public decimal Sum => Price * Count;
    }
}
