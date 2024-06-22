using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ApiLab7;

public class MessageData
{
    public async Task InsertMessageAsync(Message message){
        string insertMessageQuery =
            @"USE andromeda_store;
            INSERT INTO dbo.messages (id, message, date) 
            VALUES (@id, @message, @date)";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(insertMessageQuery, connection))
            {
                command.Parameters.AddWithValue("@id", message.Id);
                command.Parameters.AddWithValue("@message", Encoding.UTF8.GetBytes(message.Text));
                command.Parameters.AddWithValue("@date", message.SentAt);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Message> DeleteMessageAsync(string id)
    {
        string deleteMessageQuery = @"
            USE andromeda_store;
            DELETE FROM messages 
            OUTPUT DELETED.*
            WHERE id = @id";

        Message deletedMessage = null;

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(deleteMessageQuery, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        deletedMessage = 
                        Message.BuildForDisplay(new Guid(reader[0].ToString()),
                        Encoding.UTF8.GetString((byte[])reader[1]),
                        reader.GetDateTime(2));
                    }
                }
            }
        }
        return deletedMessage;
}

    public async Task<IEnumerable<Message>> GetMessagesAsync()
    {
        string getMessageQuery =
            @"USE andromeda_store;
            SELECT * FROM messages";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();

            List<Message> messages = new List<Message>();
            using (SqlCommand command = new SqlCommand(getMessageQuery, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Message message = Message.BuildForDisplay(new Guid(reader[0].ToString()), 
                        Encoding.UTF8.GetString((byte[])reader[1]), reader.GetDateTime(2));
                        messages.Add(message);
                    }
                }
            }
            return messages;
        }
    }

    public async Task<IEnumerable<Message>> GetLastThreeMessagesAsync()
    {
        string query = @"
            USE andromeda_store;
            SELECT TOP 3 *
            FROM messages
            ORDER BY date DESC";

        var messages = new List<Message>();

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Message message = Message.BuildForDisplay(new Guid(reader[0].ToString()), 
                        Encoding.UTF8.GetString((byte[])reader[1]), reader.GetDateTime(2));
                        messages.Add(message);
                    }
                }
            }
        }

        return messages;
    }
}