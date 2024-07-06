public class CampaignMessage
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public bool IsDeleted { get; set; }

     public CampaignMessage(int id, string title, string content, DateTime date, bool isDeleted)
    {
        if (string.IsNullOrEmpty(title)) throw new ArgumentException("El título de la campaña es obligatorio.");
        if (string.IsNullOrEmpty(content)) throw new ArgumentException("El contenido de la campaña es obligatorio.");
        if (content.Length > 5000) throw new ArgumentException("El contenido de la campaña no puede exceder los 5000 caracteres.");

        Id = id;
        Title = title;
        Content = content;
        Date = date;
        IsDeleted = isDeleted;
    }
}
