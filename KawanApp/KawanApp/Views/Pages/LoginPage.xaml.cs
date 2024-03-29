﻿using KawanApp.ViewModels.Pages;
using System;
using Xamarin.Forms;

namespace KawanApp.Views.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginPageViewModel();
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

        private void SignUp_Tapped(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            lbl.TextColor = Color.FromHex("#551A8B");
            MessagingCenter.Send(this, "navigateToSignUp");
        }
    }
}