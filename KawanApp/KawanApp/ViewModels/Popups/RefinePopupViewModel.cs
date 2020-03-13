using KawanApp.Models;
using KawanApp.Services;
using KawanApp.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Popups
{
    public class RefinePopupViewModel : BaseViewModel
    {
        private ObservableCollection<KawanUser> _allUsers;
        private KawanUser _filterFields = new KawanUser();
        private string _orderBy = "Default";
        private string _sortingOrder = "Ascending";
        private bool _friendsOnly = false;
        private string _userType;

        public ObservableCollection<KawanUser> AllUsers
        {
            get => _allUsers;
            set
            {
                _allUsers = value;
                OnPropertyChanged();
            }
        }
        public KawanUser FilterFields
        {
            get => _filterFields;
            set
            {
                _filterFields = value;
                OnPropertyChanged();
            }
        }
        
        public string OrderBy
        {
            get => _orderBy;
            set
            {
                _orderBy = value;
                OnPropertyChanged();
            }
        }
        
        public string SortingOrder
        {
            get => _sortingOrder;
            set
            {
                _sortingOrder = value;
                OnPropertyChanged();
            }
        }

        public bool FriendsOnly
        {
            get => _friendsOnly;
            set
            {
                _friendsOnly = value;
                OnPropertyChanged();
            }
        }

        public string UserType
        {
            get => _userType;
            set
            {
                _userType = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        public RefinePopupViewModel(ObservableCollection<KawanUser> allusers)
        {
            MessagingCenter.Subscribe<RefinePopup>(this, "updateRefine", (sender) => { if (FilterFields.IsAnyFilterFieldsNotNull) DataService.FilterFields = FilterFields; DataService.OrderBy = OrderBy; DataService.SortingOrder = SortingOrder; FriendsOnly = false; });
            AllUsers = allusers;
            UserType = App.CurrentUserType;
            OrderBy = DataService.OrderBy;
            SortingOrder = DataService.SortingOrder;
            FriendsOnly = DataService.FriendsOnly;
            SearchCommand = new Command(() => 
            {
                if (FilterFields != null) //The object has at least been initialised
                {
                    DataService.FilterFields = FilterFields;
                    DataService.OrderBy = OrderBy;
                    DataService.SortingOrder = SortingOrder;
                    DataService.FriendsOnly = FriendsOnly;
                    MessagingCenter.Send(this, "updateList", DataService.GetFilteredResults(FilterFields)); //Send to ViewAllProfilesPageViewModel to filter the list 
                }
                else
                    return;
            });

            ClearCommand = new Command(() =>
            {
                DataService.FilterFields = new KawanUser();
                FilterFields = new KawanUser();
                OrderBy = "Default";
                SortingOrder = "Ascending";
                FriendsOnly = false;
                DataService.OrderBy = OrderBy;
                DataService.SortingOrder = SortingOrder;
                DataService.FriendsOnly = FriendsOnly;
                MessagingCenter.Send(this, "clearSearch"); //Send to ViewAllProfilesPageViewModel to filter the list 
            });

            if (DataService.FilterFields != null) //The object has at least been initialised
                if(DataService.FilterFields.IsAnyFilterFieldsNotNull) //At least one of the fields of the initialised object is not null
                    FilterFields = DataService.FilterFields;
        }
    }
}