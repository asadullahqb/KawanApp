using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Popups
{
    public class UpdatePasswordPopupViewModel : BaseViewModel
    {
        private KawanUser _kawanUser = new KawanUser();
        private string _confirmPassword;
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);

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

        public ICommand SaveCommand { get; set; }


        public UpdatePasswordPopupViewModel()
        {
            SaveCommand = new Command(() => { OnSave(); });
        }

        private async void OnSave()
        {
            KawanUser.StudentId = App.CurrentUser;
            KawanUser.Type = App.CurrentUserType;
            if (string.IsNullOrEmpty(KawanUser.Password) ||
                string.IsNullOrEmpty(ConfirmPassword) ||
                string.IsNullOrEmpty(KawanUser.CurrentPassword)
              ) // Make sure all fields are filled in
                await App.Current.MainPage.DisplayAlert("Note", "Please fill out all fields!", "Ok");

            else if (KawanUser.Password.Length < 6)
                await App.Current.MainPage.DisplayAlert("Note", "Password must be at least 6 characters long!", "Ok");

            else if (ConfirmPassword != KawanUser.Password) //Make sure password == confirm password
                await App.Current.MainPage.DisplayAlert("Note", "Password is not same as confirmed password!", "Ok");
            else
            {
                var rm = await ServerApi.UpdatePassword(KawanUser);
                if (rm.Status)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Your password has been updated!", "Ok");
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                    await App.Current.MainPage.DisplayAlert("Failure", rm.Message, "Ok");
                
            }
        }
    }
}