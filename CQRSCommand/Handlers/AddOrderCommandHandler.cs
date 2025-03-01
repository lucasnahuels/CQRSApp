using CQRSCommand.Commands;
using CQRSCommand.Database;
using CQRSCommand.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSCommand.Handlers
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, bool>
    {
        private readonly ApplicationDbContext _writeDbContext;
        private readonly MongoDbContext _readDbContext;

        public AddOrderCommandHandler(ApplicationDbContext writeDbContext, MongoDbContext readDbContext)
        {
            _writeDbContext = writeDbContext;
            _readDbContext = readDbContext;
        }

        public async Task<bool> Handle(AddOrderCommand command, CancellationToken cancellationToken)
        {
            var customer = await _writeDbContext.Customer
                .FirstOrDefaultAsync(c => c.Id == command.CustomerId, cancellationToken);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            var product = await _writeDbContext.Product
                .FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var orderDate = DateTime.UtcNow;
            var order = new Order
            {
                CustomerId = command.CustomerId,
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                OrderDate = orderDate
            };
            var orderQuery = new OrderQuery
            {
                //Id is generated automatically by MongoDB
                CustomerName = customer.Name,
                ProductName = product.Name,
                Quantity = command.Quantity,
                OrderDate = orderDate
            };

            try
            {
                // Save to SQL database
                _writeDbContext.Orders.Add(order);
                await _writeDbContext.SaveChangesAsync(cancellationToken);

                // Save to MongoDB
                await _readDbContext.Orders.InsertOneAsync(orderQuery, cancellationToken: cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                // Two-Phase Commit (2pc)
                // Rollback SQL changes if MongoDB insert fails
                _writeDbContext.Orders.Remove(order);
                await _writeDbContext.SaveChangesAsync(cancellationToken);

                // Handle exceptions appropriately
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
