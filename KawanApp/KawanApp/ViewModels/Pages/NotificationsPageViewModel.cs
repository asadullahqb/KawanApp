using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Views.Pages;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class NotificationsPageViewModel : BaseViewModel
    {
        private ObservableCollection<Notification> _allNotifications;
        private bool _isRefreshing;
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public ObservableCollection<Notification> AllNotifications
        {
            get => _allNotifications;
            set
            {
                _allNotifications = value;
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
                        await FetchNotifications();
                    }

                    IsRefreshing = false;
                });
            }
        }

        public NotificationsPageViewModel()
        {
            MessagingCenter.Subscribe<NotificationsPage, int>(this, "notificationRead", (sender, notificationId) => 
            {
                int i = 0;
                for (; i < AllNotifications.Count; i++)
                    if (AllNotifications[i].NotificationId == notificationId)
                        break;
                Notification n = AllNotifications[i];
                n.IsRead = true;
                AllNotifications[i] = n;
                if(App.NetworkStatus)
                    ServerApi.ReadNotification(n);
            });
            MessagingCenter.Subscribe<string>(this, "updateAllNotifications", async(sender) => { await Task.Delay(1000); await FetchNotifications(); });
            FetchNotifications();
        }

        private async Task FetchNotifications()
        {
            List<Notification> NotificationsFromDb;
            if(App.NetworkStatus)
                NotificationsFromDb = await ServerApi.FetchNotifications(new User() { StudentId = App.CurrentUser, Type = App.CurrentUserType });
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            ObservableCollection<Notification> temp = new ObservableCollection<Notification>(NotificationsFromDb);
            AllNotifications = temp;
        }
    }
}