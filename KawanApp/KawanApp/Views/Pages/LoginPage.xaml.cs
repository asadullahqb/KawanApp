using KawanApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginPageViewModel();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Send<LoginPage>(this, "loadUserData"); //Send to ViewAllProfilesViewModel.cs
            base.OnDisappearing();
        }
        protected override bool OnBackButtonPressed()
        {
            // Back button on android doesn't close login page
            return false;
        }

        private void OnStudentIdReturnCommand()
        {
            if (StudentIdEntry.IsFocused)
                PasswordEntry.Focus();
            return;
        }
    }
}