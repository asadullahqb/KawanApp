using KawanApp.ViewModels;
using KawanApp.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    public partial class AnalyticsPage : ContentPage
    {
        public AnalyticsPage(string kawanuserstudentid)
        {
            InitializeComponent();
            this.BindingContext = new AnalyticsPageViewModel(kawanuserstudentid);
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }

        private void BackIcon_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
        }
    }
}