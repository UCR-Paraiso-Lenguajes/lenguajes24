using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationHub notificationHub;
        private readonly NotificationsLogic notificationsLogic;

        public NotificationsController(NotificationHub notificationHub)
        {
            notificationHub = notificationHub;
            notificationsLogic = new NotificationsLogic();
        }

        [HttpPost("Notifications/Insert")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> CreateNotificationAsync([FromBody] Notifications newNotify)
        {
            try
            {
                Notifications insertedNotification = await notificationsLogic.InsertNotificationAsync(newNotify);
                await notificationHub.SendNotificationAsync(insertedNotification);
                return Ok(true);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, "An error has ocurred while generating the notification." + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error has ocurred while generating the notification." + ex.Message);
            }
        }

        [HttpDelete("Notifications/Delete/{idNotification}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> DeleteNotificationAsync(int idNotification)
        {
            try
            {
                await notificationsLogic.DeleteNotificationAsync(idNotification);
                await notificationHub.NotifyDeletionIdAsync(idNotification);
                return Ok(true);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, "An error has ocurred while deleting the notification." + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error has ocurred while deleting the notification." + ex.Message);
            }
        }

        [HttpGet("notifications/select")]
        public async Task<IActionResult> SelectNotificationAsync()
        {
            try
            {
                var allNotificacionsForAdmin = await notificationsLogic.SelectAllNotifications();
                return Ok(allNotificacionsForAdmin);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, "An error has ocurred while selecting the notifications." + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error has ocurred while selecting the notifications." + ex.Message);
            }
        }
    }
}