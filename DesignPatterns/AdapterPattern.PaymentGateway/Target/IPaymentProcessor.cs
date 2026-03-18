
namespace AdapterPattern.PaymentGateway.Target
{
    public interface IPaymentProcessor
    {
        void ProcessPayment(decimal amount);
    }
}
