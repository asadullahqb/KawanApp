using KawanApp.Helpers;
using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Services;
using KawanApp.Views.Pages;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KawanApp.ViewModels
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
            EditCommand = new Command(() => { MessagingCenter.Send(this, "navigateToEditPage", KawanUser); }); //Send to App.xaml.cs
            ProfileImageCommand = new Command(() => { MessagingCenter.Send(this, "navigateToProfileImagePage", new ProfileImageFields() { IsOwnProfile = true, Pic = KawanUser.Pic }); }); //Send to App.xaml.cs
            IsOwnProfile = true;
            FetchDataFromServer();
            MessagingCenter.Subscribe<SignUpPageViewModel>(this, "updateAfterEdit", (sender) => {
                KawanUser = new KawanUser();
                KawanUser = DataService.KawanUser;
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
        }

        private async void FetchDataFromServer()
        {
            User u = new User() { StudentId = App.CurrentUser, Type = App.CurrentUserType };
            if(App.NetworkStatus)
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
            await Task.Run(() => { IsLoading = false; });
        }

    }
}