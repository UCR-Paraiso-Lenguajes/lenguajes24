namespace Core.Models
{
    public class Notifications
    {
        public int notificationId { get; private set; }
        public string notificationName { get; private set; }
        public string notificationMessage { get; private set; }
        public DateTime notificationCreatedDate { get; set; }
        public int notificationStatus { get; set; }

        public Notifications(int notificationId, string notificationName, string notificationMessage , DateTime notificationCreatedDate, int notificationStatus)
        {
            if (notificationId == null)
                throw new ArgumentException($"{nameof(notificationId)} cannot be negative or 0");

            if (string.IsNullOrWhiteSpace(notificationName))
                throw new ArgumentException($"{nameof(notificationName)} cannot be null");

            if (string.IsNullOrWhiteSpace(notificationMessage))
                throw new ArgumentException($"{nameof(notificationMessage)} cannot be null");

            if (notificationMessage.Length > 5000)
                throw new ArgumentException($"{nameof(notificationMessage)} cannot have more tha 5K characters");

            if (notificationStatus != 0 && notificationStatus != 1)
                throw new ArgumentException($"{nameof(notificationStatus)} should be 0 or 1");

            this.notificationId = notificationId;
            this.notificationName = notificationName;
            this.notificationMessage = notificationMessage;
            this.notificationCreatedDate = notificationCreatedDate;
            this.notificationStatus = notificationStatus;
        }
    }

}