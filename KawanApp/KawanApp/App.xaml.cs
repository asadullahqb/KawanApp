﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using KawanApp.Models;
using KawanApp.Views.Pages;

namespace KawanApp
{
    public partial class App : Application
    {
        private static bool _isUserLoggedIn;

        public static string Server => "http://192.168.0.157/"; //at Sunny Ville home: http://192.168.0.157/
                                                                //at USM: http://10.212.41.232/
                                                                //at KL home: http://192.168.0.197/

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

        public static bool StayLoggedIn { get; set; }

        public static ObservableCollection<KawanUser> AllKawanUsers { get; set; } 

        public App()
        {
            InitializeComponent();
            GetPreferences();
            var appshell = new AppShell(); //Reuse the same app shell once it's reloaded

            MainPage = appshell;

            if (!IsUserLoggedIn || !StayLoggedIn)
                MainPage.Navigation.PushModalAsync(new LoginPage());

            MessagingCenter.Subscribe<ViewAllProfilesPage, KawanUser>(this, "navigateToViewAProfilePage", (sender, KawanUser) => { MainPage = new NavigationPage(); MainPage.Navigation.PushAsync(new ViewAProfilePage(KawanUser)); });
            MessagingCenter.Subscribe<ViewAProfilePage>(this, "navigateBack", (sender) => { MainPage = appshell; });
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
