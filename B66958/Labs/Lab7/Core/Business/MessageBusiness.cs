namespace ApiLab7;

public class MessageBusiness
{
    private MessageData messageData;

    public MessageBusiness()
    {
        messageData = new MessageData();
    }

    public async Task AddMessage(String text)
    {
        if(String.IsNullOrEmpty(text)) throw new ArgumentException("The message cannot be empty");
        var message = Message.BuildForStorage(text);
        await messageData.InsertMessageAsync(message);
    }

    public async Task<Message> RemoveMessage(String id)
    {
        if(String.IsNullOrEmpty(id)) throw new ArgumentException("The message identifier cannot be empty");
        return await messageData.DeleteMessageAsync(id);
    }

    public async Task<IEnumerable<Message>> GetMessages()
    {
        return await messageData.GetMessages();
    }
}