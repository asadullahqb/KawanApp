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
    public partial class AnalyticsPage : ContentPage
    {
        public AnalyticsPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }
    }
}