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

            using var transaction = await _writeDbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Save to SQL database
                _writeDbContext.Orders.Add(order);
                var sqlSaveTask = _writeDbContext.SaveChangesAsync(cancellationToken);

                // Save to MongoDB
                var mongoSaveTask = _readDbContext.Orders.InsertOneAsync(orderQuery, cancellationToken: cancellationToken);

                // Wait for both tasks to complete
                await Task.WhenAll(sqlSaveTask, mongoSaveTask);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                // Using a distributed transaction would be ideal to ensure that both databases are updated consistently at the same time.
                // Howerver, since we're dealing with two different databases (SQL and MongoDB), achieving a distributed transaction can be complex.
                // One approach would be to use a two-phase commit (2PC) protocol,
                // but it can be challenging to implement and may not be supported out-of-the-box by all databases. 
                // (Transactions are not handle in MongoDb standalone servers. It needs a replica set)

                // Rollback SQL changes if MongoDB insert fails
                await transaction.RollbackAsync(cancellationToken);

                // Handle exceptions appropriately
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
