using KawanApp.Models;
using Rg.Plugins.Popup.Pages;
using KawanApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using KawanApp.Interfaces;
using Refit;
using KawanApp.Services;
using Microsoft.AspNetCore.SignalR.Client;
using KawanApp.ViewModels.Pages;

namespace KawanApp.Views.Pages
{
    public partial class ViewAProfilePage : ContentPage
    {
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        private KawanUser KawanDataForView;
        public ViewAProfilePage()
        {
            InitializeComponent();
            this.BindingContext = new ViewAProfilePageViewModel();
        }
        public ViewAProfilePage(KawanUser KawanData)
        {
            InitializeComponent();
            KawanDataForView = KawanData;
            this.BindingContext = new ViewAProfilePageViewModel(KawanDataForView);
        }
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }

        private void BackIcon_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "updateData"); //Send to View All Profiles Page View Model
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
        }

        //Icon here refers to the triple purpose icon for add friend/un-add friend/send message
        public async void Icon_Tapped(object sender, EventArgs e)
        {
            ImageButton img = sender as ImageButton;
            string imgsrc = img.Source.ToString();
            var converter = new ImageSourceConverter();
            int index = KawanDataForView.Index;
            FriendRequest fr = new FriendRequest() { SendingStudentId = App.CurrentUser, ReceivingStudentId = KawanDataForView.StudentId };

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
                    KawanDataForView.FriendStatus = 1;
                    DataService.AllUsers[index].FriendStatus = 1;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("friendRequestSent.png");
                    break;
                case "File: friendRequestSent.png":
                    if (App.NetworkStatus)
                    {
                        await ServerApi.UnsendFriendRequest(fr);
                        await ServerApi.DeleteNotification(new Notification() { ReceivingUser = fr.ReceivingStudentId, SendingUser = App.CurrentUser, Title = "Friend", Message = "sent you their friend request." });
                        await App.HubConnection.InvokeAsync("SendNotification", fr.ReceivingStudentId, App.CurrentFirstName + " unsent you their friend request.", "Friend");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Please turn on internet.", "Ok");
                        return;
                    }
                    KawanDataForView.FriendStatus = 0;
                    DataService.AllUsers[index].FriendStatus = 0;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                    break;
                case "File: friendRequestReceived.png":
                    var accepted = await DisplayAlert("Friend request received", DataService.AllUsers[index].FirstName + " sent you a friend request!", "Accept", "Reject");
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
                        KawanDataForView.FriendStatus = 3;
                        DataService.AllUsers[index].FriendStatus = 3;
                        img.Source = (ImageSource)converter.ConvertFromInvariantString("sendMessage.png");
                        await DisplayAlert("Success", "You are now friends with " + DataService.AllUsers[index].FirstName + "!", "Ok");
                        break;
                    }
                    else
                    {
                        var accepted2 = await DisplayAlert("Rejecting friend request", "Are you sure you want to reject " + DataService.AllUsers[index].FirstName + "'s friend request?", "Yes", "No");
                        if (accepted2)
                        {
                            if (App.NetworkStatus)
                                await ServerApi.RejectFriendRequest(fr);
                            else
                            {
                                await DisplayAlert("Error", "Please turn on internet.", "Ok");
                                return;
                            }
                            KawanDataForView.FriendStatus = 0;
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
                    MessagingCenter.Send(this, "navigateToChatPage", KawanDataForView); //Send to App.xaml.cs
                    break;
            }
        }

        private async void Analytics_Tapped(object sender, EventArgs e)
        {
            if (App.NetworkStatus)
                MessagingCenter.Send(this, "navigateToAnalyticsPage", App.CurrentUser); //Send to App.xaml.cs
            else
            {
                await DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
        }

    }
}