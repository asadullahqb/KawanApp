using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KawanApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views
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

        public ChatPage()
        {
            InitializeComponent();
            this.BindingContext = new ChatPageViewModel();
            //Subscribing from viewmodel:
            MessagingCenter.Subscribe<ChatPageViewModel>(this, "scrolltobottom", (sender) => {
                ChatList?.ScrollToFirst();
            });
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
