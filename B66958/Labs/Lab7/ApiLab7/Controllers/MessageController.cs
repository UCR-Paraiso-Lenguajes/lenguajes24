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
            var messageResult = await messageBusiness.AddMessage(message.Text);
            _webSocketService.SendMessage(messageResult);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(String id)
        {
            if(id == null) throw new ArgumentException("Please provide a message id");
            var deleted = await messageBusiness.RemoveMessage(id);
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<IEnumerable<Message>> GetMessage()
        {
            IEnumerable<Message> messages = await messageBusiness.GetMessages();
            return messages;
        }
    }
}