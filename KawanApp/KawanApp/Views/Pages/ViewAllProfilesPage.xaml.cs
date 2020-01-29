﻿using KawanApp.ViewModels;
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
using KawanApp.Services;

namespace KawanApp.Views.Pages
{
    public partial class ViewAllProfilesPage : ContentPage
    {
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public ViewAllProfilesPage()
        {
            InitializeComponent();
            this.BindingContext = new ViewAllProfilesPageViewModel();
        }

        private void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;

            KawanUser KawanUser;
            KawanUser = (KawanUser)e.Item;

            MessagingCenter.Send(this, "navigateToViewAProfilePage", KawanUser); //Send to App.xaml.cs
        }

        //Icon here refers to the triple purpose icon for add friend/un-add friend/send message
        public void Icon_Tapped(object sender, EventArgs e) 
        {
            ImageButton img = sender as ImageButton;
            if (img.Source.ToString().Equals("File: sendMessage.png")) //Doing the click animation for add friend and un-add friend causes crashes. 
                                                                       //Hence, the animation of switching to other icon suffices. 
            {
                img.BackgroundColor = Color.White; //The colour is changed to white so that the click animation happens
                Animation(sender);
            }
            string imgsrc = img.Source.ToString();
            var converter = new ImageSourceConverter();
            string imgid = img.ClassId;
            int index=int.Parse(imgid);
            FriendRequest fr = new FriendRequest() { SendingStudentId = App.CurrentUser, ReceivingStudentId = DataService.AllUsers[index].StudentId };

            switch (imgsrc)
            {
                case "File: addFriend.png":
                    if (App.NetworkStatus)
                        ServerApi.SendFriendRequest(fr);
                    else
                    {
                        App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                        return;
                    }
                    DataService.AllUsers[index].FriendStatus = 1;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("friendRequestSent.png");
                    break;
                case "File: friendRequestSent.png":
                    if (App.NetworkStatus)
                        ServerApi.UnsendFriendRequest(fr);
                    else
                    {
                        App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                        return;
                    }
                    DataService.AllUsers[index].FriendStatus = 0;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                    break;
                case "File: sendMessage.png":
                    MessagingCenter.Send(this, "navigateToChatPage", DataService.AllUsers[index].StudentId); //Send to App.xaml.cs
                    break;
            }
        }

        private async void Animation(object sender) //Revert the background of the icon to transparent in 100 seconds
        {
            ImageButton img = sender as ImageButton;
            await Task.Delay(100);
            await Task.Run(() => { img.BackgroundColor = Color.Transparent; });
        }
    }
}