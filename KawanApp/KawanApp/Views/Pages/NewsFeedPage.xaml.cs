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