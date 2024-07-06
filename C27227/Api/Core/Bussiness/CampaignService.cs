using Core;
using KEStoreApi.Data;
using KEStoreApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEStoreApi
{
    public sealed class CampaignService
    {
        private readonly CampaignsDatabase _campaignsDatabase;
        private readonly IHubContext<CampaignHub> _hubContext;

        public CampaignService(IHubContext<CampaignHub> hubContext)
        {
            _hubContext = hubContext;
            _campaignsDatabase = new CampaignsDatabase();
        }

        public async Task<CampaignMessage> AddCampaignAsync(CampaignMessage campaignMessage)
        {
            ValidateCampaignMessage(campaignMessage);

            try
            {
                var insertedCampaign = await _campaignsDatabase.InsertCampaignAsync(campaignMessage);
                await _hubContext.Clients.All.SendAsync("ReceiveNewCampaign", insertedCampaign.Content, insertedCampaign.Title, insertedCampaign.Id);
                return insertedCampaign;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting campaign and broadcasting update via SignalR", ex);
            }
        }

        public async Task<IEnumerable<CampaignMessage>> GetAllCampaignsAsync()
        {
            return await _campaignsDatabase.GetAllCampaignsAsync();
        }

        public async Task<IEnumerable<CampaignMessage>> GetLatestTop3CampaignsAsync()
        {
            return await _campaignsDatabase.GetLastTop3CampaignsAsync();
        }

        public async Task<bool> RemoveCampaignAsync(int id)
        {
            var isDeleted = await _campaignsDatabase.DeleteCampaignAsync(id);
            if (isDeleted)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveDeletedCampaign", id);
            }
            return isDeleted;
        }

        private void ValidateCampaignMessage(CampaignMessage campaignMessage)
        {
            if (campaignMessage == null) throw new ArgumentException("Campaign message cannot be null.");
            if (string.IsNullOrEmpty(campaignMessage.Title)) throw new ArgumentException("Campaign title is required.");
            if (string.IsNullOrEmpty(campaignMessage.Content)) throw new ArgumentException("Campaign content is required.");
            if (campaignMessage.Content.Length > 5000) throw new ArgumentException("Campaign content cannot exceed 5000 characters.");
        }
    }
}
