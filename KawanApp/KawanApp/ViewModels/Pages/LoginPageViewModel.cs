using FluentValidation.Results;
using KawanApp.Helpers;
using KawanApp.Interfaces;
using KawanApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Refit;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string _studentId;
        private string _currentUserType;
        private string _currentFirstName;
        private string _currentPic;
        private string _password;
        private bool _stayLoggedIn = true; //Set as true by default so that user only has to toggle stay logged in to be off
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
        
        public string CurrentFirstName
        {
            get => _currentFirstName;
            set
            {
                _currentFirstName = value;
                OnPropertyChanged();
            }
        }
        public string CurrentPic
        {
            get => _currentPic;
            set
            {
                _currentPic = value;
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
            App.HubConnection.InvokeAsync("OnDisconnected", App.CurrentUser, App.CurrentFirstName);
            App.CurrentUser = null;
            App.CurrentFirstName = null;
            Preferences.Clear();
            OnLoginCommand = new Command(() => { Login(); });
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Password) || Password.Length < 6 || Password.Length > 20 )
            {
                await App.Current.MainPage.DisplayAlert("Error", "Password must be 6 to 20 characters long!", "Ok");
                return; //This is done to ensure that the return key of the keyboard doesn't bypass
                        //the fluent validator.
            }

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
                    CurrentFirstName = message.CurrentFirstName;
                    UpdateStateData();
                    MessagingCenter.Send(this, "loadUserData"); //Send to AppShell View Model
                }
                else
                    await App.Current.MainPage.DisplayAlert("Error", "Wrong username or password.", "Ok");
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No internet connection", "Ok");
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
            App.CurrentFirstName = CurrentFirstName;
            App.CurrentPic = CurrentPic;
            App.CurrentUserType = CurrentUserType;
            App.IsUserLoggedIn = true;
            Preferences.Set("CurrentUser", StudentId);
            Preferences.Set("CurrentFirstName", CurrentFirstName);
            Preferences.Set("CurrentPic", CurrentPic);
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