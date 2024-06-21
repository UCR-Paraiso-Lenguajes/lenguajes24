namespace ApiLab7;

public class Message
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime SentAt { get; set; }

    public Message(){
        
    }

    private Message(Guid id, String text, DateTime date){
        Id = id;
        Text = text;
        SentAt = date;
    }

    public static Message BuildForStorage(String text)
    {
        if(String.IsNullOrEmpty(text)) throw new ArgumentNullException("The message cannot be empty");
        var id = Guid.NewGuid();
        var date = DateTime.Now;
        return new Message(id, text, date);
    }

    public static Message BuildForDisplay(Guid id, String text, DateTime date)
    {
        if(id == null) throw new ArgumentNullException("The id cannot be null");
        if(String.IsNullOrEmpty(text)) throw new ArgumentNullException("The message cannot be empty");
        return new Message(id, text, date);
    }
}