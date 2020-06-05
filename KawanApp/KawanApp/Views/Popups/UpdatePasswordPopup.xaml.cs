using KawanApp.ViewModels.Popups;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;

namespace KawanApp.Views.Popups
{
    public partial class UpdatePasswordPopup : PopupPage
    {

        public UpdatePasswordPopup()
        {
            InitializeComponent();
            this.BindingContext = new UpdatePasswordPopupViewModel();
        }

        private void Cancel_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}