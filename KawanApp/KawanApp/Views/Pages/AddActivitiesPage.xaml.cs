using KawanApp.Models;
using KawanApp.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    public partial class AddActivitiesPage : ContentPage
    {
        public AddActivitiesPage()
        {
            InitializeComponent();
            this.BindingContext = new AddActivitiesPageViewModel();
            startDatePicker.Date = DateTime.Now.AddHours(-1);
            endDatePicker.Date = DateTime.Now;
            startTimePicker.Time = DateTime.Now.AddHours(-1).TimeOfDay;
            endTimePicker.Time = DateTime.Now.TimeOfDay;
        }

        private void BackIcon_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PopAsync();
        }
    }
}