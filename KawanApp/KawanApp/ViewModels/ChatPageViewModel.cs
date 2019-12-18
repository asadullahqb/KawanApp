using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using KawanApp.Models;
using KawanApp.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged
    {
        private string _receivingUser = "sam@sham.com";
        private bool _showScrollTap = false;
        private bool _lastMessageVisible = true;
        private int _pendingMessageCount = 0;
        private bool _pendingMessageCountVisible;
        private Queue<Message> _delayedMessages = new Queue<Message>();
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        private string _textToSend = string.Empty;
        private bool _isConnected = false;
        private HubConnection hubConnection;

        public event PropertyChangedEventHandler PropertyChanged;
        public string ReceivingUser
        {
            get 
            {
                return _receivingUser;
            }
            set
            {
                _receivingUser = value;
                OnPropertyChanged();
            }
        }
        public bool ShowScrollTap
        {
            get
            {
                return _showScrollTap;
            }
            set
            {
                _showScrollTap = value;
                OnPropertyChanged();
            }
        }
        public bool LastMessageVisible
        {
            get
            {
                return _lastMessageVisible;
            }
            set
            {
                _lastMessageVisible = value;
                OnPropertyChanged();
            }
        }
        public int PendingMessageCount 
        {
            get
            {
                return _pendingMessageCount;
            }
            set
            {
                _pendingMessageCount = value;
                OnPropertyChanged();
            }
        }
        public bool PendingMessageCountVisible 
        {
            get
            {
                return PendingMessageCount > 0;
            }
            set
            {
                _pendingMessageCountVisible = value;
                OnPropertyChanged();
            }
        }

        public Queue<Message> DelayedMessages
        {
            get
            {
                return _delayedMessages;
            }
            set
            {
                _delayedMessages = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Message> Messages 
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }
        public string TextToSend 
        {
            get
            {
                return _textToSend;
            }
            set
            {
                _textToSend = value;
                OnPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }

        public ChatPageViewModel()
        {
            Messages.Insert(0, new Message() { Text = "Hi. \na\nb\nc\nd\ne\nf\ng\nh\ni\nj\nk\nl\nm\nn\no\np\nq\nr\ns\nt\nu\nv\nw\nMy name is Asadullah Qamar Bin Qamar Siddique Bhatti. I want to test how long of a message can this UI handle. \nIf it's long enough, then I will use this UI. So let's see now. Hehehe \n\nRegards,\nAsadullah Qamar" });
            OnSendCommand = new Command(async () => { await SendPersonalMessage(ReceivingUser, TextToSend); });

            MessagingCenter.Subscribe<ChatPage>(this, "connectOnAppearing", (sender) => { Connect(App.CurrentUser); });
            MessagingCenter.Subscribe<ChatPage>(this, "disconnectOnDisappearing", (sender) => { Disconnect(App.CurrentUser); });

            MessageAppearingCommand = new Command<Message>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<Message>(OnMessageDisappearing);

            try
            {
                hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://kawantest.azurewebsites.net/chathub")
                .Build();
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Problem while building connection. Please try again later.", "OK");
            }

            #region Hub functions

            hubConnection.On<string, string>("ReceivePersonalMessage", (receivingUser, message) =>
            {
                if (receivingUser == App.CurrentUser)
                {
                    Messages.Insert(0, new Message() { User = receivingUser, Text = message});
                }
            });

            #endregion

            /*
            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    Messages.Insert(0, new Message() { Text = TextToSend, User = App.User });
                    TextToSend = string.Empty;
                    MessagingCenter.Send<ChatPageViewModel>(this, "scrolltobottom");
                }

            });
            */
        }

        async Task Connect(string user)
        {
            try
            {
                await hubConnection.StartAsync();
                await hubConnection.InvokeAsync("OnConnected", user);
                IsConnected = true;
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Error", "Problem while connecting to server. Please try again later.", "OK");
            }
        }

        async Task SendPersonalMessage(string receivingUser, string message)
        {
            if (!string.IsNullOrEmpty(TextToSend))
            {
                //Log message, clear the entry and scroll to bottom.
                Messages.Insert(0, new Message() { Text = TextToSend, User = App.CurrentUser });
                TextToSend = string.Empty;
                MessagingCenter.Send<ChatPageViewModel>(this, "scrolltobottom");

                //Send message to hub or store in local database for sending later.
                if(IsConnected)
                {
                    try
                    {
                        await hubConnection.InvokeAsync("SendPersonalMessage", receivingUser, message);
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                    }
                    //Store message in SQLite database.
                    //Store message in MySQL database.
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Note", "You are currently offline! The message will be sent once you are online.", "OK");
                    //Store message in SQLite database
                }
            }
        }
        async Task Disconnect(string user)
        {
            try
            {
                await hubConnection.InvokeAsync("OnDisconnected", user);
                await hubConnection.StopAsync();
                IsConnected = false;
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Error", "Problem while disconnecting from server. Please try again later.", "OK");
            }
        }

        void OnMessageAppearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        void OnMessageDisappearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
