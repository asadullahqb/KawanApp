using FluentValidation.Results;
using KawanApp.Helpers;
using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class ViewAllProfilesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<KawanUser> _allKawanUsers;
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public ObservableCollection<KawanUser> AllKawanUsers
        {
            get => _allKawanUsers;
            set
            {
                _allKawanUsers = value;
                OnPropertyChanged();
            }
        }

        public ViewAllProfilesViewModel()
        {
            FetchAllKawanUsers();

        }

        private async void FetchAllKawanUsers()
        {
            List<KawanUser> AllKawanUsersFromDb = new List<KawanUser>();
            
            try
            {
                AllKawanUsersFromDb = await ServerApi.FetchAllKawanUsers();
            }
            catch (Refit.ApiException ex)
            {
                if (ex.Message.Contains("404"))
                    await App.Current.MainPage.DisplayAlert("Error", "Failed to connect to server.", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "JSON parsing error.", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

            ObservableCollection<KawanUser> temp = new ObservableCollection<KawanUser>(AllKawanUsersFromDb as List<KawanUser>);
            AllKawanUsers = temp;

        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}