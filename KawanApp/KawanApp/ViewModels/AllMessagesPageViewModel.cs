using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels
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
        }

        private async void FetchAllMessages()
        {
            List<ChatMessageItem> AllChatMessagesFromDb;
            ChatMessageRequest cmr = new ChatMessageRequest() { SendingUser = App.CurrentUser , CurrentUserType = App.CurrentUserType};
            AllChatMessagesFromDb = await ServerApi.FetchAllMessages(cmr);
            ObservableCollection<ChatMessageItem> temp = new ObservableCollection<ChatMessageItem>(AllChatMessagesFromDb);
            AllChatMessages = temp;
        }
    }
}