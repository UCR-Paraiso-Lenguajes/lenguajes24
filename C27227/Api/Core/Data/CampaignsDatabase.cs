using Core;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace  KEStoreApi.Data
{
    public sealed class CampaignsDatabase
    {
        public async Task<CampaignMessage> InsertCampaignAsync(CampaignMessage campaignMessage)
        {
            using (var connection = new MySqlConnection(DatabaseConfiguration.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        var insertQuery = @"INSERT INTO Campaign (Title, ContentCam, DateCam, IsDeleted) 
                                            VALUES (@Title, @ContentCam, @DateCam, @IsDeleted);
                                            SELECT LAST_INSERT_ID();";
                        using (var insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@Title", campaignMessage.Title);
                            insertCommand.Parameters.AddWithValue("@ContentCam", campaignMessage.Content);
                            insertCommand.Parameters.AddWithValue("@DateCam", campaignMessage.Date);
                            insertCommand.Parameters.AddWithValue("@IsDeleted", campaignMessage.IsDeleted);
                            var newId = Convert.ToInt32(await insertCommand.ExecuteScalarAsync());
                            campaignMessage.Id = newId;
                            await transaction.CommitAsync();
                        }
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            return campaignMessage;
        }

        public async Task<IEnumerable<CampaignMessage>> GetAllCampaignsAsync()
        {
            var campaigns = new List<CampaignMessage>();
            using (var connection = new MySqlConnection(DatabaseConfiguration.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT Id, Title, ContentCam, DateCam, IsDeleted FROM Campaign WHERE IsDeleted = 0";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        campaigns.Add(new CampaignMessage(
                            reader.GetInt32("Id"),
                            reader.GetString("Title"),
                            reader.GetString("ContentCam"),
                            reader.GetDateTime("DateCam"),
                            reader.GetBoolean("IsDeleted")
                        ));
                    }
                }
            }
            return campaigns;
        }

        public async Task<bool> DeleteCampaignAsync(int id)
        {
            using (var connection = new MySqlConnection(DatabaseConfiguration.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                var query = "UPDATE Campaign SET IsDeleted = 1 WHERE Id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<IEnumerable<CampaignMessage>> GetLastTop3CampaignsAsync()
        {
            var campaigns = new List<CampaignMessage>();
            using (var connection = new MySqlConnection(DatabaseConfiguration.Instance.ConnectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT Id, Title, ContentCam, DateCam, IsDeleted
                              FROM Campaign WHERE IsDeleted = 0
                              ORDER BY DateCam DESC 
                              LIMIT 3";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        campaigns.Add(new CampaignMessage(
                            reader.GetInt32("Id"),
                            reader.GetString("Title"),
                            reader.GetString("ContentCam"),
                            reader.GetDateTime("DateCam"),
                            reader.GetBoolean("IsDeleted")
                        ));
                    }
                }
            }
            return campaigns;
        }
    }
}
