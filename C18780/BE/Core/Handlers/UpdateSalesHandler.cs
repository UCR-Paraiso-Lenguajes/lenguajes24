using System.Text.Json;
using MediatR;
using StoreApi.Commands;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Handler
{
    public sealed class UpdateSalesHandler : IRequestHandler<UpdateSalesCommand, int>
    {
        private readonly ISalesRepository _salesRepository;
        private HttpClient client = new HttpClient();
        private string apiUrl = "https://us1.locationiq.com/v1/search";
        private string key = "pk.907e2c27b4df9c13243827db78332485";
        public UpdateSalesHandler(ISalesRepository salesRepository)
        {
            if (salesRepository == null)
            {
                throw new ArgumentException("Illegal action, salesRepository is invalid.");
            }
            _salesRepository = salesRepository;
        }
        public async Task<int> Handle(UpdateSalesCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);
            var sales = await _salesRepository.GetSalesByIdAsync(command.Uuid);
            if (sales == null)
                return default;

            sales.Date = command.Date;
            sales.Confirmation = command.Confirmation;
            sales.PaymentMethod = command.PaymentMethods;
            sales.Total = command.Total;
            sales.Address = command.Address;
            sales.PurchaseNumber = command.PurchaseNumber;


            return await _salesRepository.UpdateSalesAsync(sales);
        }
        private void ValidateCommand(UpdateSalesCommand command)
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
            if (string.IsNullOrWhiteSpace(command.Address))
            {
                throw new ArgumentException("The address cannot be empty.");
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