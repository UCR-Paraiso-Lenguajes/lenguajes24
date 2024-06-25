using System;
using System.Collections.Generic;
using storeapi.Database;

namespace storeapi.Models
{
    public class Campanna
    {
        public int Id { get; set; }

        private string _contenidoHtml = string.Empty;
        private bool _estado;

        public string ContenidoHtml
        {
            get => _contenidoHtml;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ContenidoHtml must not be null or empty.");
                }
                if (value.Length > 5000)
                {
                    throw new ArgumentException("ContenidoHtml must not exceed 5000 characters.");
                }
                _contenidoHtml = value;
            }
        }

        public bool Estado
        {
            get => _estado;
            set => _estado = value;
        }

        public DateTime CreatedAt { get; set; }

        public Campanna(string contenidoHtml)
        {
            ContenidoHtml = contenidoHtml;
        }

        public Campanna() { }

        public static IEnumerable<Campanna> LoadCampannasFromDatabase()
        {
            IEnumerable<string[]> campannaData = CampannaDB.RetrieveCampannas();
            List<Campanna> campannas = new List<Campanna>();

            foreach (string[] row in campannaData)
            {
                if (ValidateCampannaRow(row))
                {
                    try
                    {
                        int id = int.Parse(row[0]);
                        string contenidoHtml = row[1];
                        DateTime createdAt = DateTime.Parse(row[2]);
                        bool estado = bool.Parse(row[3]);

                        Campanna campanna = new Campanna
                        {
                            Id = id,
                            ContenidoHtml = contenidoHtml,
                            CreatedAt = createdAt,
                            Estado = estado
                        };

                        campannas.Add(campanna);
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepción según sea necesario
                        Console.WriteLine($"Error processing row: {ex.Message}");
                    }
                }
            }

            return campannas;
        }

        private static bool ValidateCampannaRow(string[] row)
        {
            if (row.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(row[0], out _))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(row[1]) || row[1].Length > 5000)
            {
                return false;
            }

            if (!DateTime.TryParse(row[2], out _))
            {
                return false;
            }

            if (!bool.TryParse(row[3], out _))
            {
                return false;
            }

            return true;
        }
    }
}

