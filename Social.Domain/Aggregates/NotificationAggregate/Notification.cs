using System;

namespace Social.Domain.Aggregates.NotificationAggregate
{
    public class Notification
    {
        private Notification() { }

        public Guid Id { get; private set; }

        public Guid ReceiverUserId { get; private set; }

        public Guid SenderUserId { get; private set; }

        public NotificationType Type { get; private set; }

        public Guid? PostId { get; private set; }

        public string Message { get; private set; }

        public bool IsRead { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public static Notification CreateNotification(
            Guid receiverUserId,
            Guid senderUserId,
            NotificationType type,
            string message,
            Guid? postId = null)
        {
            return new Notification
            {
                Id = Guid.NewGuid(),
                ReceiverUserId = receiverUserId,
                SenderUserId = senderUserId,
                Type = type,
                PostId = postId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }

    public enum NotificationType
    {
        Follow = 1,
        Like = 2,
        Comment = 3,
        Mention = 4,
        Repost = 5
    }
}