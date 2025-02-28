using MediatR;

namespace CQRSCommand.Commands
{
    public class AddOrderCommand : IRequest<bool>
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public AddOrderCommand(int customerId, int productId, int quantity)
        {
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
