using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class AddActivitiesPageViewModel : BaseViewModel
    {
        private ObservableCollection<Activity> _newActivities;
        public ObservableCollection<Activity> NewActivities
        {
            get => _newActivities;
            set 
            {
                _newActivities = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddActivities { get; set; }
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public AddActivitiesPageViewModel()
        {
            AddActivities = new Command(() => SubmitActivities());
        }

        private async void SubmitActivities()
        {
            ReplyMessage rm;
            ActivitiesForServer NewActivitiesForServer = new ActivitiesForServer() { Activities = new List<Activity>(NewActivities) };
            if (App.NetworkStatus)
                rm = await ServerApi.StoreActivities(NewActivitiesForServer);
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }

            if (rm.Status)
                await App.Current.MainPage.DisplayAlert("Success", "Activities logged successfully!", "Ok");
            else
                await App.Current.MainPage.DisplayAlert("Error", rm.Message, "Ok");
        }
    }
}