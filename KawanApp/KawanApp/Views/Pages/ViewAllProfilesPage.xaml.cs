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
using KawanApp.Interfaces;
using Refit;

namespace KawanApp.Views.Pages
{
    public partial class ViewAllProfilesPage : ContentPage
    {
        private ViewAllProfilesViewModel vm = new ViewAllProfilesViewModel();
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public ViewAllProfilesPage()
        {
            InitializeComponent();
            this.BindingContext = vm;
        }

        private void List_ItemTapped(object sender, ItemTappedEventArgs e)
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
        public void Icon_Tapped(object sender, EventArgs e) 
        {
            Image img = sender as Image;
            string imgsrc = img.Source.ToString();
            var converter = new ImageSourceConverter();
            string imgid = img.ClassId;
            int index=int.Parse(imgid);
            FriendRequest fr = new FriendRequest() { SendingStudentId = App.CurrentUser, ReceivingStudentId = vm.AllUsers[index].StudentId };

            switch (imgsrc)
            {
                case "File: addFriend.png":
                    ServerApi.SendFriendRequest(fr);
                    vm.AllUsers[index].FriendStatus = 1;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("friendRequestSent.png");
                    break;
                case "File: friendRequestSent.png":
                    ServerApi.UnsendFriendRequest(fr);
                    vm.AllUsers[index].FriendStatus = 0;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                    break;
                case "File: sendMessage.png":
                    MessagingCenter.Send<ViewAllProfilesPage, string>(this, "navigateToChatPage", vm.AllUsers[index].StudentId); //Send to App.xaml.cs
                    break;
            }
        }
    }
}