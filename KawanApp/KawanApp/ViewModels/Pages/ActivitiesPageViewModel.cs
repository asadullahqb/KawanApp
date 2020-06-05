using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class ActivitiesPageViewModel : BaseViewModel
    {
        private ObservableCollection<Activity> _allActivities;
        private bool _isRefreshing;
        public ObservableCollection<Activity> AllActivities
        {
            get => _allActivities;
            set 
            {
                _allActivities = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    if (App.NetworkStatus)
                    {
                        await FetchAllActivities();
                    }

                    IsRefreshing = false;
                });
            }
        }

        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);

        public ActivitiesPageViewModel()
        {
            MessagingCenter.Subscribe<AddActivitiesPageViewModel, ObservableCollection<Activity>>(this, "updateAllActivities", (sender, NewActivities) => { foreach (Activity NewActivity in NewActivities) AllActivities.Insert(0, NewActivity); });
            FetchAllActivities();
        }

        private async Task FetchAllActivities()
        {
            List<Activity> AllActivitiesFromDb;
            if (App.NetworkStatus)
                AllActivitiesFromDb = await ServerApi.FetchAllActivities(new User() { StudentId = App.CurrentUser });
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            var temp = new ObservableCollection<Activity>(AllActivitiesFromDb);
            AllActivities = temp; 
        }
    }
}