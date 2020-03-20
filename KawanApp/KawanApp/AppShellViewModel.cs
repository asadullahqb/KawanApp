using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.ViewModels;
using KawanApp.Views.Pages;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KawanApp
{
    public class AppShellViewModel : BaseViewModel
    {
        private bool _isLoading;
        private KawanUser _kawanUser;
        private Color _titleColour = Color.FromHex("#f68712");
        private string _currentUserType;
        private Color _onlineColor = Color.Red;
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public KawanUser KawanUser
        {
            get => _kawanUser;
            set
            {
                _kawanUser = value;
                OnPropertyChanged();
            }
        }

        public Color TitleColour
        {
            get => _titleColour;
            set
            {
                _titleColour = value;
                OnPropertyChanged();
            }
        }
        public string CurrentUserType
        {
            get => _currentUserType;
            set
            {
                _currentUserType = value;
                OnPropertyChanged();
            }
        }

        public Color OnlineColor
        {
            get => _onlineColor;
            set
            {
                _onlineColor = value;
                OnPropertyChanged();
            }
        }
        public ICommand OnProfileCommand { get; set; }

        public AppShellViewModel()
        {
            OnProfileCommand = new Command(() => { if (!IsLoading) { Shell.Current.FlyoutIsPresented = false; MessagingCenter.Send(this, "navigateToViewAProfilePage"); } }); // Send to App.xaml.cs

            MessagingCenter.Subscribe<string>(this, "updateConnection", async(sender) => { await Task.Delay(1000); if (App.NetworkStatus) OnlineColor = Color.Green; else OnlineColor = Color.Red; });
            MessagingCenter.Subscribe<ProfileImagePage, string>(this, "updatePic", (sender, picture) => { KawanUser ku = KawanUser; ku.Pic = picture; KawanUser = ku; App.CurrentPic = picture; Preferences.Set("CurrentPic", picture); });
            MessagingCenter.Subscribe<SignUpPageViewModel>(this, "updateAfterEdit", (sender) => { CurrentUserType = App.CurrentUserType; KawanUser ku = KawanUser; ku.FirstName = App.CurrentFirstName; KawanUser = ku; });
            MessagingCenter.Subscribe<LoginPageViewModel>(this, "loadUserData", (sender) => { CurrentUserType = App.CurrentUserType; FetchDataFromServer(); });
            //Set the title colour to the current page
            MessagingCenter.Subscribe<NewsFeedPage>(this, "currentPage", (sender) => { TitleColour = Color.FromHex("#f68712"); }); //Orange
            MessagingCenter.Subscribe<ViewAllProfilesPage>(this, "currentPage", (sender) => { TitleColour = Color.FromHex("#8d198f"); }); //Purple
            MessagingCenter.Subscribe<NotificationsPage>(this, "currentPage", (sender) => { TitleColour = Color.FromHex("#ed127c"); }); //Pink
            MessagingCenter.Subscribe<AllMessagesPage>(this, "currentPage", (sender) => { TitleColour = Color.FromHex("#3871c1"); }); //Blue
            MessagingCenter.Subscribe<MarketPlacePage>(this, "currentPage", (sender) => { TitleColour = Color.FromHex("#80cc28"); }); //Green

            CurrentUserType = App.CurrentUserType;
            FetchDataFromServer();
        }

        private async void FetchDataFromServer()
        {
            IsLoading = true;
            if (!App.NetworkStatus)
                await App.CheckConnectivity();
            User u = new User() { StudentId = App.CurrentUser, Type = App.CurrentUserType };
            if (App.NetworkStatus)
                KawanUser = await ServerApi.FetchCurrentKawanUser(u);
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            await Task.Run(() => App.CurrentKawanUser = KawanUser);
            await Task.Run(() => IsLoading = false);
            //await Task.Run(() => MessagingCenter.Send(this, "connect"));
        }
    }
}