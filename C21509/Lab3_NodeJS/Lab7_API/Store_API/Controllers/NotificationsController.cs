using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business;
using Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly NotificationsLogic _notificationsLogic;

        public NotificationsController(IHubContext<NotificationHub> notificationHub, NotificationsLogic notificationsLogic)
        {
            _notificationHub = notificationHub;
            _notificationsLogic = notificationsLogic;
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> CreateNotificationAsync([FromBody] Notifications newNotify)
        {
            try
            {
                if (newNotify == null)
                {
                    return BadRequest("Notification cannot be null.");
                }

                Notifications insertedNotification = await _notificationsLogic.InsertNotificationAsync(newNotify);
                await _notificationHub.Clients.All.SendAsync("Receive", insertedNotification);
                return Ok(insertedNotification);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"An error has occurred while generating the notification: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error has occurred while generating the notification: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{idNotification}")]
        public async Task<IActionResult> DeleteNotificationAsync(int idNotification)
        {
            try
            {
                await _notificationsLogic.DeleteNotificationAsync(idNotification);
                await _notificationHub.Clients.All.SendAsync("Delete", idNotification);
                return Ok(true);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"An error has occurred while deleting the notification: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error has occurred while deleting the notification: {ex.Message}");
            }
        }

        [HttpGet("Select")]
        public async Task<IActionResult> SelectNotificationAsync()
        {
            try
            {
                var allNotifications = await _notificationsLogic.SelectAllNotifications();
                return Ok(allNotifications);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"An error has occurred while selecting the notifications: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error has occurred while selecting the notifications: {ex.Message}");
            }
        }
    }
}