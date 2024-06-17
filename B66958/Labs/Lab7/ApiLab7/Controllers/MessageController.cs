using Microsoft.AspNetCore.Mvc;

namespace ApiLab7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly WebSocketService _webSocketService;

        public MessagesController(WebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpPost]
        public IActionResult CreateMessage([FromBody] MessageDto messageDto)
        {
            _webSocketService.SendMessage(messageDto.Text);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMessage(int id)
        {
            // Logic to delete a message (not related to WebSockets)
            return Ok();
        }
    }

    public class MessageDto
    {
        public string Text { get; set; }
    }

}