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
            INSERT INTO dbo.messages (id, message) 
            VALUES (@id, @message)";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(insertMessageQuery, connection))
            {
                command.Parameters.AddWithValue("@id", message.Id);
                command.Parameters.AddWithValue("@message", Encoding.UTF8.GetBytes(message.Text));

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
                        Message.BuildForDisplay(new Guid(reader[0].ToString()),Encoding.UTF8.GetString((byte[])reader[1]) );
                    }
                }
            }
        }
        return deletedMessage;
}

    public async Task<IEnumerable<Message>> GetMessages()
    {
        string getMessageQuery =
            @"USE andromeda_store;
            SELECT * FROM messages";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            connection.Open();

            List<Message> messages = new List<Message>();
            using (SqlCommand command = new SqlCommand(getMessageQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Message message = Message.BuildForDisplay(new Guid(reader[0].ToString()), 
                        Encoding.UTF8.GetString((byte[])reader[1]));
                        messages.Add(message);
                    }
                }
            }
            return messages;
        }
    }
}