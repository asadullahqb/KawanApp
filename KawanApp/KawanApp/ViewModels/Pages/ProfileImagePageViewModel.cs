using KawanApp.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class ProfileImagePageViewModel : BaseViewModel
    {
        private string _pic; 
        private bool _isOwnProfile;
        public string Pic
        {
            get => _pic;
            set
            {
                _pic = value;
                OnPropertyChanged();
            }
        }
        public bool IsOwnProfile
        {
            get => _isOwnProfile;
            set
            {
                _isOwnProfile = value;
                OnPropertyChanged();
            }
        }
        public ProfileImagePageViewModel(bool isownprofile, string pic)
        {
            Pic = pic;
            IsOwnProfile = isownprofile;
            MessagingCenter.Subscribe<ProfileImagePage, string>(this, "updatePic", (sender, picture) => { Pic = picture; });
        }
    }
}