using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Views.Pages;
using Refit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class AllMessagesPageViewModel : BaseViewModel
    {
        private ObservableCollection<ChatMessageItem> _allChatMessages;
        private bool _isRefreshing = false;


        public ObservableCollection<ChatMessageItem> AllChatMessages
        {
            get => _allChatMessages;
            set
            {
                _allChatMessages = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    if (App.NetworkStatus)
                    {
                        await Task.Run(() => FetchAllMessages());
                    }

                    IsRefreshing = false;
                });
            }
        }

        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);

        public AllMessagesPageViewModel()
        {
            FetchAllMessages();
            MessagingCenter.Subscribe<string>(this, "updateAllMessages", async (sender) => { await Task.Delay(1000); await FetchAllMessages(); });
            MessagingCenter.Subscribe<NotificationsPage, string>(this, "initiateNavigateToChatPage", async (sender, SendingUser) =>
            {
                while(AllChatMessages == null)
                    await Task.Delay(100); //Let the data load first
                await Task.Delay(400);
                ChatMessageItem cmi = AllChatMessages.Where(i => i.StudentId == SendingUser).FirstOrDefault();
                KawanUser ku = new KawanUser() { StudentId = cmi.StudentId, Pic = cmi.Pic, FirstName = cmi.FirstName };
                MessagingCenter.Send(this, "navigateToChatPage", ku); //Send to App.xaml.cs
            });
        }

        private async Task FetchAllMessages()
        {
            List<ChatMessageItem> AllChatMessagesFromDb;
            ChatMessageRequest cmr = new ChatMessageRequest() { SendingUser = App.CurrentUser , CurrentUserType = App.CurrentUserType};
            if (App.NetworkStatus)
                AllChatMessagesFromDb = await ServerApi.FetchAllMessages(cmr);
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            ObservableCollection<ChatMessageItem> temp = new ObservableCollection<ChatMessageItem>(AllChatMessagesFromDb);
            AllChatMessages = temp;
        }
    }
}