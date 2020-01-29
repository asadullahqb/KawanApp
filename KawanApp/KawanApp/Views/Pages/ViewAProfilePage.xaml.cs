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
            this.BindingContext = new ViewAProfilePageViewModel(KawanData);
        }
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }

        private void BackIcon_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
        }

        private void ProfileImage_Tapped(object sender, EventArgs e)
        {

        }

        //Icon here refers to the triple purpose icon for add friend/un-add friend/send message
        public void Icon_Tapped(object sender, EventArgs e)
        {
            ImageButton img = sender as ImageButton;
            if (img.Source.ToString().Equals("File: sendMessage.png")) //Doing the click animation for add friend and un-add friend causes crashes. 
                                                                       //Hence, the animation of switching to other icon suffices. 
            {
                img.BackgroundColor = Color.FromHex("#f3f3f3"); //The colour is changed to white so that the click animation happens
                Animation(sender);
            }
            string imgsrc = img.Source.ToString();
            var converter = new ImageSourceConverter();
            FriendRequest fr = new FriendRequest() { SendingStudentId = App.CurrentUser, ReceivingStudentId = KawanDataForView.StudentId };

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
                    KawanDataForView.FriendStatus = 1;
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
                    KawanDataForView.FriendStatus = 0;
                    img.Source = (ImageSource)converter.ConvertFromInvariantString("addFriend.png");
                    break;
                case "File: sendMessage.png":
                    MessagingCenter.Send(this, "navigateToChatPage", KawanDataForView.StudentId); //Send to App.xaml.cs
                    break;
            }
        }
        private async void Animation(object sender) //Revert the background of the icon to transparent in 100 seconds
        {
            ImageButton img = sender as ImageButton;
            await Task.Delay(100);
            await Task.Run(() => { img.BackgroundColor = Color.Transparent; });
        }

        private void Analytics_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "navigateToAnalyticsPage"); //Send to App.xaml.cs
        }
    }
}