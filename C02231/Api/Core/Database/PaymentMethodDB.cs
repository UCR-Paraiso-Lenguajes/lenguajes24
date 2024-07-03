using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using StoreAPI.models;
using Core;

namespace StoreAPI.Database
{
    public sealed class PaymentMethodDB
    {
        public async Task<IEnumerable<PayMethod>> GetActivePaymentMethodsAsync()
        {
            var paymentMethods = new List<PayMethod>();

            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id, method_name, active FROM paymentMethod WHERE active = 1";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var paymentMethod = new PayMethod
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("method_name"),
                            Active = reader.GetInt32("active")
                        };
                        paymentMethods.Add(paymentMethod);
                    }
                }
            }

            return paymentMethods;
        }

        public async Task<bool> DisablePaymentMethodAsync(int id)
        {
            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE paymentMethod SET active = 0 WHERE id = @Id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> TogglePaymentMethodAsync(int id, bool active)
        {
            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE paymentMethod SET active = @Active WHERE id = @Id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Active", active);
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> IsPaymentMethodActiveAsync(int paymentMethodId)
        {
            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT active FROM paymentMethod WHERE id = @Id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", paymentMethodId);
                    var result = await command.ExecuteScalarAsync();
                    if (result != null && int.TryParse(result.ToString(), out int active))
                    {
                        return active == 1;
                    }
                    return false;
                }
            }
        }


    }
}
