using KawanApp.Models;
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
    public partial class ActivitiesPage : ContentPage
    {
        public ActivitiesPage()
        {
            InitializeComponent();
            this.BindingContext = new ActivitiesPageViewModel();
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

        private void AddActivity_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new AddActivitiesPage());
        }

        private void ViewProfile(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            MessagingCenter.Send(this, "navigateToViewAProfilePage", btn.ClassId); //Send to App.xaml.cs
        }

        private void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}