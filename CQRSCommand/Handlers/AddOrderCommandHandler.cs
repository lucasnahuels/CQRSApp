using CQRSCommand.Commands;
using CQRSCommand.Database;
using CQRSCommand.Models;
using MediatR;

namespace CQRSCommand.Handlers
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public AddOrderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                CustomerId = command.CustomerId,
                ProductId = command.ProductId,
                Quantity = command.Quantity
            };

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }
    }
}
