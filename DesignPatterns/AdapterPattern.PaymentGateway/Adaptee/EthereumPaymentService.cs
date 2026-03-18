
namespace AdapterPattern.PaymentGateway.Adaptee
{
    public class EthereumPaymentService
    {
        public string SendTransaction(decimal amount, string walletAddress)
        {
            return $"ETH_TX_{Guid.NewGuid()}";
        }

        public bool WaitForConfirmation(string transactionHash)
        {
            Console.WriteLine("Waiting for ethereum confirmation...");

            Thread.Sleep(1000); // simulate delay

            // simulate success/failure
            return new Random().Next(0, 2) == 1;
        }
    }
}
