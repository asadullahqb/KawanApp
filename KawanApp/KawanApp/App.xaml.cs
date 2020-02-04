using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using KawanApp.Models;
using KawanApp.Views.Pages;
using System.Runtime.CompilerServices;

namespace KawanApp
{
    public partial class App : Application
    {
        private static bool _isUserLoggedIn;
        public static string ServerKey => "4&6R=KLL2gf%7^+E";
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


        public App()
        {
            InitializeComponent();
            GetPreferences();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            var current = Connectivity.NetworkAccess;
            var appshell = new AppShell(); //Reuse the same app shell once it's loaded

            if (current == NetworkAccess.Internet)
                NetworkStatus = true;

            MainPage = appshell;

            if (!IsUserLoggedIn || !StayLoggedIn)
                MainPage.Navigation.PushModalAsync(new LoginPage());

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
            MessagingCenter.Subscribe<SettingsPage>(this, "navigateToLoginPage", (sender) => { appshell = new AppShell();  MainPage = appshell; MainPage.Navigation.PushModalAsync(new LoginPage()); });
            //
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
                NetworkStatus = true;
            else
                NetworkStatus = false;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
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
