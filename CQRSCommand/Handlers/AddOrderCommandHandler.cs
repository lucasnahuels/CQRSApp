using CQRSCommand.Commands;
using MediatR;
using Newtonsoft.Json;
using System.Text;

namespace CQRSCommand.Handlers
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, bool>
    {
        private readonly HttpClient _httpClient;

        public AddOrderCommandHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Handle(AddOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new
            {
                customerName = command.CustomerName,
                productName = command.ProductName,
                quantity = command.Quantity
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(order),
                Encoding.UTF8,
                "application/json");

            // Call an upstream API to add the order
            var response = await _httpClient.PostAsync("https://upstream-api.com/orders", content);

            return response.IsSuccessStatusCode;
        }
    }
}
