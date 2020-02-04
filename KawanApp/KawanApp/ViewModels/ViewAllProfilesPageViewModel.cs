using FFImageLoading.Transformations;
using FFImageLoading.Work;
using FluentValidation.Results;
using KawanApp.Helpers;
using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Services;
using KawanApp.ViewModels.Popups;
using KawanApp.Views.Pages;
using KawanApp.Views.Popups;
using Refit;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class ViewAllProfilesPageViewModel : BaseViewModel
    {
        private ObservableCollection<KawanUser> _allUsers;
        private bool _isRefreshing = false;
        private bool _isKawanTitleVisible;
        private bool _isInternationalStudentsTitleVisible;
        private ObservableCollection<Country> _listOfCountryData;
        private string _isSearchedCountry = null;
        private string _searchedCountry = null;
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public ObservableCollection<KawanUser> AllUsers
        {
            get => _allUsers;
            set
            {
                _allUsers = value;
                OnPropertyChanged();
            }
        }
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        public bool IsKawanTitleVisible
        {
            get { return _isKawanTitleVisible; }
            set
            {
                _isKawanTitleVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsInternationalStudentsTitleVisible
        {
            get { return _isInternationalStudentsTitleVisible; }
            set
            {
                _isInternationalStudentsTitleVisible = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Country> ListOfCountryData
        {
            get => _listOfCountryData;
            set
            {
                _listOfCountryData = value;
                OnPropertyChanged();
            }
        }
        public string IsSearchedCountry
        {
            get => _isSearchedCountry;
            set
            {
                _isSearchedCountry = value;
                OnPropertyChanged();
            }
        }

        public string SearchedCountry
        {
            get => _searchedCountry;
            set
            {
                _searchedCountry = value;
                OnPropertyChanged();
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    if(App.NetworkStatus)
                    {
                        await Task.Run(() => FetchAllUsers());
                        await Task.Run(() => FetchCountriesList());
                        DataService.Country = null;
                        SetCountryViewParameters();
                    }

                    IsRefreshing = false;
                });
            }
        }

        public ICommand OnCountryTappedCommand { get; set; }

        public ViewAllProfilesPageViewModel()
        {
            OnCountryTappedCommand = new Command(()=> { CountryTapped(); });
            MessagingCenter.Subscribe<LoginPageViewModel>(this, "loadUserData", (sender) => { FetchAllUsers(); FetchCountriesList(); });
            MessagingCenter.Subscribe<CountryPopup, ObservableCollection<KawanUser>>(this, "updateList", (sender, SearchResults) => { AllUsers = SearchResults; SetCountryViewParameters(); });
            MessagingCenter.Subscribe<CountryPopup>(this, "clearSearch", (sender) => { AllUsers = DataService.AllUsers; SetCountryViewParameters(); });
            MessagingCenter.Subscribe<ViewAProfilePage>(this, "updateData", (sender) => 
            { 
                if (!string.IsNullOrEmpty(DataService.Country)) 
                { 
                    AllUsers = DataService.GetSearchResults(DataService.Country);
                    SetCountryViewParameters();
                } 
                else 
                {
                    AllUsers = DataService.GetSearchResults("Search"); //Simulate a change in the data so that the view updates
                    AllUsers = DataService.GetSearchResults("");
                    SetCountryViewParameters();
                } 
            });
            FetchAllUsers();
            FetchCountriesList();
            SetCountryViewParameters();
        }

        private void SetCountryViewParameters()
        {
            if (!string.IsNullOrEmpty(DataService.Country))
            {
                IsSearchedCountry = ": ";
                SearchedCountry = DataService.Country;
            }
            else
            {
                IsSearchedCountry = string.Empty;
                SearchedCountry = string.Empty;
            }
        }

        private async void FetchCountriesList()
        {
            List<Country> ListOfCountriesFromDb;
            User u;

            if (App.CurrentUserType == "International Student")
                u = new User() { Type = "Kawan" };
            else if (App.CurrentUserType == "Kawan")
                u = new User() { Type = "International Student" };
            else
                return;

            if (App.NetworkStatus)
            {
                ListOfCountriesFromDb = await ServerApi.FetchListOfCountries(u);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }

            ObservableCollection<Country> temp = new ObservableCollection<Country>(ListOfCountriesFromDb.OrderBy(c => c.CountryName));
            for(int i=0; i<temp.Count; i++)
            {
                if(temp[i].CountryName == "")
                {
                    temp.RemoveAt(i);
                    i--;
                }
            }
            ListOfCountryData = temp;
        }

        private void CountryTapped()
        {
            PopupNavigation.Instance.PushAsync(new CountryPopup(ListOfCountryData));
        }

        private async void FetchAllUsers()
        {
            if (App.CurrentUserType == "International Student")
            {
                IsInternationalStudentsTitleVisible = false;
                IsKawanTitleVisible = true;
                List<KawanUser> AllKawanUsersFromDb;
                User u = new User() { StudentId = App.CurrentUser };

                if (App.NetworkStatus)
                    AllKawanUsersFromDb = await ServerApi.FetchAllKawanUsers(u);
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                    AllKawanUsersFromDb = new List<KawanUser>();
                }

                ObservableCollection<KawanUser> temp = new ObservableCollection<KawanUser>(AllKawanUsersFromDb as List<KawanUser>);
                AllUsers = temp;
                DataService.AllUsers = AllUsers;
            }
            else if(App.CurrentUserType == "Kawan")
            {
                IsInternationalStudentsTitleVisible = true;
                IsKawanTitleVisible = false;
                List<User> AllInternationalStudentUsersFromDb;
                User u = new KawanUser() { StudentId = App.CurrentUser };

                if (App.NetworkStatus)
                    AllInternationalStudentUsersFromDb = await ServerApi.FetchAllInternationalStudentUsers(u);
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                    AllInternationalStudentUsersFromDb = new List<User>(); ;
                }
                
                ObservableCollection<User> temp = new ObservableCollection<User>(AllInternationalStudentUsersFromDb as List<User>);
                ObservableCollection<KawanUser> tempku = new ObservableCollection<KawanUser>();
                foreach(User user in temp) //parse user into kawan user to be used with the XAML.
                {
                    KawanUser ku = new KawanUser()
                    {
                        AboutMe = user.AboutMe,
                        AverageResponseTime = "",
                        Campus = user.Campus,
                        Country = user.Country,
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        FriendStatus = user.FriendStatus,
                        Gender = user.Gender,
                        Index = user.Index,
                        LastName = user.LastName,
                        PhoneNum = user.PhoneNum,
                        Pic = user.Pic,
                        Rating = 0.00,
                        School = user.School,
                        StudentId = user.StudentId,
                        Type = user.Type
                    };
                    tempku.Add(ku);
                }
                AllUsers = tempku;
                DataService.AllUsers = AllUsers;
            }
        }
    }
}