using KawanApp.Models;
using KawanApp.Services;
using System.Collections.ObjectModel;

namespace KawanApp.ViewModels.Popups
{
    public class CountryPopupViewModel : BaseViewModel
    {
        private ObservableCollection<Country> _listOfCountries;
        private Country _selectedCountry;
        public ObservableCollection<Country> ListOfCountries
        {
            get => _listOfCountries;
            set
            {
                _listOfCountries = value;
                OnPropertyChanged();
            }
        }
        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged();
            }
        }

        public CountryPopupViewModel(ObservableCollection<Country> locd)
        {
            ListOfCountries = locd;
            if(!string.IsNullOrEmpty(DataService.Country))
                SelectedCountry = new Country() { CountryName = DataService.Country };
        }
    }
}