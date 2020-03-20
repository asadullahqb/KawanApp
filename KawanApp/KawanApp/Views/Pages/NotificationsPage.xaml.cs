using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
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
    }
}