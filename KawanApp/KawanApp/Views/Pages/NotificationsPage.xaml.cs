using KawanApp.Models;
using KawanApp.ViewModels.Pages;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KawanApp.Views.Pages
{
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
            this.BindingContext = new NotificationsPageViewModel();
        }
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, "currentPage"); //Send to AppShell View Model
            MessagingCenter.Send(this, "currentPageApp"); //Send to App.xaml.cs
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, "clearCurrentPage"); //Send to App.xaml.cs
            base.OnAppearing();
        }

        private async void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;

            Notification n;
            n = (Notification)e.Item;

            MessagingCenter.Send(this, "notificationRead", n.NotificationId); //Send to view model

            switch (n.Title)
            {
                case "Friend":
                    await Shell.Current.GoToAsync($"//corepages/profiles/profiles", true);
                    await Task.Delay(500); //Let the page construct first
                    MessagingCenter.Send(this, "initiateNavigateToViewAProfilePage", n.SendingUser); //Send to View All Profiles View Model
                    break;
                case "Message":
                    await Shell.Current.GoToAsync($"//corepages/allmessages/allmessages", true);
                    await Task.Delay(500); //Let the page construct first
                    MessagingCenter.Send(this, "initiateNavigateToChatPage", n.SendingUser); //Send to All Messages View Model
                    break;
                case "Activity Logged":
                    MessagingCenter.Send(this, "navigateToSatisfactoryFormsPage"); //Send to App.xaml.cs
                    break;
            }
        }
    }
}