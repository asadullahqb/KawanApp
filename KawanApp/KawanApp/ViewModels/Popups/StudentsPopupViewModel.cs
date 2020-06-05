using KawanApp.Models;
using KawanApp.Views.Popups;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Popups
{
    public class StudentsPopupViewModel : BaseViewModel
    {
        private ObservableCollection<StudentForActivity> _listOfStudents;
        public ObservableCollection<StudentForActivity> ListOfStudents
        {
            get => _listOfStudents;
            set
            {
                _listOfStudents = value;
                OnPropertyChanged();
            }
        }

        public StudentsPopupViewModel(ObservableCollection<StudentForActivity> listofstudents)
        {
            MessagingCenter.Subscribe<StudentsPopup, int>(this, "updateList", (sender, Index) =>
            {
                var los = ListOfStudents[Index];
                los.IsChecked ^= true;
                ListOfStudents[Index] = new StudentForActivity();
                ListOfStudents[Index] = los;
                MessagingCenter.Send(this, "refreshList", ListOfStudents); //Send to Add Activities View Model.
            });
            ListOfStudents = new ObservableCollection<StudentForActivity>();
            ListOfStudents = listofstudents;
        }

    }
}