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