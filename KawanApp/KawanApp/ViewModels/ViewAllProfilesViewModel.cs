using FFImageLoading.Transformations;
using FFImageLoading.Work;
using FluentValidation.Results;
using KawanApp.Helpers;
using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Views.Pages;
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
    public class ViewAllProfilesViewModel : BaseViewModel
    {
        private ObservableCollection<KawanUser> _allUsers;
        private bool _isRefreshing = false;
        private string _currentUserType;
        private bool _isKawanTitleVisible;
        private bool _isInternationalStudentsTitleVisible;
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
        public string CurrentUserType
        {
            get { return _currentUserType; }
            set
            {
                _currentUserType = value;
                OnPropertyChanged();
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
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await Task.Run(() => FetchAllUsers());

                    IsRefreshing = false;
                });
            }
        }

        public ICommand ItemTappedCommand { get; set; }

        public ViewAllProfilesViewModel()
        {
            MessagingCenter.Subscribe<LoginPage>(this, "loadUserData", (sender) => { FetchAllUsers(); });
            FetchAllUsers();
        }

        private async void FetchAllUsers()
        {
            CurrentUserType = App.CurrentUserType;
            if (CurrentUserType == "International Student")
            {
                IsInternationalStudentsTitleVisible = false;
                IsKawanTitleVisible = true;
                List<KawanUser> AllKawanUsersFromDb = new List<KawanUser>();
                User u = new User() { StudentId = App.CurrentUser };
                AllKawanUsersFromDb = await ServerApi.FetchAllKawanUsers(u);

                ObservableCollection<KawanUser> temp = new ObservableCollection<KawanUser>(AllKawanUsersFromDb as List<KawanUser>);
                AllUsers = temp;
            }
            else if(CurrentUserType == "Kawan")
            {
                IsInternationalStudentsTitleVisible = true;
                IsKawanTitleVisible = false;
                List<User> AllInternationalStudentUsersFromDb = new List<User>();
                User u = new KawanUser() { StudentId = App.CurrentUser };
                AllInternationalStudentUsersFromDb = await ServerApi.FetchAllInternationalStudentUsers(u);

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
            }
        }
    }
}