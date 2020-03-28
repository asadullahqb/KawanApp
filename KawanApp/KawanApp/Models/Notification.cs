using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Models
{
    public class Notification
    {
        public string ServerKey { get { return App.ServerKey; } }
        public int NotificationId { get; set; }
        public string SendingUser { get; set; } //StudentId
        public string ReceivingUser { get; set; } //StudentId
        public string Pic { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string FormattedTime
        {
            get
            {
                TimeSpan timeago = DateTime.Now.Subtract(Timestamp);
                if (Timestamp.Equals(DateTime.Now))
                    return "Now";
                else if (timeago.TotalMinutes <= 3)
                    return "Now";
                else if (timeago.TotalMinutes <= 59)
                {
                    int intmins = (int)timeago.TotalMinutes;
                    return intmins.ToString() + "mins ago";
                }
                else if ((timeago.TotalHours > 1) && timeago.TotalHours < 2)
                    return "1hr ago";
                else if ((timeago.TotalHours >= 2) && (timeago.TotalHours < 24))
                {
                    int inthrs = (int)timeago.TotalHours;
                    return inthrs.ToString() + "hrs ago";
                }
                else if (timeago.TotalHours == 24)
                    return "1d ago";
                else
                {
                    int intdays = (int)timeago.TotalDays;
                    return intdays.ToString() + "d ago";
                }
            }
        }
        public bool IsRead { get; set; }
    }
}
