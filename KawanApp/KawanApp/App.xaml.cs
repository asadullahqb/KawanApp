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
using Microsoft.AspNetCore.SignalR.Client;
using KawanApp.ViewModels.Pages;

namespace KawanApp
{
    public partial class App : Application
    {
        private static bool _isUserLoggedIn;
        public static string ServerKey => "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";
        public static string Server => "http://192.168.0.157/";
        //at Sunny Ville home: http://192.168.0.157/
        //at KL home: http://192.168.0.197/
        //live: https://imcc.usm.my/kawan/
        public static bool IsUserLoggedIn
        {
            get => _isUserLoggedIn;
            set
            {
                _isUserLoggedIn = value;
                ShowLoginPage(_isUserLoggedIn);
            }
        }
        public static KawanUser CurrentKawanUser { get; set; }
        public static string CurrentUser { get; set; }
        public static string CurrentFirstName { get; set; }
        public static string CurrentPic { get; set; }
        public static string CurrentUserType { get; set; }
        public static bool NetworkStatus { get; set; } = false;
        public static bool ForceReconnectOnce { get; set; } = false;
        public static bool AppClosed { get; set; } = false;
        public static bool CheckingConnectivity { get; set; } = false;

        public static bool StayLoggedIn { get; set; }
        public static HubConnection HubConnection { get; set; }
        public static string PreviousNotificationId { get; set; } = null;
        public static INotificationManager NotificationManager { get; set; }

        private static string CurrentPage { get; set; } = null;
        private static string OriginPage { get; set; } = null;
        private static string CurrentSessionId { get; set; }

        private IServerApi ServerApi => RestService.For<IServerApi>(Server);

        public App()
        {
            InitializeComponent();
            GetPreferences();
            
            //For internet connection:
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            //For notifications:
            NotificationManager = DependencyService.Get<INotificationManager>();
            NotificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
            };

            #region Chat Hub
            HubConnection = new HubConnectionBuilder()
                .WithUrl($"https://kawantest.azurewebsites.net/chathub")
                .Build();
            HubConnection.On<string, string, string>("ReceiveMessageNotification", (notificationid, sendingUser, message) =>
            {
                if (notificationid == PreviousNotificationId)
                    return;
                if(CurrentPage != "Chat Page" && CurrentPage != "All Messages Page" && CurrentPage != "Notifications Page")
                    NotificationManager.ScheduleMessageNotification(sendingUser, message);
                MessagingCenter.Send("App", "updateAllMessages"); //Send to all message page view model
                MessagingCenter.Send("App", "updateAllNotifications"); //Send to notifications page view model
                PreviousNotificationId = notificationid;
            });
            HubConnection.On<string, string, string>("ReceiveFriendNotification", (notificationid, sendingUser, message) =>
            {
                if (notificationid == PreviousNotificationId)
                    return;
                if (message.Contains("unsent"))
                {
                    NotificationManager.RemoveNotification(1, sendingUser);
                }
                else if(message.Contains("rejected"))
                {
                    //Do nothing, just update the other pages
                }
                else if (CurrentPage != "Notifications Page")
                    NotificationManager.ScheduleFriendNotification(sendingUser, message);
                MessagingCenter.Send("App", "updateAllNotifications"); //Send to notifications page view model
                MessagingCenter.Send("App", "updateProfiles"); //Send to view all profiles page view model and view a profile page view model
                PreviousNotificationId = notificationid;
            });
            HubConnection.On<string>("ReceiveReply", async(message) =>
            {
                //This function is used as a handshake between server and app.
                switch(message)
                {
                    case "Connect success!":
                        NetworkStatus = true;
                        MessagingCenter.Send("App", "updateConnection");
                        break;
                    case "Connect success but need to login.":
                        NetworkStatus = true;
                        MessagingCenter.Send("App", "updateConnection"); //Send to app shell view model
                        await Task.Delay(1000);
                        await HubConnection.InvokeAsync("OnConnected", CurrentUser, CurrentFirstName);
                        break;
                    case "Connect failure!":
                        ForceReconnectOnce = true; //Force the reconnecting
                        NetworkStatus = false;
                        MessagingCenter.Send("App", "updateConnection"); //Send to app shell view model
                        await Task.Delay(1000);
                        await CheckConnectivity();
                        break;
                    case "Join group success!":
                        break;
                    case "Join group failure!":
                        MessagingCenter.Send("App", "connectToGroup"); //Retry connecting to the group
                        break;
                }
            });
            #endregion

