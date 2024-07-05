namespace ApiLab7;

public class CampaignBusiness
{
    private CampaignData campaignData;

    public CampaignBusiness()
    {
        campaignData = new CampaignData();
    }

    public async Task<Campaign> AddCampaignAsync(String text)
    {
        if (String.IsNullOrEmpty(text))
            throw new ArgumentException("The text cannot be empty");
        if (text.Length > 5000)
            throw new ArgumentException("The text cannot exceed 5000 characters");
        var campaign = Campaign.BuildForStorage(text);
        await campaignData.InsertCampaignAsync(campaign);
        return campaign;
    }

    public async Task<Campaign> RemoveCampaignAsync(String id)
    {
        if (String.IsNullOrEmpty(id))
            throw new ArgumentException("The campaign identifier cannot be empty");
        return await campaignData.DeleteCampaignAsync(id);
    }

    public async Task<IEnumerable<Campaign>> GetCampaignsAsync()
    {
        return await campaignData.GetCampaignsAsync();
    }
}
