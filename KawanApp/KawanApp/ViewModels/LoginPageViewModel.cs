using FluentValidation.Results;
using KawanApp.Helpers;
using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string _studentId;
        private string _currentUserType;
        private string _password;
        private bool _stayLoggedIn;
        private bool _isLoadingVisible;
        private bool _isValid;
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public string StudentId
        {
            get => _studentId;
            set 
            {
                _studentId = value; 
                OnPropertyChanged(); 
                ValidateEntries(); 
            }
        }

        public string CurrentUserType
        {
            get => _currentUserType;
            set
            {
                _currentUserType = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set 
            { 
                _password = value; 
                OnPropertyChanged(); 
                ValidateEntries(); 
            }
        }

        public bool StayLoggedIn
        {
            get => _stayLoggedIn;
            set 
            { 
                _stayLoggedIn = value; 
                OnPropertyChanged(); 
            }
        }

        public bool IsLoadingVisible
        {
            get => _isLoadingVisible;
            set
            { 
                _isLoadingVisible = value; 
                OnPropertyChanged(); 
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        UserAuthValidator Validator { get; set; } = new UserAuthValidator();
        ValidationResult ValidationResult { get; set; } = new ValidationResult();

        public ICommand OnLoginCommand { get; set; }

        public LoginPageViewModel()
        {
            Preferences.Clear();
            OnLoginCommand = new Command(() => { Login(); });
        }

        private async void Login()
        {
            try
            {
                IsLoadingVisible = true;
                IsValid = false;

                var UserToLogin = new UserAuthentication(StudentId, Password);
                LoginReply message = new LoginReply();

                if(App.NetworkStatus)
                    message = await ServerApi.Login(UserToLogin);
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                    return;
                }

                if (message.Status)
                {
                    CurrentUserType = message.UserType;
                    UpdateStateData();
                    MessagingCenter.Send<LoginPageViewModel>(this, "loadUserData"); //Send to ViewAllProfilesViewModel.cs
                }
                else
                    await App.Current.MainPage.DisplayAlert("Error", "Wrong username or password.", "Ok");
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "no internet connection", "Ok");
            }
            finally
            {
                IsLoadingVisible = false;
                IsValid = true;
            }
        }

        private void UpdateStateData()
        {
            App.CurrentUser = StudentId;
            App.CurrentUserType = CurrentUserType;
            App.IsUserLoggedIn = true;
            Preferences.Set("CurrentUser", StudentId);
            Preferences.Set("CurrentUserType", CurrentUserType);
            Preferences.Set("IsUserLoggedIn", true);
            Preferences.Set("StayLoggedIn", StayLoggedIn);
        }

        private void ValidateEntries()
        {
            var UserToLogin = new UserAuthentication(StudentId, Password);
            ValidationResult = Validator?.Validate(UserToLogin);
            IsValid = ValidationResult.IsValid;
        }
    }
}