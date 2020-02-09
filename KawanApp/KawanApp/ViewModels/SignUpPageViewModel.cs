using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    //This page is reused for edit a profile
    public class SignUpPageViewModel : BaseViewModel
    {
        private bool _isEdit;
        private bool _isSelecting = true;
        private KawanUser _kawanUser = new KawanUser() { AboutMe = "Hi! I am excited to use this app. :)" };
        private string _confirmPassword;

        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);

        public bool IsEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                _isEdit = value;
                OnPropertyChanged();
            }
        }
        public bool IsSelecting
        {
            get
            {
                return _isSelecting;
            }
            set
            {
                _isSelecting = value;
                OnPropertyChanged();
            }
        }
        public KawanUser KawanUser
        {
            get
            {
                return _kawanUser;
            }
            set
            {
                _kawanUser = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand IsKawanCommand { get; set; }
        public ICommand IsInternationalStudentCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        public SignUpPageViewModel()
        {
            IsEdit = false;
            string abtme = "Hi! I am excited to use this app. :)";
            IsKawanCommand = new Command(() => { IsSelecting = false; KawanUser ku = new KawanUser() { Type = "Kawan", AboutMe = abtme }; KawanUser = ku; });
            IsInternationalStudentCommand = new Command(() => { IsSelecting = false; KawanUser ku = new KawanUser() { Type = "International Student", AboutMe = abtme }; KawanUser = ku; });
            SubmitCommand = new Command(() => { SubmitToServer(); });
            GoBackCommand = new Command(() => { IsSelecting = true; });

        }

        

        public SignUpPageViewModel(KawanUser ku)
        {
            IsEdit = true;
            IsSelecting = false;
            string abtme = "Hi! I am excited to use this app. :)";
            if (string.IsNullOrEmpty(KawanUser.AboutMe))
                ku.AboutMe = abtme;
            KawanUser = ku;
            SubmitCommand = new Command(() => { SubmitToServer(); });
            GoBackCommand = new Command(() => { App.Current.MainPage.Navigation.PopModalAsync(); });

        }

        private async void SubmitToServer()
        {
            KawanUser ku = KawanUser;
            if (string.IsNullOrEmpty(KawanUser.StudentId) || string.IsNullOrEmpty(KawanUser.FirstName) ||
                string.IsNullOrEmpty(KawanUser.LastName) || string.IsNullOrEmpty(KawanUser.Email) ||
                string.IsNullOrEmpty(KawanUser.Password) || string.IsNullOrEmpty(ConfirmPassword) ||
                string.IsNullOrEmpty(KawanUser.Gender) || string.IsNullOrEmpty(KawanUser.PhoneNum) ||
                string.IsNullOrEmpty(KawanUser.Campus) || string.IsNullOrEmpty(KawanUser.School) ||
                string.IsNullOrEmpty(KawanUser.Country) || string.IsNullOrEmpty(KawanUser.AboutMe)
              ) // Make sure all fields are filled in
                await App.Current.MainPage.DisplayAlert("Note", "Please fill out all fields!", "Ok");

            else if (!Regex.IsMatch(KawanUser.Email, ".*@.*\\..*")) //Make sure email is valid: __@__.__
                await App.Current.MainPage.DisplayAlert("Note", "Please enter a valid email address!", "Ok");

            else if (KawanUser.Password.Length < 6)
                await App.Current.MainPage.DisplayAlert("Note", "Password must be at least 6 characters long!", "Ok");

            else if (ConfirmPassword != KawanUser.Password) //Make sure password == confirm password
                await App.Current.MainPage.DisplayAlert("Note", "Password is not same as confirmed password!", "Ok");

            else
            {
                await ServerApi.SignUp(KawanUser);
                if (KawanUser.Type.Equals("Kawan"))
                    await App.Current.MainPage.DisplayAlert("Success", "You have been signed up! Please wait for the admin to approve your account so that it may be used to login.", "Ok");
                else if (KawanUser.Type.Equals("International Student"))
                    await App.Current.MainPage.DisplayAlert("Success", "You have been signed up! You may now proceed to login.", "Ok");

            }
        }
    }
}