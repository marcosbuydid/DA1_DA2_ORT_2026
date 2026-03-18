
using AdapterPattern.PaymentGateway.Adaptee;
using AdapterPattern.PaymentGateway.Target;

namespace AdapterPattern.PaymentGateway.Adapter
{
    public class EthereumPaymentGateway : IPaymentProcessor
    {
        private readonly EthereumPaymentService _ethereumPaymentService;
        private readonly string _walletAddress;

        public EthereumPaymentGateway(EthereumPaymentService ethereumPaymentService, string walletAddress)
        {
            _ethereumPaymentService = ethereumPaymentService;
            _walletAddress = walletAddress;
        }

        public void ProcessPayment(decimal amount)
        {
            string txHash = _ethereumPaymentService.SendTransaction(amount, _walletAddress);

            Console.WriteLine($"Ethereum TX sent: {txHash}");

            Console.WriteLine("Waiting for ethereum confirmation...");

            bool paymentConfirmed = _ethereumPaymentService.WaitForConfirmation(txHash);

            if (paymentConfirmed)
                Console.WriteLine("Payment confirmed.");
            else
                Console.WriteLine("Payment failed.");
        }
    }
}
