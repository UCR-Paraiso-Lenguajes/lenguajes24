using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Store_API.Database;

namespace Core.Business
{
    public class NotificationsLogic
    {
        private readonly DB_API db_api;

        public NotificationsLogic()
        {
            db_api = new DB_API();
        }

        public async Task<Notifications> InsertNotificationAsync(Notifications insertedNotify)
        {
            if (insertedNotify == null)
                throw new ArgumentException($"{nameof(insertedNotify)} cannot be null");

            if (string.IsNullOrWhiteSpace(insertedNotify.notificationName))
                throw new ArgumentException($"{nameof(insertedNotify.notificationName)} cannot be null or empty");

            if (string.IsNullOrWhiteSpace(insertedNotify.notificationMessage))
                throw new ArgumentException($"{nameof(insertedNotify.notificationMessage)} cannot be null or empty");

            if (insertedNotify.notificationCreatedDate == DateTime.MinValue)
                throw new ArgumentException($"{nameof(insertedNotify.notificationCreatedDate)} not valid date");

            Notifications insertedNotification = await db_api.InsertNotificationAsync(insertedNotify);
            if (insertedNotification == null)
                throw new ArgumentException($"{nameof(insertedNotification)} cannot be null");

            return insertedNotification;
        }

        public async Task DeleteNotificationAsync(int deletedNotify)
        {
            if (deletedNotify <= 0)
                throw new ArgumentException($"{nameof(deletedNotify)} cannot be null");

            await db_api.DeleteOneNotificationAsync(deletedNotify);
        }

        public async Task<IEnumerable<Notifications>> SelectAllNotifications()
        {
            IEnumerable<Notifications> allNotifications = await db_api.getNotificationsForAdminsAsync();
            if (allNotifications == null)
                throw new ArgumentException($"{nameof(allNotifications)} can be 0 but not null");

            foreach (var notification in allNotifications)
            {
                if (notification.notificationId <= 0)
                    throw new ArgumentException($"{nameof(notification)} must contain a notifyID positive.");

                if (string.IsNullOrEmpty(notification.notificationName))
                    throw new ArgumentException($"{nameof(notification.notificationName)} cannot contain an empty name.");

                if (string.IsNullOrEmpty(notification.notificationMessage))
                    throw new ArgumentException($"{nameof(notification.notificationMessage)} cannot contain a null message.");

                if (notification.notificationCreatedDate == DateTime.MinValue)
                    throw new ArgumentException($"{nameof(notification.notificationCreatedDate)} must contain a valid creation date.");
            }
            return allNotifications;
        }

        public async Task<IEnumerable<Notifications>> SelectNotificationsForUsers()
        {
            IEnumerable<Notifications> userNotifications = await db_api.getNotificationsForUsersAsync();
            if (userNotifications == null)
                throw new ArgumentException($"{nameof(userNotifications)} can be 0 but not null");

            foreach (var notification in userNotifications)
            {
                if (notification.notificationId <= 0)
                    throw new ArgumentException($"{nameof(notification.notificationId)} must contain a notifyID positive.");

                if (string.IsNullOrEmpty(notification.notificationName))
                    throw new ArgumentException($"{nameof(notification.notificationName)} cannot contain an empty name.");

                if (string.IsNullOrEmpty(notification.notificationMessage))
                    throw new ArgumentException($"{nameof(notification.notificationMessage)} cannot contain a null message.");

                if (notification.notificationCreatedDate == DateTime.MinValue)
                    throw new ArgumentException($"{nameof(notification.notificationCreatedDate)} must contain a valid creation date.");
            }
            return userNotifications;
        }
    }
}