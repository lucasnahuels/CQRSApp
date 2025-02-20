using CQRSCommand.Commands;
using CQRSCommand.Database;
using CQRSCommand.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

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
                CustomerName = command.CustomerName,
                ProductName = command.ProductName,
                Quantity = command.Quantity
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
