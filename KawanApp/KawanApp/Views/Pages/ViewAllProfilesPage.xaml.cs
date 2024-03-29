﻿using KawanApp.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using KawanApp.Interfaces;
using Refit;
using KawanApp.Services;
using Microsoft.AspNetCore.SignalR.Client;
using KawanApp.ViewModels.Pages;

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

        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, "currentPage"); //Send to AppShell View Model
            base.OnAppearing();
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
        public async void Icon_Tapped(object sender, EventArgs e) 
        {
            ImageButton img = sender as ImageButton;
            if (img.Source.ToString().Equals("File: sendMessage.png")) //Doing the click animation for add friend and un-add friend causes crashes. 
                                                                       //Hence, the animation of switching to other icon suffices. 
            {
                img.BackgroundColor = Color.White; //The colour is changed to white so that the click animation is visible
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
                    {
                        await ServerApi.SendFriendRequest(fr);
                        await ServerApi.StoreNotification(new Notification() { ReceivingUser = fr.ReceivingStudentId, SendingUser = App.CurrentUser, Title = "Friend", Message = "sent you a friend request.", Timestamp = DateTime.Now });
                        await App.HubConnection.InvokeAsync("SendNotification", fr.ReceivingStudentId, App.CurrentFirstName + " sent you a friend request.", "Friend");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Please turn on internet.", "Ok");
                        return;
                    }
                    DataService.AllUsers[index].FriendStatus = 1;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("friendRequestSent.png");
                    break;
                case "File: friendRequestSent.png":
                    if (App.NetworkStatus)
                    {
                        await ServerApi.UnsendFriendRequest(fr);
                        await ServerApi.DeleteNotification(new Notification() { ReceivingUser = fr.ReceivingStudentId, SendingUser = App.CurrentUser, Title = "Friend", Message = "sent you a friend request." });
                        await App.HubConnection.InvokeAsync("SendNotification", fr.ReceivingStudentId, App.CurrentFirstName + " unsent you their friend request.", "Friend");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Please turn on internet.", "Ok");
                        return;
                    }
                    DataService.AllUsers[index].FriendStatus = 0;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                    break;
                case "File: friendRequestReceived.png":
                    var accepted = await DisplayAlert("Friend request received", DataService.AllUsers[index].FirstName + " sent you a friend request!","Accept","Reject");
                    if (accepted)
                    {
                        if (App.NetworkStatus)
                        {
                            await ServerApi.AcceptFriendRequest(fr);
                            await ServerApi.StoreNotification(new Notification() { ReceivingUser = fr.ReceivingStudentId, SendingUser = App.CurrentUser, Title = "Friend", Message = "accepted your friend request.", Timestamp = DateTime.Now });
                            await App.HubConnection.InvokeAsync("SendNotification", fr.ReceivingStudentId, App.CurrentFirstName + " accepted your friend request.", "Friend");
                        }
                        else
                        {
                            await DisplayAlert("Error", "Please turn on internet.", "Ok");
                            return;
                        }
                        DataService.AllUsers[index].FriendStatus = 3;
                        img.Source = (ImageSource)converter.ConvertFromInvariantString("sendMessage.png");
                        await DisplayAlert("Success","You are now friends with " + DataService.AllUsers[index].FirstName + "!", "Ok");
                        break;
                    }
                    else
                    {
                        var accepted2 = await DisplayAlert("Rejecting friend request", "Are you sure you want to reject " + DataService.AllUsers[index].FirstName + "'s friend request?", "Yes", "No");
                        if(accepted2)
                        {
                            if (App.NetworkStatus)
                            {
                                await ServerApi.RejectFriendRequest(fr);
                                await App.HubConnection.InvokeAsync("SendNotification", fr.ReceivingStudentId, App.CurrentFirstName + " rejected your friend request.", "Friend");
                            }
                            else
                            {
                                await DisplayAlert("Error", "Please turn on internet.", "Ok");
                                return;
                            }
                            DataService.AllUsers[index].FriendStatus = 0;
                            img.Source = (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                            break;
                        }
                        else
                        {
                            break; //Do nothing
                        }
                    }
                case "File: sendMessage.png":
                    MessagingCenter.Send(this, "navigateToChatPage", DataService.AllUsers[index]); //Send to App.xaml.cs
                    break;
            }
        }

        private async Task Animation(object sender)
        {
            ImageButton img = sender as ImageButton;
            await Task.Delay(100);
            await Task.Run(() => { img.BackgroundColor = Color.Transparent; });
        }
    }
}