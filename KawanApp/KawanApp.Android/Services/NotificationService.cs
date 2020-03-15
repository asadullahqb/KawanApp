using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using KawanApp.Interfaces;
using Xamarin.Forms;

namespace KawanApp.Droid.Services
{
    [Service(Exported = true, Name = "com.companyname.kawanapp.NotificationService")]
    public class NotificationService : Service
    {
        private INotificationManager NotificationManager;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            var t = new Thread(async() => {
                NotificationManager = DependencyService.Get<INotificationManager>();
                NotificationManager.NotificationReceived += (sender, eventArgs) =>
                {
                    var evtData = (NotificationEventArgs)eventArgs;
                };
                while (true)
                {
                    await Task.Run(() => NotificationManager.ScheduleNotification("Hello", "Your notification is working!"));
                    await Task.Delay(5000);
                    //await Task.Run(() => StopSelf());
                }
            }
            );
            t.Start();
            return StartCommandResult.NotSticky;
        }
    }
}