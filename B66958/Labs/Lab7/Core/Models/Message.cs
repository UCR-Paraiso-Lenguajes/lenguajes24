namespace ApiLab7;

public class Message
{
    public Guid Id { get; private set; }
    public string Message { get; private set; }

    public Message(String message){
        Id = Guid.NewGuid();
        if(String.IsNullOrEmpty(Message)) throw new ArgumentNullException("The message cannot be empty");
        Message = message;
    }
}