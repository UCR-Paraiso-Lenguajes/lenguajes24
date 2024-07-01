using KEStoreApi.Models;
using Microsoft.AspNetCore.SignalR;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MessageDatabase
    {
        private readonly string _connectionString;
        private readonly IHubContext<MarketingHub> _hubContext;

        public MessageDatabase(IHubContext<MarketingHub> hubContext)
        {
            _connectionString = DatabaseConfiguration.Instance.ConnectionString;
            _hubContext = hubContext;
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            message.Timestamp = DateTime.UtcNow;

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO messages (title, content, timestamp, is_deleted) VALUES (@title, @content, @timestamp, 0); SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@title", message.Title);
                        command.Parameters.AddWithValue("@content", message.Content);
                        command.Parameters.AddWithValue("@timestamp", message.Timestamp);
                        var result = await command.ExecuteScalarAsync();
                        message.Id = Convert.ToInt32(result);
                    }
                }

                var recentMessages = await GetRecentMessagesAsync(3);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", recentMessages);
                return message;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the message.", ex);
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(int limit = 10)
        {
            List<Message> messages = new List<Message>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM messages WHERE is_deleted = 0 ORDER BY timestamp DESC LIMIT @limit";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@limit", limit);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                messages.Add(new Message
                                {
                                    Id = reader.GetInt32("id"),
                                    Title = reader.GetString("title"),
                                    Content = reader.GetString("content"),
                                    Timestamp = reader.GetDateTime("timestamp")
                                });
                            }
                        }
                    }
                }

                return messages;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving messages.", ex);
            }
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            Message message = null;

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM messages WHERE id = @id AND is_deleted = 0";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                message = new Message
                                {
                                    Id = reader.GetInt32("id"),
                                    Title = reader.GetString("title"),
                                    Content = reader.GetString("content"),
                                    Timestamp = reader.GetDateTime("timestamp")
                                };
                            }
                        }
                    }
                }

                return message;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the message.", ex);
            }
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "UPDATE messages SET is_deleted = 1 WHERE id = @id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            return false;
                        }
                    }
                }

                await _hubContext.Clients.All.SendAsync("RemoveMessage", id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the message.", ex);
            }
        }

        private async Task<IEnumerable<Message>> GetRecentMessagesAsync(int limit)
        {
            List<Message> messages = new List<Message>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM messages WHERE is_deleted = 0 ORDER BY timestamp DESC LIMIT @limit";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@limit", limit);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                messages.Add(new Message
                                {
                                    Id = reader.GetInt32("id"),
                                    Title = reader.GetString("title"),
                                    Content = reader.GetString("content"),
                                    Timestamp = reader.GetDateTime("timestamp")
                                });
                            }
                        }
                    }
                }

                return messages;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving recent messages.", ex);
            }
        }
    }
}
