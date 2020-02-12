using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using KawanApp.Models;
using KawanApp.Views.Pages;
using System.Runtime.CompilerServices;
using KawanApp.Interfaces;
using Refit;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using KawanApp.ViewModels;

namespace KawanApp
{
    public partial class App : Application
    {
        private static bool _isUserLoggedIn;
        public static string ServerKey => "zAWscvPdyx8YFCZAvDmGcw3pmG6jcSbF7THV5WCNGzu6axRFVfA3aFHuKarkbK7tXuLTyF7xGWNapgDphm832S8KpmjAFxNxpzy8f5Ef2FMtmZxuFsnmVfxaA467wQZWq3P3qKFfP5wzrbhGKBEge4BtYZjb5LA3a8gMtyLNQUNkQq2YcbURK838fkdyD7rUPWfswr9BWGeT5aV674yDTqFsH95a3rTPdV38aaHu8Q9GCGayUWSVaqYkCrzPr5tc";
        public static string Server => "http://www.imcc.usm.my/kawan/";
        //at Sunny Ville home: http://192.168.0.157/
        //at KL home: http://192.168.0.197/
        //at USM: http://10.212.148.92/
        //live: http://www.imcc.usm.my/kawan/
        public static bool IsUserLoggedIn
        {
            get => _isUserLoggedIn;
            set
            {
                _isUserLoggedIn = value;
                ShowLoginPage(_isUserLoggedIn);
            }
        }
        public static string CurrentUser { get; set; }
        public static string CurrentUserType { get; set; }
        public static bool NetworkStatus { get; set; } = false;

        public static bool StayLoggedIn { get; set; }
        private static string OriginPage { get; set; } = null;
        private static string CurrentSessionId { get; set; }

        private IServerApi ServerApi => RestService.For<IServerApi>(Server);

        public App()
        {
            InitializeComponent();
            GetPreferences();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            CheckConnectivity();

            var appshell = new AppShell(); //Reuse the same app shell once it's loaded

            MainPage = appshell;

            if (!IsUserLoggedIn || !StayLoggedIn)
                MainPage.Navigation.PushModalAsync(new LoginPage());

            MessagingCenter.Subscribe<string>(this, "AppLoggedIn", (sender) => { LogInSession(); });

            //Navigation to views outside of the app shell and back:
            //
            //Messaging center is used for view navigation so that the same app shell 
            //(with its state conserved) is used.
            MessagingCenter.Subscribe<LoginPage>(this, "navigateToSignUp", (sender) => { MainPage.Navigation.PushModalAsync(new SignUpPage()); });
            MessagingCenter.Subscribe<ViewAllProfilesPage, KawanUser>(this, "navigateToViewAProfilePage", (sender, KawanUser) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White  }; MainPage.Navigation.PushAsync(new ViewAProfilePage(KawanUser)); });
            MessagingCenter.Subscribe<ViewAProfilePage>(this, "navigateBack", (sender) => { MainPage = appshell; });
            MessagingCenter.Subscribe<ViewAllProfilesPage, string>(this, "navigateToChatPage", (sender, ReceivingUserStudentId) => { OriginPage = null; MainPage = new NavigationPage(); MainPage.Navigation.PushModalAsync(new ChatPage(ReceivingUserStudentId)); });
            MessagingCenter.Subscribe<ViewAProfilePage, string>(this, "navigateToChatPage", (sender, ReceivingUserStudentId) => { OriginPage = "View A Profile Page"; MainPage.Navigation.PushModalAsync(new ChatPage(ReceivingUserStudentId)); });
            MessagingCenter.Subscribe<ChatPage>(this, "navigateBack", (sender) => { if (OriginPage == "View A Profile Page") MainPage.Navigation.PopModalAsync(); else MainPage = appshell; });
            MessagingCenter.Subscribe<ViewAProfilePage, string>(this, "navigateToAnalyticsPage", (sender, KawanUserStudentId) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new AnalyticsPage(KawanUserStudentId)); });
            MessagingCenter.Subscribe<AnalyticsPage>(this, "navigateBack", (sender) => { MainPage = appshell; });
            MessagingCenter.Subscribe<ViewAProfilePageViewModel, KawanUser>(this, "navigateToEditPage", (sender, KawanData) => { MainPage.Navigation.PushModalAsync(new SignUpPage(KawanData)); });
            MessagingCenter.Subscribe<AnalyticsPage>(this, "navigateBack", (sender) => { MainPage = appshell; });
            MessagingCenter.Subscribe<AllMessagesPage, string>(this, "navigateToChatPage", (sender, ReceivingUserStudentId) => { OriginPage = null; MainPage = new NavigationPage(); MainPage.Navigation.PushModalAsync(new ChatPage(ReceivingUserStudentId)); });
            MessagingCenter.Subscribe<SettingsPage>(this, "navigateToLoginPage", (sender) => { appshell = new AppShell();  MainPage = appshell; MainPage.Navigation.PushModalAsync(new LoginPage()); LogOutSession(); });
            //
        }

        static public void CheckConnectivity()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                Task.Run(async() => 
                {
                    var ping = new Ping();
                    var isconnected = await ping.SendPingAsync("google.com");
                    if (isconnected.Status == IPStatus.Success)
                        NetworkStatus = true;
                    else
                        NetworkStatus = false;
                });
            }
            else
                NetworkStatus = false;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckConnectivity();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            if (IsUserLoggedIn || StayLoggedIn)
                LogInSession();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            LogOutSession();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            if (IsUserLoggedIn || StayLoggedIn)
                LogInSession();
                
        }

        async void LogInSession() 
        {
            if (string.IsNullOrEmpty(CurrentSessionId))
            {
                CurrentSessionId += "0"; //Prevent this function from being executed again if the server is too slow
                Session s = new Session() { StudentId = CurrentUser, StartOrEnd = DateTime.Now, Type = "Log" };
                SessionReply sr = await ServerApi.StartSession(s);
                if (sr.Status)
                    CurrentSessionId = sr.SessionId;
                else
                    throw new Exception("Unable to log in to server!");
            }
        }

        async void LogOutSession()
        {
            if (!string.IsNullOrEmpty(CurrentSessionId))
            {
                Session s = new Session() { SessionId = CurrentSessionId, StartOrEnd = DateTime.Now };
                ReplyMessage rm = await ServerApi.EndSession(s);
                if (rm.Status)
                    CurrentSessionId = null;
                else
                    throw new Exception("Unable to log out from server!");
            }
        }

        static void GetPreferences()
        {
            StayLoggedIn = Preferences.Get("StayLoggedIn", false);

            IsUserLoggedIn = StayLoggedIn == false
                ? IsUserLoggedIn = false
                : Preferences.Get("IsUserLoggedIn", false);

            CurrentUser = Preferences.Get("CurrentUser", null);
            CurrentUserType = Preferences.Get("CurrentUserType", null);
        }

        static void ShowLoginPage(bool isUserLoggedIn)
        {
            if (!isUserLoggedIn)
            {
                App.Current.MainPage?.Navigation.PushModalAsync(new LoginPage());
                Preferences.Set("IsUserLoggedIn", false);
                Preferences.Set("StayLoggedIn", false);
            }
            else
            {
                App.Current.MainPage?.Navigation.PopModalAsync();
                MessagingCenter.Send("App", "AppLoggedIn");
            }

        }
    }
}
