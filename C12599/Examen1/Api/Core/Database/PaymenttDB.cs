using System;
using System.Collections.Generic;
using MySqlConnector;
using core;
//PROYECTO

namespace storeapi.Database
{
    public sealed class PaymentDB
    {
        public PaymentDB()
        {
        }

        public static void CreateMysql()
        {
            using (var connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS paymentMethods (
                        id INT PRIMARY KEY,
                        name VARCHAR(20),
                        isActive BOOLEAN DEFAULT TRUE
                    )";
                using (var createTableCommand = new MySqlCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }

            InsertInitialPaymentMethods();
        }

        private static void InsertInitialPaymentMethods()
        {
            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string insertCashQuery = @"
                    INSERT IGNORE INTO paymentMethods (id, name, isActive)
                    VALUES (0, 'CASH', TRUE)";

                using (var insertCashCommand = new MySqlCommand(insertCashQuery, connection))
                {
                    insertCashCommand.ExecuteNonQuery();
                }

                string insertSinpeQuery = @"
                    INSERT IGNORE INTO paymentMethods (id, name, isActive)
                    VALUES (1, 'SINPE', TRUE)";

                using (var insertSinpeCommand = new MySqlCommand(insertSinpeQuery, connection))
                {
                    insertSinpeCommand.ExecuteNonQuery();
                }
            }
        }

        public static List<string[]> RetrievePaymentMethods()
        {
            List<string[]> paymentMethods = new List<string[]>();

            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string sql = "SELECT id, name, isActive FROM paymentMethods";

                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string[] methodInfo = new string[3];
                            methodInfo[0] = reader["id"].ToString();
                            methodInfo[1] = reader["name"].ToString();
                            methodInfo[2] = reader["isActive"].ToString();
                            paymentMethods.Add(methodInfo);
                        }
                    }
                }
            }

            return paymentMethods;
        }

        public static void UpdatePaymentMethodStatus(int id, bool isActive)
        {
            using (var connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string updateQuery = @"
                    UPDATE paymentMethods
                    SET isActive = @isActive
                    WHERE id = @id";

                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@isActive", isActive);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
