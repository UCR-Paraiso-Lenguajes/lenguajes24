using MediatR;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Repositories;
using System.Text.Json;

namespace StoreApi.Handler
{
    public sealed class CreateSalesHandler : IRequestHandler<CreateSalesCommand, Sales>
    {
        private readonly ISalesRepository _salesRepository;
        private HttpClient client = new HttpClient();
        private string apiUrl = "https://us1.locationiq.com/v1/search";
        private string key = "pk.907e2c27b4df9c13243827db78332485";
        public CreateSalesHandler(ISalesRepository salesRepository)
        {
            if (salesRepository == null)
            {
                throw new ArgumentException("Illegal action, salesRepository is invalid.");
            }
            _salesRepository = salesRepository;
        }

        public async Task<Sales> Handle(CreateSalesCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var sales = new Sales()
            {
                Date = command.Date,
                Confirmation = command.Confirmation,
                PaymentMethod = command.PaymentMethods,
                Total = command.Total,
                Address = command.Address,
                PurchaseNumber = command.PurchaseNumber
            };

            return await _salesRepository.AddSalesAsync(sales);
        }
        private void ValidateCommand(CreateSalesCommand command)
        {
            if (command.Date == DateTime.MinValue)
            {
                throw new ArgumentException("The Date cannot be empty.");
            }
            if (command.Confirmation != 0 && command.Confirmation != 1)
            {
                throw new ArgumentException("Confirmation error.");
            }
            if (string.IsNullOrWhiteSpace(command.PaymentMethods))
            {
                throw new ArgumentException("The Paymet Methods cannot be empty.");
            }
            if (command.Total < 0)
            {
                throw new ArgumentException("The total must be greater than zero.");
            }
            if (ValidateAddressAsync(command.Address).Result == false)
            {
                throw new ArgumentException("The address is invalid.");
            }
        }

        private async Task<bool> ValidateAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("The address is null or empty.");
            }

            try
            {
                string encodedAddress = Uri.EscapeDataString(address);
                string requestUrl = $"{apiUrl}?q={encodedAddress}&limit=10&key={key}&format=json";

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var locations = await JsonSerializer.DeserializeAsync<LocationData[]>(responseStream, options);

                    if (locations.Any(l => l.Display_name == address) || locations.Length <= 3)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ArgumentException("Error in HTTP request when validating address.", ex);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Error deserializing JSON when validating address.", ex);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error validating address.", ex);
            }
        }
    }
}