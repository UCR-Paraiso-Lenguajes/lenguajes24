using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using core;

namespace storeapi.Database
{
    public sealed class SalesDB
    {
        public static async Task<IEnumerable<string[]>> GetForWeekAsync(string date)
        {
             await using (MySqlConnection connection = new MySqlConnection(DataConnection.Instance.ConnectionStringMyDb))
            {
                await connection.OpenAsync();

                DateTime startDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime startOfWeek = startDate.AddDays(-(int)startDate.DayOfWeek);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                string formattedStartOfWeek = startOfWeek.ToString("yyyy-MM-dd");
                string formattedEndOfWeek = endOfWeek.ToString("yyyy-MM-dd");

                string sql = @"
                    SELECT *
                    FROM Compras
                    WHERE date >= @startOfWeek AND date <= @endOfWeek";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@startOfWeek", formattedStartOfWeek);
                    command.Parameters.AddWithValue("@endOfWeek", formattedEndOfWeek);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var databaseInfo = new List<string[]>();

                        while (await reader.ReadAsync())
                        {
                            int fieldCount = reader.FieldCount;
                            string[] row = new string[fieldCount];

                            for (int i = 0; i < fieldCount; i++)
                            {
                                row[i] = reader.GetValue(i).ToString();
                            }

                            databaseInfo.Add(row);
                        }

                        return databaseInfo;
                    }
                }
            }
        }

        public static async Task<IEnumerable<string[]>> GetForDayAsync(string date)
        {
            DateTime startDate;

            if (!DateTime.TryParse(date, out startDate))
            {
                throw new ArgumentException("Fecha no válida", nameof(date));
            }

            string connectionString = "Server=localhost;Database=lab;Uid=root;Pwd=123456;";

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string sql = @"
                    SELECT *
                    FROM Compras
                    WHERE DATE(date) = DATE(@startDate)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    // Asigna el parámetro @startDate en el formato adecuado
                    command.Parameters.AddWithValue("@startDate", startDate);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var databaseInfo = new List<string[]>();

                        while (await reader.ReadAsync())
                        {
                            int fieldCount = reader.FieldCount;
                            string[] row = new string[fieldCount];

                            for (int i = 0; i < fieldCount; i++)
                            {
                                row[i] = reader.GetValue(i).ToString();
                            }

                            databaseInfo.Add(row);
                        }

                        return databaseInfo;
                    }
                }
            }
        }
    }
}

