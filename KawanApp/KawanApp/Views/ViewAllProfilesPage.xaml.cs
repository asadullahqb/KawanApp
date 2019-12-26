using KawanApp.ViewModels;
using KawanApp.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAllProfilesPage : ContentPage
    {
        public ViewAllProfilesPage()
        {
            InitializeComponent();
            this.BindingContext = new ViewAllProfilesViewModel();
        }

        private object ViewAllProfilesViewModel()
        {
            throw new NotImplementedException();
        }

        private void KawanList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;

            KawanUser KawanUser;
            KawanUser = (KawanUser)e.Item;
            System.Diagnostics.Debug.WriteLine("View All Profiles Name: " + KawanUser.FullName);
            PopupNavigation.Instance.PushAsync(new ViewAProfilePage(KawanUser));

        }
    }
}