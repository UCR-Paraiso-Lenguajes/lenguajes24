namespace ApiLab7;

public class Campaign
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime SentAt { get; set; }
    public bool Enabled { get; set; }

    public Campaign() { }

    private Campaign(Guid id, String text, DateTime date, bool enabled)
    {
        Id = id;
        Text = text;
        SentAt = date;
        Enabled = enabled;
    }

    public static Campaign BuildForStorage(String text)
    {
        if (String.IsNullOrEmpty(text))
            throw new ArgumentNullException("The text cannot be empty");
        var id = Guid.NewGuid();
        var date = DateTime.Now;
        return new Campaign(id, text, date, true);
    }

    public static Campaign BuildForDisplay(Guid id, String text, DateTime date, bool enabled)
    {
        if (id == null)
            throw new ArgumentNullException("The id cannot be null");
        if (String.IsNullOrEmpty(text))
            throw new ArgumentNullException("The text cannot be empty");
        return new Campaign(id, text, date, enabled);
    }
}
