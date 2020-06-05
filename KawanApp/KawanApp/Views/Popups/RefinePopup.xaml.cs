using KawanApp.Models;
using KawanApp.ViewModels.Popups;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace KawanApp.Views.Popups
{
    public partial class RefinePopup : PopupPage
    {
        public RefinePopup(ObservableCollection<KawanUser> allusers)
        {
            InitializeComponent();
            this.BindingContext = new RefinePopupViewModel(allusers);
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, "updateRefine"); //Send to View Model
            base.OnDisappearing();
        }

        private void Cancel_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            Entry entry = sender as Entry;
            entry.Text = null;
        }

        private void Clear_Tapped(object sender, EventArgs e)
        {
            slider.Value = 0;
        }
    }
}