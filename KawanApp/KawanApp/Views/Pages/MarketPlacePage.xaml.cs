using Xamarin.Forms;

namespace KawanApp.Views.Pages
{
    public partial class MarketPlacePage : ContentPage
    {
        public MarketPlacePage()
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