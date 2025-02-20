using CQRSQuery.Models;
using MediatR;

namespace CQRSQuery.Queries
{
    public class GetOrdersQuery: IRequest<List<Order>>
    {
    }
}
