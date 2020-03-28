using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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