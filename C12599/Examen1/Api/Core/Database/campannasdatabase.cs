using System;
using System.Collections.Generic;
using MySqlConnector;
using storeapi.Models;
using core;

namespace storeapi.Database
{
    public sealed class CampannaDB
    {
        public CampannaDB() { }

        public static void CreateMysql()
        {
            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Campannas (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        ContenidoHtml TEXT NOT NULL,
                        CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB";

                using (var createTableCommand = new MySqlCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        public static void InsertCampanna(Campanna campanna)
        {
            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO Campannas (ContenidoHtml)
                    VALUES (@ContenidoHtml)";

                using (var insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ContenidoHtml", campanna.ContenidoHtml);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public static List<string[]> RetrieveCampannas()
        {
            List<string[]> campannas = new List<string[]>();

            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string sql = "SELECT Id, ContenidoHtml, CreatedAt FROM Campannas";

                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int fieldCount = reader.FieldCount;
                            string[] row = new string[fieldCount];
                            for (int i = 0; i < fieldCount; i++)
                            {
                                row[i] = reader.GetValue(i).ToString();
                            }
                            campannas.Add(row);
                        }
                    }
                }
            }

            return campannas;
        }
    }
}
