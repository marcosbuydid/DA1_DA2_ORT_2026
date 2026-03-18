using AdapterPattern.PaymentGateway.Adaptee;
using AdapterPattern.PaymentGateway.Adapter;
using AdapterPattern.PaymentGateway.Target;

namespace AdapterPattern.PaymentGateway
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EthereumPaymentService ethereumPaymentService = new EthereumPaymentService();

            IPaymentProcessor processor =
                new EthereumPaymentGateway(ethereumPaymentService, "user-wallet-SDF76CVNJ87");

            processor.ProcessPayment(159.45m);
        }
    }
}
