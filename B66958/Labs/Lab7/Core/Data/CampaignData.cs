using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ApiLab7;

public class CampaignData
{
    public async Task InsertCampaignAsync(Campaign campaign)
    {
        string insertCampaignQuery =
            @"USE andromeda_store;
            INSERT INTO dbo.campaigns (id, message, date, enabled) 
            VALUES (@id, @message, @date, 1)";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(insertCampaignQuery, connection))
            {
                command.Parameters.AddWithValue("@id", campaign.Id);
                command.Parameters.AddWithValue("@message", Encoding.UTF8.GetBytes(campaign.Text));
                command.Parameters.AddWithValue("@date", campaign.SentAt);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Campaign> DeleteCampaignAsync(string id)
    {
        string disableCampaignQuery =
            @"
            USE andromeda_store;
            UPDATE campaigns 
            SET enabled = 0
            OUTPUT INSERTED.*
            WHERE id = @id";

        Campaign updatedCampaign = null;

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(disableCampaignQuery, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        updatedCampaign = Campaign.BuildForDisplay(
                            new Guid(reader[0].ToString()),
                            Encoding.UTF8.GetString((byte[])reader[1]),
                            reader.GetDateTime(2),
                            reader.GetBoolean(3)
                        );
                    }
                }
            }
        }
        return updatedCampaign;
    }

    public async Task<IEnumerable<Campaign>> GetCampaignsAsync()
    {
        string getCampaignsQuery =
            @"USE andromeda_store;
            SELECT id, message, date, enabled FROM campaigns";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();

            List<Campaign> campaigns = new List<Campaign>();
            using (SqlCommand command = new SqlCommand(getCampaignsQuery, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Campaign campaign = Campaign.BuildForDisplay(
                            new Guid(reader[0].ToString()),
                            Encoding.UTF8.GetString((byte[])reader[1]),
                            reader.GetDateTime(2),
                            reader.GetBoolean(3)
                        );
                        campaigns.Add(campaign);
                    }
                }
            }
            return campaigns;
        }
    }

    public async Task<IEnumerable<Campaign>> GetLastThreeCampaignsAsync()
    {
        string query =
            @"
            USE andromeda_store;
            SELECT TOP 3 id, message, date, enabled
            FROM campaigns
            WHERE enabled = 1
            ORDER BY date DESC";

        var campaigns = new List<Campaign>();

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Campaign campaign = Campaign.BuildForDisplay(
                            new Guid(reader[0].ToString()),
                            Encoding.UTF8.GetString((byte[])reader[1]),
                            reader.GetDateTime(2),
                            reader.GetBoolean(3)
                        );
                        campaigns.Add(campaign);
                    }
                }
            }
        }

        return campaigns;
    }
}
