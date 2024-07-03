using ApiLab7;

namespace UT;

public class CampaignTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        Db.BuildDb(
            "Data Source=163.178.173.130;User ID=basesdedatos;Password=BaSesrp.2024; Encrypt=False;"
        );
    }

    [Test]
    public async Task CreateCampaignWithNoText_ThrowsException()
    {
        CampaignBusiness campaignBusiness = new CampaignBusiness();
        String text = "";
        Assert.ThrowsAsync<ArgumentException>(() => campaignBusiness.AddCampaignAsync(text));
    }

    [Test]
    public async Task CreateCampaignWithTextLargerThan5kCharacters_ThrowsException()
    {
        CampaignBusiness campaignBusiness = new CampaignBusiness();
        string text = new string('a', 5001);
        Assert.ThrowsAsync<ArgumentException>(() => campaignBusiness.AddCampaignAsync(text));
    }

    [Test]
    public async Task RemoveCampaignWithEmptyId_ThrowsException()
    {
        CampaignBusiness campaignBusiness = new CampaignBusiness();
        string id = "";
        Assert.ThrowsAsync<ArgumentException>(() => campaignBusiness.RemoveCampaignAsync(id));
    }

    [Test]
    public async Task CreateCampaignWithText_DoesNotThrowsException()
    {
        CampaignBusiness campaignBusiness = new CampaignBusiness();
        string text = "Campaña";
        Assert.DoesNotThrowAsync(() => campaignBusiness.AddCampaignAsync(text));
    }

    [Test]
    public async Task GetAllCampaigns_DoesNotThrowException()
    {
        CampaignBusiness campaignBusiness = new CampaignBusiness();
         string text = "Campaña";
        await campaignBusiness.AddCampaignAsync(text);
        var campaigns = await campaignBusiness.GetCampaignsAsync();
        CollectionAssert.IsNotEmpty(campaigns);
    }
}