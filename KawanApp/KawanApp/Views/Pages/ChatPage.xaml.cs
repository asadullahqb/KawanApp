using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KawanApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    public partial class ChatPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<ChatPage>(this, "connectOnAppearing"); //Send to viewmodel
        } 
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<ChatPage>(this, "disconnectOnDisappearing"); //Send to viewmodel
        }

        public ChatPage(string receivingUserStudentId)
        {
            InitializeComponent();
            this.BindingContext = new ChatPageViewModel(receivingUserStudentId);
            //Subscribing from viewmodel:
            MessagingCenter.Subscribe<ChatPageViewModel>(this, "scrolltobottom", (sender) => {
                ChatList?.ScrollToFirst();
            });
        }
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send<ChatPage>(this, "navigateBack"); //Send to App.xaml.cs
            return true;
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
