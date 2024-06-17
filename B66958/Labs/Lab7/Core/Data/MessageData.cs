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
    public void InsertMessageAsync(Message message){
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
                command.Parameters.AddWithValue("@message", message.Message);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}