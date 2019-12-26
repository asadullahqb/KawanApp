using FluentValidation.Results;
using KawanApp.Helpers;
using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private bool _stayLoggedIn;
        private bool _isLoadingVisisble;
        private bool _isValid;
        public string Email
        {
            get => _email;
            set 
            { 
                _email = value; 
                OnPropertyChanged(); 
                ValidateEntries(); 
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

        public bool IsLoadingVisisble
        {
            get => _isLoadingVisisble;
            set 
            { 
                _isLoadingVisisble = value; 
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
            OnLoginCommand = new Command(() => {
                try
                {
                    IsLoadingVisisble = true;
                    IsValid = false;

                    var UserToLogin = new UserAuthentication(Email, Password);

                    UpdateStateData();
                    
                    //Insert restful call to authenticate on the server here
                    /*
                    var message = await ServerApi.UserLogin(UserToLogin);

                    if (message.status)
                        UpdateStateData();
                    else
                        App.Current.MainPage.DisplayAlert("Error", "Wrong username or password.", "Ok");
                    */
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Error", "no internet connection", "Ok");
                }
                finally
                {
                    IsLoadingVisisble = false;
                    IsValid = true;
                }
            });
        }

        private void UpdateStateData()
        {
            App.CurrentUser = Email;
            App.IsUserLoggedIn = true;
            Preferences.Set("CurrentUser", Email);
            Preferences.Set("IsUserLoggedIn", true);
            Preferences.Set("StayLoggedIn", StayLoggedIn);
        }

        private void ValidateEntries()
        {
            var UserToLogin = new UserAuthentication(Email, Password);
            ValidationResult = Validator?.Validate(UserToLogin);
            IsValid = ValidationResult.IsValid;
        }
    }
}