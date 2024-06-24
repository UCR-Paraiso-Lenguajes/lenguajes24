using StoreAPI.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAPI.models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Update { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Active { get; set; } // Asegúrate de tener esta propiedad para reflejar el estado activo

        public Campaign(string update)
        {
            Update = update;
            Timestamp = DateTime.UtcNow;
        }

        // Constructor sin parámetros para la serialización/deserialización
        public Campaign() { }

        public static async Task<IEnumerable<Campaign>> LoadCampannasFromDatabaseAsync()
        {
            var campaignDB = new CampaignDB();
            List<string[]> campannaData = await campaignDB.GetLatestMessagesAsync();
            List<Campaign> campannas = new List<Campaign>();

            foreach (string[] row in campannaData)
            {
                try
                {
                    if (ValidateCampannaRow(row))
                    {
                        int id = int.Parse(row[0]);
                        string update = row[1];
                        DateTime dateTime = DateTime.Parse(row[2]);
                        bool active = bool.Parse(row[3]); // Asegúrate de tener la columna 'active' en tu consulta SQL

                        if (active) // Solo agregar campañas activas a la lista
                        {
                            Campaign campanna = new Campaign
                            {
                                Id = id,
                                Update = update,
                                Timestamp = dateTime,
                                Active = active // Asegúrate de que la clase Campaign tenga una propiedad 'Active'
                            };

                            campannas.Add(campanna);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid campaign row");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid row format", ex);
                }
            }

            return campannas;
        }

        private static bool ValidateCampannaRow(string[] row)
        {
            if (row.Length != 4) // Asegúrate de que el número de columnas coincida con tu consulta SQL
            {
                return false;
            }

            if (!int.TryParse(row[0], out _))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(row[1]))
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
