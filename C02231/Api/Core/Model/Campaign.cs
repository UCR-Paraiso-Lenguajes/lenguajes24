using StoreAPI.Database;

namespace StoreAPI.models;



public class Campaign
{
    public int Id { get; set; }
    public string Update { get; set; }
    public DateTime Timestamp { get; set; }

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

                    Campaign campanna = new Campaign
                    {
                        Id = id,
                        Update = update,
                        Timestamp = dateTime
                    };

                    campannas.Add(campanna);
                }
                else
                {
                    // Log o manejar filas inválidas
                }
            }
            catch (Exception ex)
            {
                // Log o manejar la excepción, e.g., throw new Exception("Invalid row format", ex);
            }
        }

        return campannas;
    }

    private static bool ValidateCampannaRow(string[] row)
    {
        if (row.Length != 3)
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

        return true;
    }
}
