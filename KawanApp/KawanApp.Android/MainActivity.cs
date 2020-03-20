using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Android.Content;
using Xamarin.Forms;
using KawanApp.Interfaces;
using KawanApp.Droid.Interfaces;
using KawanApp.Droid.Services;
using System.Timers;
using System.Threading.Tasks;
using Android.Util;
using Plugin.Toast;

namespace KawanApp.Droid
{
    [Activity(Label = "Kawan", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Timer Timer = new Timer(5000); //5000 milisecs - 5 secs
        private INotificationManager NotificationManager;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            RequestedOrientation = ScreenOrientation.Portrait;

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            LoadApplication(new App());
            CreateNotificationFromIntent(base.Intent);
            var intent = new Intent(this, typeof(NotificationService));
            //StartService(intent);

            NotificationManager = DependencyService.Get<INotificationManager>();
            NotificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
            };

            Timer.Elapsed += timer_Elapsed;
            //Timer.Start();
        }


        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Toast.MakeText(this, "Hello", ToastLength.Short).Show();
            NotificationManager.ScheduleMessageNotification("Hello", "Your notification is working!");
            Timer.Start(); //Restart timer
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                string message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}