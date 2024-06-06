using System;
using System.Collections.Generic;
using MySqlConnector;
using core;

namespace storeapi.Database
{
    public sealed class PaymentDB
    {


        public PaymentDB()
        {
        }

        public static void CreateMysql()
        {


            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionStringMyDb))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS paymentMethods (
                        id INT PRIMARY KEY,
                        name VARCHAR(20)
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


            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionStringMyDb))
            {
                connection.Open();

                // Insertar método de pago CASH si no existe
                string insertCashQuery = @"
                    INSERT IGNORE INTO paymentMethods (id, name)
                    VALUES (0, 'CASH')";

                using (var insertCashCommand = new MySqlCommand(insertCashQuery, connection))
                {
                    insertCashCommand.ExecuteNonQuery();
                }

                // Insertar método de pago SINPE si no existe
                string insertSinpeQuery = @"
                    INSERT IGNORE INTO paymentMethods (id, name)
                    VALUES (1, 'SINPE')";

                using (var insertSinpeCommand = new MySqlCommand(insertSinpeQuery, connection))
                {
                    insertSinpeCommand.ExecuteNonQuery();
                }
            }
        }

        public static List<string[]> RetrievePaymentMethods()
        {
            List<string[]> paymentMethods = new List<string[]>();

            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionStringMyDb))
            {
                connection.Open();

                string sql = "SELECT id, name FROM paymentMethods";

                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string[] methodInfo = new string[2];
                            methodInfo[0] = reader["id"].ToString();
                            methodInfo[1] = reader["name"].ToString();
                            paymentMethods.Add(methodInfo);
                        }
                    }
                }
            }

            return paymentMethods;
        }
    }
}
