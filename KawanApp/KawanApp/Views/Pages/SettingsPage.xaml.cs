using KawanApp.Views.Popups;
using Microsoft.AspNetCore.SignalR.Client;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;

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

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await App.HubConnection.InvokeAsync("OnDisconnected", App.CurrentUser);
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