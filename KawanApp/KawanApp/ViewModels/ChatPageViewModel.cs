using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Views.Pages;
using Microsoft.AspNetCore.SignalR.Client;
using Refit;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class ChatPageViewModel : BaseViewModel
    {   
        private string _sendingUser = App.CurrentUser;
        private string _receivingUser;
        private bool _showScrollTap = false;
        private bool _lastMessageVisible = true;
        private int _pendingMessageCount = 0;
        private bool _pendingMessageCountVisible;
        private Queue<ChatMessage> _delayedMessages = new Queue<ChatMessage>();
        private ObservableCollection<ChatMessage> _messages = new ObservableCollection<ChatMessage>();
        private string _textToSend = string.Empty;
        private bool _isConnected = true;
        private HubConnection hubConnection;

        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public string SendingUser
        {
            get 
            {
                return _sendingUser;
            }
            set
            {
                _sendingUser = value;
                OnPropertyChanged();
            }
        }
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

        public Queue<ChatMessage> DelayedMessages
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
        public ObservableCollection<ChatMessage> Messages 
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

        public ChatPageViewModel(string receivingUserStudentId)
        {
            ReceivingUser = receivingUserStudentId;

            FetchMessages();

            OnSendCommand = new Command(async () => { await SendPersonalMessage(ReceivingUser, TextToSend); });

            MessagingCenter.Subscribe<ChatPage>(this, "connectOnAppearing", (sender) => { Connect(App.CurrentUser); });
            MessagingCenter.Subscribe<ChatPage>(this, "disconnectOnDisappearing", (sender) => { Disconnect(App.CurrentUser); });

            MessageAppearingCommand = new Command<ChatMessage>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<ChatMessage>(OnMessageDisappearing);

            try
            {
                hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://kawantest.azurewebsites.net/chathub")
                .Build();
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Error", "Problem while building connection. Please try again later.", "Ok");
            }

            #region Hub functions

            hubConnection.On<string, string, string>("ReceivePersonalMessage", (sendingUser, receivingUser, message) =>
            {
                if (receivingUser == App.CurrentUser)
                {
                    Messages.Insert(0, new ChatMessage() { SendingUser = sendingUser, Text = message});
                }
            });

            hubConnection.On<string>("ReceiveSystemMessage", (string systemmessage) =>
            {
                Messages.Insert(0, new ChatMessage() { SendingUser = "SYSTEM", Text = "CHATHUB\n\n" + systemmessage });
                //App.Current.MainPage.DisplayAlert("Chathub Message", systemmessage, "Ok");
                if (systemmessage.Contains("#101"))
                {
                    Disconnect(App.CurrentUser);
                }
            });

            #endregion
        }

        private async void FetchMessages()
        {
            //Fetch messages from database and parse into Observable Collection
            List<ChatMessage> MessagesFromDb = new List<ChatMessage>();
            ChatMessage cm = new ChatMessage() { SendingUser= SendingUser, ReceivingUser = ReceivingUser };

            if (App.NetworkStatus)
                MessagesFromDb = await ServerApi.FetchMessages(cm);
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            
            ObservableCollection<ChatMessage> temp = new ObservableCollection<ChatMessage>(MessagesFromDb as List<ChatMessage>);
            Messages = temp;

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
                await App.Current.MainPage.DisplayAlert("Error", "Problem while connecting to server. Please try again later.", "Ok");
            }
        }

        async Task SendPersonalMessage(string receivingUser, string message)
        {
            if (!string.IsNullOrEmpty(TextToSend))
            {
                //Log message, clear the entry and scroll to bottom.
                ChatMessage cm = new ChatMessage() { Text = TextToSend, SendingUser = SendingUser, ReceivingUser = receivingUser, TimeStamp = DateTime.Now};
                if (!(message=="!users"))
                    Messages.Insert(0, cm); //Log the message only if it's not "!users"
                TextToSend = string.Empty;
                MessagingCenter.Send(this, "scrolltobottom"); //Send to view.

                //Send message to hub or store in local database for sending later.
                if(IsConnected)
                {
                    try
                    {
                        await hubConnection.InvokeAsync("SendPersonalMessage", receivingUser, message);
                        if (message=="!users")
                            return; //Don't store the message in any databases
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                    }
                    //Store message in SQLite database.
                    //Not implemented yet.

                    //Store message in MySQL database.
                    ReplyMessage rm;

                    if (App.NetworkStatus)
                        rm = await ServerApi.StoreMessage(cm);
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                        return;
                    }
                    //Just for debugging:
                    if (!rm.Status)
                        await App.Current.MainPage.DisplayAlert("Failure!", rm.Message, "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Note", "You are currently offline! The message will be sent once you are online.", "Ok");
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
                Messages.Insert(0, new ChatMessage() { SendingUser = "SYSTEM", Text = "CHATHUB\n\n" + "You have been disconnected from the chat." });
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Error", "Problem while disconnecting from server. Please try again later.", "Ok");
            }
        }

        void OnMessageAppearing(ChatMessage message)
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

        void OnMessageDisappearing(ChatMessage message)
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
    }
}
