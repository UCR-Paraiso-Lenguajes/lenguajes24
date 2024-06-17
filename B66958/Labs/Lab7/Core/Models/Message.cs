namespace ApiLab7;

public class Message
{
    public Guid Id { get; set; }
    public string Text { get; set; }

    public Message(){
        
    }

    private Message(Guid id, String text){
        Id = id;
        Text = text;
    }

    public static Message BuildForStorage(String text)
    {
        if(String.IsNullOrEmpty(text)) throw new ArgumentNullException("The message cannot be empty");
        var id = Guid.NewGuid();
        return new Message(id, text);
    }

    public static Message BuildForDisplay(Guid id, String text)
    {
        if(id == null) throw new ArgumentNullException("The id cannot be null");
        if(String.IsNullOrEmpty(text)) throw new ArgumentNullException("The message cannot be empty");
        return new Message(id, text);
    }
}