            var appshell = new AppShell(); //Reuse the same app shell once it's loaded

            MainPage = appshell;

            if (!IsUserLoggedIn || !StayLoggedIn)
                MainPage.Navigation.PushModalAsync(new LoginPage());
            
            //Messaging center is used for view navigation so that the same app shell 
            //(with its state conserved) is used. 
            //
            //App.xaml.cs also manages some other operations.
            //
            #region Navigate To Page
            //From AppShell
            MessagingCenter.Subscribe<AppShellViewModel>(this, "navigateToViewAProfilePage", (sender) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new ViewAProfilePage()); OriginPage = "App Shell"; CurrentPage = null; });
            MessagingCenter.Subscribe<AppShellViewModel>(this, "navigateToAnalyticsPage", (sender) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new AnalyticsPage(CurrentUser)); OriginPage = "App Shell"; CurrentPage = null; });
            MessagingCenter.Subscribe<AppShell>(this, "navigateToActivitiesPage", (sender) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new ActivitiesPage()); CurrentPage = null; });
            MessagingCenter.Subscribe<AppShell>(this, "navigateToSatisfactoryFormsPage", (sender) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new SatisfactoryFormsPage()); CurrentPage = null; });
            MessagingCenter.Subscribe<AppShell>(this, "navigateToSettingsPage", (sender) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new SettingsPage()); CurrentPage = null; });
            
            //To ChatPage
            MessagingCenter.Subscribe<ViewAllProfilesPage, KawanUser>(this, "navigateToChatPage", (sender, ReceivingUserDetails) => { OriginPage = null; MainPage = new NavigationPage() { BarBackgroundColor = Color.FromHex("#234779") }; MainPage.Navigation.PushAsync(new ChatPage(ReceivingUserDetails)); CurrentPage = "Chat Page"; });
            MessagingCenter.Subscribe<ViewAProfilePage, KawanUser>(this, "navigateToChatPage", (sender, ReceivingUserDetails) => { OriginPage = "View A Profile Page"; MainPage.Navigation.PushAsync(new ChatPage(ReceivingUserDetails)); CurrentPage = "Chat Page"; });
            MessagingCenter.Subscribe<AllMessagesPage, KawanUser>(this, "navigateToChatPage", (sender, ReceivingUserDetails) => { OriginPage = null; MainPage = new NavigationPage() { BarBackgroundColor = Color.FromHex("#234779") }; MainPage.Navigation.PushAsync(new ChatPage(ReceivingUserDetails)); CurrentPage = "Chat Page"; });
            MessagingCenter.Subscribe<AllMessagesPageViewModel, KawanUser>(this, "navigateToChatPage", (sender, ReceivingUserDetails) => { OriginPage = null; MainPage = new NavigationPage() { BarBackgroundColor = Color.FromHex("#234779") }; MainPage.Navigation.PushAsync(new ChatPage(ReceivingUserDetails)); CurrentPage = "Chat Page"; });
           
            //Other
            MessagingCenter.Subscribe<LoginPage>(this, "navigateToSignUp", (sender) => { MainPage.Navigation.PushModalAsync(new SignUpPage()); });
            MessagingCenter.Subscribe<NotificationsPage, KawanUser>(this, "navigateToViewAProfilePage", (sender, KawanUser) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new ViewAProfilePage(KawanUser)); OriginPage = "View All Profiles Page"; });
            MessagingCenter.Subscribe<ViewAllProfilesPage, KawanUser>(this, "navigateToViewAProfilePage", (sender, KawanUser) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new ViewAProfilePage(KawanUser)); OriginPage = "View All Profiles Page"; });
            MessagingCenter.Subscribe<ViewAllProfilesPageViewModel, KawanUser>(this, "navigateToViewAProfilePage", (sender, KawanUser) => { MainPage = new NavigationPage() { BarBackgroundColor = Color.White }; MainPage.Navigation.PushAsync(new ViewAProfilePage(KawanUser)); OriginPage = "View All Profiles Page"; }); //Navigation originated from notifications.
            MessagingCenter.Subscribe<ViewAProfilePageViewModel, ProfileImageFields>(this, "navigateToProfileImagePage", (sender, ProfileImageFields) => { if (ProfileImageFields.IsOwnProfile) { MainPage.Navigation.PushAsync(new ProfileImagePage(ProfileImageFields.IsOwnProfile, ProfileImageFields.Pic)); OriginPage = "Own Profile"; } else { MainPage.Navigation.PushAsync(new ProfileImagePage(ProfileImageFields.IsOwnProfile, ProfileImageFields.Pic)); OriginPage = "Other's Profile"; } });
            MessagingCenter.Subscribe<ViewAProfilePageViewModel, KawanUser>(this, "navigateToEditPage", (sender, KawanData) => { MainPage.Navigation.PushModalAsync(new SignUpPage(KawanData)); });
            MessagingCenter.Subscribe<ViewAProfilePage, string>(this, "navigateToAnalyticsPage", (sender, KawanUserStudentId) => { MainPage.Navigation.PushAsync(new AnalyticsPage(KawanUserStudentId)); OriginPage = "View A Profile Page"; });
            MessagingCenter.Subscribe<SettingsPage>(this, "navigateToLoginPage", (sender) => { CurrentKawanUser = new KawanUser(); CurrentUser = ""; CurrentUserType = ""; appshell = new AppShell(); MainPage = appshell; MainPage.Navigation.PushModalAsync(new LoginPage()); LogOutSession(); });


            #endregion

            #region Navigate Back
            MessagingCenter.Subscribe<ActivitiesPage>(this, "navigateBack", (sender) => { MainPage = appshell; });
            MessagingCenter.Subscribe<SatisfactoryFormsPage>(this, "navigateBack", (sender) => { MainPage = appshell; });
            MessagingCenter.Subscribe<SettingsPage>(this, "navigateBack", (sender) => { MainPage = appshell; });
            MessagingCenter.Subscribe<ViewAProfilePage>(this, "navigateBack", (sender) => { if (OriginPage == "View All Profiles Page") MainPage = appshell; else MainPage = appshell; });
            MessagingCenter.Subscribe<ChatPage>(this, "navigateBack", (sender) => { if (OriginPage == "View A Profile Page") MainPage.Navigation.PopAsync(); else MainPage = appshell; CurrentPage = null; });
            MessagingCenter.Subscribe<ProfileImagePage>(this, "navigateBack", (sender) => { MainPage.Navigation.PopAsync(); });
            MessagingCenter.Subscribe<AnalyticsPage>(this, "navigateBack", (sender) => { if (OriginPage == "App Shell") { MainPage = appshell; OriginPage = null; } else MainPage.Navigation.PopAsync(); });
            #endregion

            #region Other
            MessagingCenter.Subscribe<NotificationsPage>(this, "currentPageApp", (sender) => { CurrentPage = "Notifications Page"; });
            MessagingCenter.Subscribe<NotificationsPage>(this, "clearCurrentPage", (sender) => { CurrentPage = null; });
            MessagingCenter.Subscribe<AllMessagesPage>(this, "currentPageApp", (sender) => { CurrentPage = "All Messages Page"; });
            MessagingCenter.Subscribe<AllMessagesPage>(this, "clearCurrentPage", (sender) => { if(CurrentPage != "Chat Page") CurrentPage = null; });
            MessagingCenter.Subscribe<ChatPage>(this, "clearCurrentPage", (sender) => { CurrentPage = null; });
            MessagingCenter.Subscribe<LoginPageViewModel>(this, "loadUserData", async(sender) => { appshell = new AppShell(); MainPage = appshell; await CheckConnectivity(); });
            MessagingCenter.Subscribe<string>(this, "AppLoggedIn", async(sender) => { await LogInSession(); });
            #endregion

        }

        static public async Task CheckConnectivity()
        {
            CheckingConnectivity = true;

            if(!ForceReconnectOnce) //If not forced to reconnect once, access the network
                await AccessNetwork();

            //Do a recursive try of connecting and break only if the connection is established
            while (!AppClosed && !NetworkStatus)
            {
                try
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
                    else if (NetworkStatus)
                    {
                        CrossToastPopUp.Current.ShowCustomToast("Connected!", "#838181", "#FFFFFF", Plugin.Toast.Abstractions.ToastLength.Short);
                        return;
                    }
                }
                catch(Exception ex)
                {
                    await Task.Delay(1000); //Await looper prepare to complete
                }
                ForceReconnectOnce = false; //Reconnect now only everytime the network is still false
            }
        }

        private static async Task AccessNetwork()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                /*
                var ping = new Ping();
                var isconnected = await ping.SendPingAsync("google.com", 5000);
                if (isconnected.Status == IPStatus.Success)
                {
                    NetworkStatus = true;
                    if (IsUserLoggedIn || StayLoggedIn)
                    {
                        await HubConnection.StartAsync();
                        await HubConnection.InvokeAsync("OnConnected", CurrentUser, CurrentFirstName);
                    }
                }
                else
                {
                    await HubConnection.StopAsync();
                    NetworkStatus = false;
                }*/
                //NetworkStatus = true;
                await HubConnection.StartAsync();
                await HubConnection.InvokeAsync("OnConnected", CurrentUser, CurrentFirstName);
            }
            else
            {
                await HubConnection.StopAsync();
                NetworkStatus = false;
                MessagingCenter.Send("App", "updateConnection"); //Send to app shell view model
            }
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (!CheckingConnectivity)
                await CheckConnectivity();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            
            AppClosed = false;
            if(!CheckingConnectivity && !NetworkStatus)
                await CheckConnectivity();
            if (IsUserLoggedIn || StayLoggedIn)
                 await LogInSession();

            //await HubConnection.StartAsync();
            //MessagingCenter.Subscribe<AppShellViewModel>(this, "connect", async(sender) => await HubConnection.InvokeAsync("OnConnected", CurrentUser, CurrentKawanUser.FirstName)); //Only connect once the current user details are done loading
        }

        protected override async void OnSleep()
        {
            // Handle when your app sleeps
            await LogOutSession();
            AppClosed = true;
        }

        protected override async void OnResume()
        {
            // Handle when your app resumes
            AppClosed = false;
            if (!CheckingConnectivity && !NetworkStatus)
                await CheckConnectivity();
            if (IsUserLoggedIn || StayLoggedIn)
                await LogInSession();
        }

        async Task LogInSession() 
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

        async Task LogOutSession()
        {
            if (!string.IsNullOrEmpty(CurrentSessionId))
            {
                if (CurrentSessionId == "0")
                {
                    System.Console.WriteLine("Not finished logging in session yet!");
                    return;
                }
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

        static async Task GetPreferences()
        {
            StayLoggedIn = Preferences.Get("StayLoggedIn", false);

            IsUserLoggedIn = StayLoggedIn == false
                ? IsUserLoggedIn = false
                : Preferences.Get("IsUserLoggedIn", false);

            CurrentUser = Preferences.Get("CurrentUser", null);
            CurrentFirstName = Preferences.Get("CurrentFirstName", null);
            CurrentPic = Preferences.Get("CurrentPic", null);
            CurrentUserType = Preferences.Get("CurrentUserType", null);
            MessagingCenter.Send("App", "setCurrentUserType"); //Send to App Shell View Model
            return;
        }

        static async void ShowLoginPage(bool isUserLoggedIn)
        {
            if (!isUserLoggedIn)
            {
                App.Current.MainPage?.Navigation.PushModalAsync(new LoginPage());
                Preferences.Set("IsUserLoggedIn", false);
                Preferences.Set("StayLoggedIn", false);
                await HubConnection.StopAsync();
                await CheckConnectivity();
            }
            else
            {
                App.Current.MainPage?.Navigation.PopModalAsync();
                MessagingCenter.Send("App", "AppLoggedIn");
            }

        }
    }
}
