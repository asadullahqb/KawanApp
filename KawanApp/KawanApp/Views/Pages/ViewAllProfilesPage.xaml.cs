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

namespace KawanApp.Views.Pages
{
    public partial class ViewAllProfilesPage : ContentPage
    {
        public ViewAllProfilesPage()
        {
            InitializeComponent();
            this.BindingContext = new ViewAllProfilesViewModel();
        }

        private void KawanList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;

            KawanUser KawanUser;
            KawanUser = (KawanUser)e.Item;

            MessagingCenter.Send<ViewAllProfilesPage, KawanUser>(this, "navigateToViewAProfilePage", KawanUser); //Send to App.xaml.cs
            //PopupNavigation.Instance.PushAsync(new ViewAProfilePage(KawanUser));
        }

        private void Icon_Tapped(object sender, EventArgs e) //Icon here refers to the dual purpose icon for add friend/send message
        {
            //Add friend or open chat box (depending on if friend or not)
        }
    }
}