using System;

namespace KawanApp.Models
{
    public class ChatMessageItem
    {
        public string Pic { get; set; }
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string LastMessage { get; set; }
        public bool LastMessageIsOwnMessage { get; set; }
        public DateTime TimeStamp { get; set; }
        public string FormattedTime 
        {
            get 
            {
                TimeSpan timeago = DateTime.Now.Subtract(TimeStamp);
                if (TimeStamp.Equals(DateTime.Now))
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
                    return intdays.ToString()+"d ago";
                }
            } 
        }
    }
}
