using System;
using KawanApp.Models;
using KawanApp.ViewModels.Pages;
using Xamarin.Forms;

namespace KawanApp.Views.Pages
{
    public partial class ChatPage : ContentPage
    {
        protected override void OnAppearing()
        {
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#234779");
            MessagingCenter.Send(this, "connectOnAppearing"); //Send to viewmodel
            base.OnAppearing();
        } 
        protected override void OnDisappearing()
        {
            if(App.Current.MainPage.Navigation.NavigationStack.Count > 1) //Only change the colour back if theres previously a navigation page in the stack
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            MessagingCenter.Send(this, "clearCurrentPage"); //Send to App.xaml.cs
            //MessagingCenter.Send(this, "disconnectOnDisappearing"); //Send to viewmodel
            base.OnDisappearing();
        }

        public ChatPage(KawanUser ku)
        {
            InitializeComponent();
            this.BindingContext = new ChatPageViewModel(ku);
            //Subscribing from viewmodel:
            MessagingCenter.Subscribe<ChatPageViewModel>(this, "scrolltobottom", (sender) => {
                ChatList?.ScrollToFirst();
            });
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

        public void ScrollTap(object sender, System.EventArgs e)
        {
            lock (new object())
            {
                if (BindingContext != null)
                {
                    var vm = BindingContext as ChatPageViewModel;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        while (vm.DelayedMessages.Count > 0)
                        {
                            vm.Messages.Insert(0, vm.DelayedMessages.Dequeue());
                        }
                        vm.ShowScrollTap = false;
                        vm.LastMessageVisible = true;
                        vm.PendingMessageCount = 0;
                        ChatList?.ScrollToFirst();
                    });
                }
            }
        }

        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }
    }
}
