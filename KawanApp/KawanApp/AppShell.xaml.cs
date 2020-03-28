using KawanApp.ViewModels;
using KawanApp.Views.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {

        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = new AppShellViewModel();
        }
        /*
        private void Profile_Clicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = false;
            MessagingCenter.Send(this, "navigateToViewAProfilePage"); // Send to App.xaml.cs
        }*/

        private void ActivitiesSatisfactoryForms_Clicked(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            Shell.Current.FlyoutIsPresented = false;
            if(mi.Text == "Activities")
                MessagingCenter.Send(this, "navigateToActivitiesPage");
            else if(mi.Text == "Satisfactory Forms")
                MessagingCenter.Send(this, "navigateToSatisfactoryFormsPage");
        }
        private async void Settings_Clicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = false;
            MessagingCenter.Send(this, "navigateToSettingsPage"); //Send to App.xaml.cs
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}
