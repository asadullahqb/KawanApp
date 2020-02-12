using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Services;
using KawanApp.ViewModels.Popups;
using Refit;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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