using KawanApp.Models;
using KawanApp.ViewModels.Pages;
using System;
using Xamarin.Forms;

namespace KawanApp.Views.Pages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            this.BindingContext = new SignUpPageViewModel();
            dateOfBirthPicker.MinimumDate = DateTime.Now.AddYears(-50); //The oldest a user can be is 50 years old
            dateOfBirthPicker.MaximumDate = DateTime.Now.AddYears(-13); //The youngest a user can be is 13 years old
        }
        public SignUpPage(KawanUser ku)
        {
            InitializeComponent();
            this.BindingContext = new SignUpPageViewModel(ku);

            dateOfBirthPicker.MinimumDate = DateTime.Now.AddYears(-50); //The oldest a user can be is 50 years old
            dateOfBirthPicker.MaximumDate = DateTime.Now.AddYears(-13); //The youngest a user can be is 13 years old

            #region Set The School Picker
            switch (ku.SchoolShort)
            {
                case "":
                    schoolPicker.SelectedIndex = -1;
                    break;
                case "The Arts":
                    schoolPicker.SelectedIndex = 0;
                    break;
                case "Education":
                    schoolPicker.SelectedIndex = 1;
                    break;
                case "Humanities":
                    schoolPicker.SelectedIndex = 2;
                    break;
                case "Social Science":
                    schoolPicker.SelectedIndex = 3;
                    break;
                case "Communication":
                    schoolPicker.SelectedIndex = 4;
                    break;
                case "Computer Sciences":
                    schoolPicker.SelectedIndex = 5;
                    break;
                case "Industrial Technology":
                    schoolPicker.SelectedIndex = 6;
                    break;
                case "Pharmaceutical Sciences":
                    schoolPicker.SelectedIndex = 7;
                    break;
                case "Management":
                    schoolPicker.SelectedIndex = 8;
                    break;
                case "Housing, Building and Planning":
                    schoolPicker.SelectedIndex = 9;
                    break;
                case "Lang., Literacies and Translations":
                    schoolPicker.SelectedIndex = 10;
                    break;
                case "Physics":
                    schoolPicker.SelectedIndex = 11;
                    break;
                case "Chemistry":
                    schoolPicker.SelectedIndex = 12;
                    break;
                case "Biology":
                    schoolPicker.SelectedIndex = 13;
                    break;
                case "Mathematics":
                    schoolPicker.SelectedIndex = 14;
                    break;
                case "Civil Eng.":
                    schoolPicker.SelectedIndex = 15;
                    break;
                case "Chemical Eng.":
                    schoolPicker.SelectedIndex = 16;
                    break;
                case "Aerospace Eng.":
                    schoolPicker.SelectedIndex = 17;
                    break;
                case "Mechanical Eng.":
                    schoolPicker.SelectedIndex = 18;
                    break;
                case "Electrical and Electronic Eng.":
                    schoolPicker.SelectedIndex = 19;
                    break;
                case "Mat. and Min. Eng.":
                    schoolPicker.SelectedIndex = 20;
                    break;
                case "Health Sciences":
                    schoolPicker.SelectedIndex = 21;
                    break;
                case "Dental Sciences":
                    schoolPicker.SelectedIndex = 22;
                    break;
                case "Medical Sciences":
                    schoolPicker.SelectedIndex = 23;
                    break;
            }
            #endregion

            #region Set the Campus Picker
            switch (ku.Campus)
            {
                case "":
                    campusPicker.SelectedIndex = -1;
                    break;
                case "Main":
                    campusPicker.SelectedIndex = 0;
                    break;

                case "Engineering":
                    campusPicker.SelectedIndex = 1;
                    break;

                case "Health":
                    campusPicker.SelectedIndex = 2;
                    break;
            }

            switch (ku.Campus)
            {
                case "":
                    campusPicker2.SelectedIndex = -1;
                    break;

                case "Main Campus":
                    campusPicker2.SelectedIndex = 0;
                    break;

                case "Engineering Campus":
                    campusPicker2.SelectedIndex = 1;
                    break;

                case "Health Campus":
                    campusPicker2.SelectedIndex = 2;
                    break;
            }
            #endregion
        }
    }
}