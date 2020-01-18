using KawanApp.Models;
using KawanApp.Services;
using KawanApp.ViewModels.Popups;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Popups
{
    public partial class CountryPopup : PopupPage
    {
        public CountryPopup(ObservableCollection<Country> locd)
        {
            InitializeComponent();
            this.BindingContext = new CountryPopupViewModel(locd);
        }

        protected override void OnDisappearing()
        {
            if (string.IsNullOrEmpty(DataService.Country))
            {
                DataService.Country = null;
                MessagingCenter.Send<CountryPopup>(this, "clearSearch"); //Send to ViewAllProfilesViewModel to clear the search and display all users
            }
            base.OnDisappearing();
        }

        private void CountryEntry_Focused(object sender, FocusEventArgs e)
        {
            MainFrame.Margin = new Thickness(40, 80, 40, 60);
        }

        private void CountryEntry_Unfocused(object sender, FocusEventArgs e)
        {
            MainFrame.Margin = new Thickness(40, 80, 40, 320);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataService.Country = e.NewTextValue;
            ObservableCollection<KawanUser> SearchResults = DataService.GetSearchResults(e.NewTextValue);
            MessagingCenter.Send<CountryPopup, ObservableCollection<KawanUser>>(this, "updateList", SearchResults); //Send to ViewAllProfilesViewModel to filter the list
        }
    }
}