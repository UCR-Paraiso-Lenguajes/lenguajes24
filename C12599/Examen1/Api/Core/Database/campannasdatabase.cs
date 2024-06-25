//investigacion
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
            using (var connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Campannas (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        ContenidoHtml TEXT NOT NULL,
                        estado BOOLEAN,
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
                    INSERT INTO Campannas (ContenidoHtml, estado)
                    VALUES (@ContenidoHtml, @Estado)";

                using (var insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ContenidoHtml", campanna.ContenidoHtml);
                    insertCommand.Parameters.AddWithValue("@Estado", campanna.Estado);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateCampannaEstado(int id, bool estado)
        {
            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string updateQuery = @"
                    UPDATE Campannas
                    SET estado = @Estado
                    WHERE Id = @Id";

                using (var updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Estado", estado);
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public static List<string[]> RetrieveCampannas()
        {
            List<string[]> campannas = new List<string[]>();

            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string sql = "SELECT Id, ContenidoHtml, CreatedAt, estado FROM Campannas WHERE estado = true";

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

        public static Campanna GetCampannaById(int id)
        {
            Campanna campanna = null;

            if (id <= 0)
            {
                throw new ArgumentException("ID must be a positive number.");
            }

            using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionString))
            {
                connection.Open();

                string sql = "SELECT Id, ContenidoHtml, CreatedAt, estado FROM Campannas WHERE Id = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            campanna = new Campanna
                            {
                                Id = reader.GetInt32(0),
                                ContenidoHtml = reader.GetString(1),
                                CreatedAt = reader.GetDateTime(2),
                                Estado = reader.GetBoolean(3)
                            };
                        }
                    }
                }
            }

            return campanna;
        }
    }
}
