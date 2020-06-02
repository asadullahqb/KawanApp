using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.ViewModels.Popups;
using KawanApp.Views.Popups;
using Microsoft.AspNetCore.SignalR.Client;
using OxyPlot;
using Refit;
using Rg.Plugins.Popup.Services;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class AddActivitiesPageViewModel : BaseViewModel
    {
        private int numOfStudentListClicks = 0;
        private bool _isSubmitting = false;
        private bool _isListLoading = true;
        private ObservableCollection<StudentForActivity> _listOfStudents;
        private string _listOfStudentsString;
        private Activity _baseActivity = new Activity();
        private HubConnection hubConnection;
        public bool IsSubmitting
        {
            get => _isSubmitting;
            set 
            {
                _isSubmitting = value;
                OnPropertyChanged();
            }
        }
        public bool IsListLoading
        {
            get => _isListLoading;
            set 
            {
                _isListLoading = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<StudentForActivity> ListOfStudents
        {
            get => _listOfStudents;
            set
            {
                _listOfStudents = value;
                OnPropertyChanged();
            }
        }
        public string ListOfStudentsString
        {
            get => _listOfStudentsString;
            set
            {
                _listOfStudentsString = value;
                OnPropertyChanged();
            }
        }
        public Activity BaseActivity
        {
            get => _baseActivity;
            set
            {
                _baseActivity = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddActivities { get; set; }
        public ICommand StudentsCommand { get; set; }
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);

        public AddActivitiesPageViewModel()
        {
            MessagingCenter.Subscribe<StudentsPopupViewModel, ObservableCollection<StudentForActivity>>(this, "refreshList", (sender, NewListOfStudents) =>
            {
                ListOfStudentsString = "";
                ListOfStudents = new ObservableCollection<StudentForActivity>();
                ListOfStudents = NewListOfStudents;
                int numChecked = 0;
                foreach (StudentForActivity Student in ListOfStudents)
                    if (Student.IsChecked) numChecked++;
                int i = 1;
                foreach (StudentForActivity Student in ListOfStudents.OrderByDescending(x => x.IsChecked).ToList())
                {
                    ListOfStudentsString += (Student.IsChecked) ? Student.StudentInfo.FullName : "";
                    if (numChecked == i)
                        break; //Don't append the comma
                    ListOfStudentsString += (Student.IsChecked) ? ", " : "";

                    i++;
                }
            });
            AddActivities = new Command(() => SubmitActivities());
            StudentsCommand = new Command(async() =>
            {
                if(numOfStudentListClicks > 0)
                {
                    PopupNavigation.Instance.PushAsync(new StudentsPopup(ListOfStudents));
                    PopupNavigation.Instance.PopAsync();
                    await PopupNavigation.Instance.PushAsync(new StudentsPopup(ListOfStudents));
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new StudentsPopup(ListOfStudents));
                    numOfStudentListClicks++;
                }
            });
            hubConnection = App.HubConnection;
            FetchListOfStudents();
        }

        private async Task FetchListOfStudents()
        {
            IsListLoading = true;
            if (App.NetworkStatus)
                ListOfStudents = new ObservableCollection<StudentForActivity>(await ServerApi.FetchListOfStudents(new Session()));
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            IsListLoading = false;
        }

        private async void SubmitActivities()
        {
            //Exception conditions
            if (string.IsNullOrEmpty(BaseActivity.Name))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please select an activity name.", "Ok");
                return;
            }
            else if (string.IsNullOrEmpty(ListOfStudentsString))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please select at least one student involved.", "Ok");
                return;
            }

            ReplyMessage rm;
            var NewActivities = new ObservableCollection<Activity>();

            //Build Activities List from the activities logged.
            
            ////Construct the complete start date and end date based on the date picker and time picker
            var BaseStartTimeOfDay = BaseActivity.StartDate.TimeOfDay;
            BaseActivity.StartDate.AddHours(-BaseStartTimeOfDay.TotalHours);
            BaseActivity.StartDate.AddMinutes(-BaseStartTimeOfDay.TotalMinutes);
            BaseActivity.StartDate.AddSeconds(-BaseStartTimeOfDay.TotalSeconds);
            var CompleteStartDate = BaseActivity.StartDate + BaseActivity.StartTime;
            
            var BaseEndTimeOfDay = BaseActivity.StartDate.TimeOfDay;
            BaseActivity.EndDate.AddHours(-BaseEndTimeOfDay.TotalHours);
            BaseActivity.EndDate.AddMinutes(-BaseEndTimeOfDay.TotalMinutes);
            BaseActivity.EndDate.AddSeconds(-BaseEndTimeOfDay.TotalSeconds);
            var CompleteEndDate = BaseActivity.EndDate + BaseActivity.EndTime;


            ////Create the activity for each student.
            foreach (StudentForActivity Student in ListOfStudents)
            {
                if(Student.IsChecked)
                {
                    NewActivities.Add(new Activity
                    {
                        //Not needed for db but needed for the app:
                        StudentFirstName = Student.StudentInfo.FirstName,
                        StudentLastName = Student.StudentInfo.LastName,
                        StudentPic = Student.StudentInfo.Pic,

                        //Needed for the db and app:
                        KawanStudentId = App.CurrentUser,
                        Name = BaseActivity.Name,
                        Description = BaseActivity.Description,
                        StudentHelped = Student.StudentInfo.StudentId,
                        StartDate = CompleteStartDate,
                        EndDate = CompleteEndDate
                    });
                }
            }


            //Submit to the server
            var NewActivitiesForServer = new ActivitiesForServer() { Activities = new List<Activity>(NewActivities) };
            if (App.NetworkStatus)
            {
                IsSubmitting = true;
                rm = await ServerApi.StoreActivities(NewActivitiesForServer);
                IsSubmitting = false;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }

            if (rm.Status)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Activities logged successfully!", "Ok");
                MessagingCenter.Send(this, "updateAllActivities", NewActivities);
                await App.Current.MainPage.Navigation.PopAsync();
                foreach (StudentForActivity Student in ListOfStudents)
                {
                    hubConnection.InvokeAsync("SendNotification", Student.StudentInfo.StudentId, "", "Activity");
                }
            }
            else
                await App.Current.MainPage.DisplayAlert("Error", rm.Message, "Ok");
        }
    }
}