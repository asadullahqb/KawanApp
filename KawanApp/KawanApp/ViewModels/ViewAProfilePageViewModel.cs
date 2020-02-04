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
            KawanUser ku = new KawanUser()
            {
                AboutMe = "Hello everyone! I'm a meow!! I love saying meows",
                AverageResponseTime = "--",
                Campus = "Engineering",
                Country = "Malaysia",
                DateOfBirth = DateTime.Now,
                Email = "asadqb16@gmail.com",
                FirstName = "Asadullah",
                LastName = "Qamar",
                FriendStatus = 2,
                Gender = "Male",
                Index = 0,
                PhoneNum = "0127865652",
                Pic = "uploads/IMG20181221155121.jpg",
                Rating = 4.74,
                StudentId = "132879",
                Type = "International Student",
                School = "School of Computer Sciences"
            };
            KawanUser = ku;
            FetchCurrentUser();
            AboutMeSource = new HtmlWebViewSource
            {
                Html = "<html>" +
                    "<body  style=\"font-size:14px; color:#9C9A9B; text-align: justify;\">" +
                    String.Format("<p>{0}</p>", KawanUser.AboutMe) +
                    "</body>" +
                    "</html>"
            };
        }

        private async void FetchCurrentUser()
        {
            //KawanUser = await ServerApi.FetchCurrentKawanUser;
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