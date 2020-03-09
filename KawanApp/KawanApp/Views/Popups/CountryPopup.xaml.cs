using KawanApp.Models;
using KawanApp.Services;
using KawanApp.ViewModels.Popups;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
            if (!string.IsNullOrEmpty(DataService.Country))
                for (int x = 0; x < locd.Count; x++)
                    if (locd[x].CountryName == DataService.Country)
                        FlagPicker.SelectedIndex = x;
        }

        protected override void OnDisappearing()
        {
            if (string.IsNullOrEmpty(DataService.Country))
            {
                DataService.Country = null;
                MessagingCenter.Send(this, "clearSearch"); //Send to ViewAllProfilesPageViewModel to clear the search and display all users
            }
            base.OnDisappearing();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(e.NewTextValue))
                FlagPicker.SelectedIndex = -1;
            DataService.Country = e.NewTextValue;
            ObservableCollection<KawanUser> SearchResults = DataService.GetSearchResults(e.NewTextValue);
            MessagingCenter.Send(this, "updateList", SearchResults); //Send to ViewAllProfilesPageViewModel to filter the list
        }
        private void Ok_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}