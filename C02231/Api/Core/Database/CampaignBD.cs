using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using StoreAPI.models;
using Core;

namespace StoreAPI.Database
{
    public sealed class CampaignDB
    {
        public async Task SaveAsync(Campaign campaign)
        {
            if (campaign == null) throw new ArgumentNullException($"The {nameof(campaign)} object cannot be null.");

            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string insertQuery = @"
                            INSERT INTO messages (content, timestamp)
                            VALUES (@content, @timestamp);";

                        using (var command = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@content", campaign.Update);
                            command.Parameters.AddWithValue("@timestamp", campaign.Timestamp);

                            await command.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch (MySqlException ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Error saving the campaign in the database. MySQL Error: " + ex.Message, ex);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Error saving the campaign in the database. General Error: " + ex.Message, ex);
                    }
                }
            }
        }
        public async Task<List<string[]>> GetLatestMessagesAsync()
        {
            List<string[]> messages = new List<string[]>();

            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                string selectQuery = "SELECT id, content, timestamp FROM messages ORDER BY timestamp DESC LIMIT 3;";

                using (var command = new MySqlCommand(selectQuery, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int fieldCount = reader.FieldCount;
                        string[] row = new string[fieldCount];
                        for (int i = 0; i < fieldCount; i++)
                            {
                                row[i] = reader.GetValue(i).ToString();
                            }
                        
                        messages.Add(row);
                    }
                }
            }

            return messages;
        }

    }
}