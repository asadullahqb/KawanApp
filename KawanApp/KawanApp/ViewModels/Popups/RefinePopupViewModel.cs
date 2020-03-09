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
        private KawanUser _filterFields;
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
            AllUsers = allusers;
            UserType = App.CurrentUserType;
            SearchCommand = new Command(() => 
            {
                DataService.FilterFields = FilterFields;
                MessagingCenter.Send(this, "updateList", DataService.GetFilteredResults(FilterFields)); //Send to ViewAllProfilesPageViewModel to filter the list 
            });

            ClearCommand = new Command(() =>
            {
                DataService.FilterFields = new KawanUser();
                FilterFields = new KawanUser();
                MessagingCenter.Send(this, "clearSearch"); //Send to ViewAllProfilesPageViewModel to filter the list 
            });

            if (!DataService.FilterFields.IsFilterFieldsNull)
                FilterFields = DataService.FilterFields;
        }
    }
}