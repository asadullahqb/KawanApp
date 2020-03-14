using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Views.Popups;
using Refit;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }

        private void BackIcon_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "navigateToLoginPage"); //Send to App.xaml.cs
        }

        private async void Password_Clicked(object sender, EventArgs e)
        {
            var accepted = await DisplayAlert("Note", "Would you like to change your password?", "Yes", "No");
            if(accepted)
                if (PopupNavigation.Instance.PopupStack.Count < 1) 
                    await PopupNavigation.Instance.PushAsync(new UpdatePasswordPopup());
        }
    }
}