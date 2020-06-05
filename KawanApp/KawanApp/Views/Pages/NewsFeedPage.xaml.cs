using Xamarin.Forms;

namespace KawanApp.Views.Pages
{
    public partial class NewsFeedPage : ContentPage
    {
        public NewsFeedPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, "currentPage"); //Send to AppShell View Model
            base.OnAppearing();
        }
    }
}