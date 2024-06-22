using Microsoft.AspNetCore.Mvc;

namespace ApiLab7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly WebSocketService _webSocketService;
        private MessageBusiness messageBusiness;

        public MessagesController(WebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
            messageBusiness = new MessageBusiness();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessageAsync([FromBody] Message message)
        {
            if(message == null) throw new ArgumentException("Please provide a message");
            var messageResult = await messageBusiness.AddMessageAsync(message.Text);
            _webSocketService.SendMessage(messageResult);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageAsync(String id)
        {
            if(id == null) throw new ArgumentException("Please provide a message id");
            var deleted = await messageBusiness.RemoveMessageAsync(id);
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            IEnumerable<Message> messages = await messageBusiness.GetMessagesAsync();
            return messages;
        }
    }
}