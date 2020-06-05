using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Services;
using KawanApp.Views.Pages;
using Refit;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class ViewAProfilePageViewModel : BaseViewModel
    {
        private bool _isOwnProfile = true;
        private bool _isLoading = true;
        private KawanUser _kawanUser = new KawanUser() { Type = "International Student" };
        private HtmlWebViewSource _aboutMeSource;
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public bool IsOwnProfile
        {
            get => _isOwnProfile;
            set
            {
                _isOwnProfile = value;
                OnPropertyChanged();
            }
        }

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

        public HtmlWebViewSource AboutMeSource
        {
            get => _aboutMeSource;
            set
            {
                _aboutMeSource = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditCommand { get; set; }
        public ICommand ProfileImageCommand { get; set; }
        public ViewAProfilePageViewModel()
        {
            KawanUser = App.CurrentKawanUser;
            IsOwnProfile = true;
            EditCommand = new Command(() => { MessagingCenter.Send(this, "navigateToEditPage", KawanUser); }); //Send to App.xaml.cs
            ProfileImageCommand = new Command(() => { MessagingCenter.Send(this, "navigateToProfileImagePage", new ProfileImageFields() { IsOwnProfile = true, Pic = KawanUser.Pic }); }); //Send to App.xaml.cs
            MessagingCenter.Subscribe<SignUpPageViewModel>(this, "updateAfterEdit", (sender) => {
                KawanUser = new KawanUser();
                KawanUser = App.CurrentKawanUser;
                AboutMeSource = new HtmlWebViewSource
                {
                    Html = "<html>" +
                    "<body  style=\"font-size:14px; color:#9C9A9B; text-align: justify;\">" +
                    String.Format("<p>{0}</p>", KawanUser.AboutMe) +
                    "</body>" +
                    "</html>"
                };
            });
            MessagingCenter.Subscribe<ProfileImagePage, string>(this, "updatePic", (sender, picture) => { KawanUser ku = KawanUser; ku.Pic = picture; KawanUser = ku; });
            AboutMeSource = new HtmlWebViewSource
            {
                Html = "<html>" +
                    "<body  style=\"font-size:14px; color:#9C9A9B; text-align: justify;\">" +
                    String.Format("<p>{0}</p>", KawanUser.AboutMe) +
                    "</body>" +
                    "</html>"
            };
            IsLoading = false;
        }

        public ViewAProfilePageViewModel(string StudentId)
        {
            KawanUser = App.CurrentKawanUser;
            IsOwnProfile = false;

            FetchUserDataFromServer(StudentId);

            ProfileImageCommand = new Command(() => { MessagingCenter.Send(this, "navigateToProfileImagePage", new ProfileImageFields() { IsOwnProfile = false, Pic = KawanUser.Pic }); }); //Send to App.xaml.cs
            MessagingCenter.Subscribe<string>(this, "updateProfiles", async (sender) => { await Task.Delay(1500); KawanUser = DataService.AllUsers[KawanUser.Index]; });
            IsLoading = false;
        }

        private async void FetchUserDataFromServer(string StudentId)
        {
            IsLoading = true;
            if (!App.NetworkStatus)
                await App.CheckConnectivity();
            User u = new User() { StudentId = StudentId, Type = (App.CurrentUserType == "Kawan")? "International Student" : "Kawan" };
            if (App.NetworkStatus)
                KawanUser = await ServerApi.FetchCurrentKawanUser(u);
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            await Task.Run(() =>
            {
                AboutMeSource = new HtmlWebViewSource
                {
                    Html = "<html>" +
                    "<body  style=\"font-size:14px; color:#9C9A9B; text-align: justify;\">" +
                    String.Format("<p>{0}</p>", KawanUser.AboutMe) +
                    "</body>" +
                    "</html>"
                };
            });
            await Task.Run(() => IsLoading = false);
        }

        public ViewAProfilePageViewModel(KawanUser KawanData)
        {
            ProfileImageCommand = new Command(() => { MessagingCenter.Send(this, "navigateToProfileImagePage", new ProfileImageFields() { IsOwnProfile = false, Pic = KawanUser.Pic }); }); //Send to App.xaml.cs   
            IsOwnProfile = false;
            IsLoading = false;
            KawanUser = KawanData;
            AboutMeSource = new HtmlWebViewSource
            {
                Html = "<html>" +
                    "<body  style=\"font-size:14px; color:#9C9A9B; text-align: justify;\">" +
                    String.Format("<p>{0}</p>", KawanUser.AboutMe) +
                    "</body>" +
                    "</html>"
            };
            MessagingCenter.Subscribe<string>(this, "updateProfiles", async(sender) => { await Task.Delay(1500); KawanUser = DataService.AllUsers[KawanUser.Index]; });
        }
    }
}