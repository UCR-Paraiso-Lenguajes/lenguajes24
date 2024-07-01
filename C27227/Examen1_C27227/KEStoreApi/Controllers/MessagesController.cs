using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KEStoreApi.Models;
using Core.Data;
using Microsoft.Extensions.Logging;

namespace KEStoreApi.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageDatabase _messageDatabase;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(MessageDatabase messageDatabase, ILogger<MessagesController> logger)
        {
            _messageDatabase = messageDatabase;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Message>> CreateMessage([FromBody] Message message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Title) || string.IsNullOrWhiteSpace(message.Content))
            {
                return BadRequest("Message, title, and content must not be null or empty.");
            }

            try
            {
                var createdMessage = await _messageDatabase.CreateMessageAsync(message);
                return CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.Id }, createdMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the message.");
                return StatusCode(500, "An error occurred while creating the message.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages([FromQuery] int limit = 3)
        {
            try
            {
                var messages = await _messageDatabase.GetMessagesAsync(limit);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving messages.");
                return StatusCode(500, "An error occurred while retrieving messages.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessageById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid message id: Id must be a positive integer.");
            }

            try
            {
                var message = await _messageDatabase.GetMessageByIdAsync(id);
                if (message == null)
                {
                    return NotFound();
                }

                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the message.");
                return StatusCode(500, "An error occurred while retrieving the message.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid message id: Id must be a positive integer.");
            }

            try
            {
                var success = await _messageDatabase.DeleteMessageAsync(id);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the message.");
                return StatusCode(500, "An error occurred while deleting the message.");
            }
        }
    }
}
