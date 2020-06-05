using System;
using Xamarin.Forms;

namespace KawanApp
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = new AppShellViewModel();
        }

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
