using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Interfaces
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        int ScheduleMessageNotification(string user, string message);
        int ScheduleFriendNotification(string user, string message);
        void ClearMessageNotifications();
        void ClearFriendNotifications();
        void ClearAllNotifications();
        int RemoveNotification(int messageId, string user);

        void ReceiveNotification(string title, string message);
    }
}
