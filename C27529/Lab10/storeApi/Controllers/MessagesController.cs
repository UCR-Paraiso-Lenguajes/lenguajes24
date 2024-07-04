using Microsoft.AspNetCore.Mvc;
using storeApi.db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace storeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest("Message content cannot be empty.");
            }

            // Prevent adding duplicate messages by checking if the content already exists
            var existingMessages = await StoreDB.GetMessagesByContentAsync(content);
            if (existingMessages.Count > 0)
            {
                return Conflict("A message with the same content already exists.");
            }

            int messageId = await StoreDB.AddMessageAsync(content);
            var message = new Dictionary<string, object>
            {
                { "id", messageId },
                { "content", content }
            };

            return Ok(message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await StoreDB.GetLastThreeMessagesAsync();
            return Ok(messages);
        }
    }
}
