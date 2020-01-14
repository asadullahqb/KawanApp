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
using System.Reflection;

namespace KawanApp.Views.Pages
{
    public partial class ViewAllProfilesPage : ContentPage
    {
        private ViewAllProfilesViewModel vm = new ViewAllProfilesViewModel();
        public ViewAllProfilesPage()
        {
            InitializeComponent();
            this.BindingContext = vm;
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

        //Icon here refers to the triple purpose icon for add friend/un-add friend/send message
        private void Icon_Tapped(object sender, EventArgs e) 
        {
            Image img = sender as Image;
            string imgsrc = img.Source.ToString();
            var converter = new ImageSourceConverter();
            string imgid = img.ClassId;
            int index=int.Parse(imgid);
            int[] indexandfs = { index, 0 }; //To store the index of the Kawan and the Friend Status
            string receivingUserEmail = vm.AllKawanUsers[index].Email;
            switch (imgsrc)
            {
                case "File: addFriend.png":
                    //Send friend request through RESTful
                    indexandfs[1] = 1;
                    MessagingCenter.Send<ViewAllProfilesPage, int[]>(this, "updateFriendStatus", indexandfs); //Send to viewmodel
                    //Image imgtest = this.FindByName<Image>("IconImageElement");
                    //this.FindByName<Image>("IconImage").Source = (ImageSource)converter.ConvertFromInvariantString("friendRequestSent.png");
                    break;
                case "File: friendRequestSent.png":
                    //Send unsend friend request through RESTful
                    indexandfs[1] = 0;
                    MessagingCenter.Send<ViewAllProfilesPage, int[]>(this, "updateFriendStatus", indexandfs); //Send to viewmodel
                    //this.FindByName<Image>("IconImage").Source = (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                    break;
                case "File: sendMessage.png":
                    MessagingCenter.Send<ViewAllProfilesPage, string>(this, "navigateToChatPage", receivingUserEmail); //Send to App.xaml.cs
                    break;
            }
        }
    }
}