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
using Plugin.Toast;

namespace KawanApp
{
    public partial class App : Application
    {
        private static bool _isUserLoggedIn;
        public static string ServerKey => "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";
        public static string Server => "http://www.imcc.usm.my/kawan/";
        //at Sunny Ville home: http://192.168.0.157/
        //at KL home: http://192.168.0.197/
        //at KL home: http://192.168.0.197/
        //at USM: http://10.212.42.130/
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
        public static bool AppClosed { get; set; } = false;
        public static bool CheckingConnectivity { get; set; } = false;

        public static bool StayLoggedIn { get; set; }
        private static string OriginPage { get; set; } = null;
        private static string CurrentSessionId { get; set; }

        private IServerApi ServerApi => RestService.For<IServerApi>(Server);

        public App()
        {
            InitializeComponent();
            GetPreferences();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

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

        static public async Task CheckConnectivity()
        {
            CheckingConnectivity = true;

            await AccessNetwork();

            await Task.Delay(2000);

            //Do a recursive try of connecting and break only if the connection is established
            while (!AppClosed && !NetworkStatus)
            {
                CrossToastPopUp.Current.ShowCustomToast("Reconnecting in 5 secs...", "#838181", "#FFFFFF", Plugin.Toast.Abstractions.ToastLength.Short);
                if (AppClosed) { CheckingConnectivity = false; return; }
                await Task.Delay(1000);
                CrossToastPopUp.Current.ShowCustomToast("Reconnecting in 4 secs...", "#838181", "#FFFFFF", Plugin.Toast.Abstractions.ToastLength.Short);
                if (AppClosed) { CheckingConnectivity = false; return; }
                await Task.Delay(1000);
                CrossToastPopUp.Current.ShowCustomToast("Reconnecting in 3 secs...", "#838181", "#FFFFFF", Plugin.Toast.Abstractions.ToastLength.Short);
                if (AppClosed) { CheckingConnectivity = false; return; }
                await Task.Delay(1000);
                CrossToastPopUp.Current.ShowCustomToast("Reconnecting in 2 secs...", "#838181", "#FFFFFF", Plugin.Toast.Abstractions.ToastLength.Short);
                if (AppClosed) { CheckingConnectivity = false; return; }
                await Task.Delay(1000);
                CrossToastPopUp.Current.ShowCustomToast("Reconnecting in 1 secs...", "#838181", "#FFFFFF", Plugin.Toast.Abstractions.ToastLength.Short);
                if (AppClosed) { CheckingConnectivity = false; return; }
                await AccessNetwork();
                await Task.Delay(1000);
                CheckingConnectivity = false;
                if (!NetworkStatus)
                    CrossToastPopUp.Current.ShowCustomToast("Reconnecting...", "#838181", "#FFFFFF", Plugin.Toast.Abstractions.ToastLength.Short);
                else if (AppClosed)
                    return;
                else if(NetworkStatus)
                {
                    CrossToastPopUp.Current.ShowCustomToast("Connected!", "#838181", "#FFFFFF", Plugin.Toast.Abstractions.ToastLength.Short);
                    return;
                }
            }
        }

        private static async Task AccessNetwork()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                /*
                var ping = new Ping();
                var isconnected = await ping.SendPingAsync("google.com");
                if (isconnected.Status == IPStatus.Success)
                    NetworkStatus = true;
                else
                    NetworkStatus = false;
                    */
                NetworkStatus = true;
            }
            else
                NetworkStatus = false;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (!CheckingConnectivity)
                await CheckConnectivity();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            /* if (IsUserLoggedIn || StayLoggedIn)
                 LogInSession();*/
            AppClosed = false;
            await Task.Delay(3000);
            if(!CheckingConnectivity && !NetworkStatus)
                await CheckConnectivity();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            //LogOutSession();
            AppClosed = true;
        }

        protected override async void OnResume()
        {
            // Handle when your app resumes
            /*if (IsUserLoggedIn || StayLoggedIn)
                LogInSession();
            */
            AppClosed = false;
            if (!CheckingConnectivity && !NetworkStatus)
                await CheckConnectivity();
        }

        async void LogInSession() 
        {
            if (string.IsNullOrEmpty(CurrentSessionId))
            {
                CurrentSessionId += "0"; //Prevent this function from being executed again if the server is too slow
                Session s = new Session() { StudentId = CurrentUser, StartOrEnd = DateTime.Now, Type = "Log" };
                SessionReply sr;
                if (NetworkStatus)
                    sr = await ServerApi.StartSession(s);
                else
                    return;
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
                ReplyMessage rm;
                if (NetworkStatus)
                    rm = await ServerApi.EndSession(s);
                else
                    return;

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
