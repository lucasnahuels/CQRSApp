using CQRSQuery.Database;
using CQRSQuery.Models;
using CQRSQuery.Queries;
using MediatR;
using MongoDB.Driver;

namespace CQRSQuery.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderQuery>>
    {
        private readonly MongoDbContext _context;

        public GetOrdersQueryHandler(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderQuery>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Find(_ => true).ToListAsync(cancellationToken);
            return orders;
        }
    }
}
