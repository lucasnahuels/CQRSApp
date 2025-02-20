using MediatR;

namespace CQRSCommand.Commands
{
    public class AddOrderCommand : IRequest<bool>
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public AddOrderCommand(string customerName, string productName, int quantity)
        {
            CustomerName = customerName;
            ProductName = productName;
            Quantity = quantity;
        }
    }
}
