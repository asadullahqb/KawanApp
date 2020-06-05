using KawanApp.Models;
using KawanApp.ViewModels.Popups;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace KawanApp.Views.Popups
{
    public partial class StudentsPopup : PopupPage
    {
        public StudentsPopup(ObservableCollection<StudentForActivity> listofstudents)
        {
            InitializeComponent();
            this.BindingContext = new StudentsPopupViewModel(listofstudents);
        }

        private void Ok_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;

            StudentForActivity sfa;
            sfa = (StudentForActivity)e.Item;

            MessagingCenter.Send(this, "updateList", sfa.Index); //Send to StudentsPopupViewModel to update IsChecked.
        }

    }
}