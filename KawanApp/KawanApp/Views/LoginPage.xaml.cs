using KawanApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginPageViewModel();
            MessagingCenter.Subscribe<LoginPageViewModel>(this, "OnEmailReturnCommand", (sender) => { OnEmailReturnCommand(); });
        }
        protected override bool OnBackButtonPressed()
        {
            // Back button on android doesn't close login page
            return false;
        }

        private void OnEmailReturnCommand()
        {
            if (EmailEntry.IsFocused)
                PasswordEntry.Focus();
            return;
        }
    }
}