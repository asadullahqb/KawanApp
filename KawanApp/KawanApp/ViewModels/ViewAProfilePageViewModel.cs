using KawanApp.Helpers;
using KawanApp.Interfaces;
using KawanApp.Models;
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
        private bool _isOwnProfile;
        private KawanUser _kawanUser;
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
        public ViewAProfilePageViewModel()
        {
            IsOwnProfile = true;
            FetchDataFromServer();
        }

        private async void FetchDataFromServer()
        {
            User u = new User() { StudentId = App.CurrentUser, Type = App.CurrentUserType };
            KawanUser = await ServerApi.FetchCurrentKawanUser(u);
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
        }

        public ViewAProfilePageViewModel(KawanUser KawanData)
        {
            IsOwnProfile = false;
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
    }
}