using CQRSQuery.Models;
using CQRSQuery.Queries;
using MediatR;
using Newtonsoft.Json;

namespace CQRSQuery.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<Order>>
    {
        private readonly HttpClient _httpClient;

        public GetOrdersQueryHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("https://upstream-api.com/orders");

            if (!response.IsSuccessStatusCode)
            {
                return null; // Or handle errors appropriately
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<Order>>(responseContent);

            return orders;
        }
    }
}
