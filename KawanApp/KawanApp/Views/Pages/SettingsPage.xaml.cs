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
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<SettingsPage>(this, "navigateToLoginPage"); //Send to App.xaml.cs
        }
    }
}