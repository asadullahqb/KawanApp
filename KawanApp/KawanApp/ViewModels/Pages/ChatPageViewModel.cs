﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Views.Pages;
using Microsoft.AspNetCore.SignalR.Client;
using Refit;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class ChatPageViewModel : BaseViewModel
    {   
        private int connectTries = 0;
        private int disconnectTries = 0;
        private string _sendingUser = App.CurrentUser;
        private string _receivingUser;
        private string _firstName;
        private string _pic;
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
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }
        public string Pic
        {
            get
            {
                return _pic;
            }
            set
            {
                _pic = value;
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

        public ChatPageViewModel(KawanUser receivingUserDetails)
        {
            ReceivingUser = receivingUserDetails.StudentId;
            FirstName = receivingUserDetails.FirstName;
            Pic = receivingUserDetails.Pic;

            FetchMessages();

            OnSendCommand = new Command(async () => { await SendPersonalMessage(ReceivingUser, TextToSend); });

            MessagingCenter.Subscribe<string>(this, "connectToGroup", async(sender) => { await Task.Delay(1000); await Connect(App.CurrentUser); });
            MessagingCenter.Subscribe<ChatPage>(this, "connectOnAppearing", async(sender) => { await Connect(App.CurrentUser); });
            MessagingCenter.Subscribe<ChatPage>(this, "disconnectOnDisappearing", async(sender) => { await Disconnect(App.CurrentUser); });

            MessageAppearingCommand = new Command<ChatMessage>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<ChatMessage>(OnMessageDisappearing);

            try
            {
                hubConnection = App.HubConnection;
            }
            catch
            {
                //App.Current.MainPage.DisplayAlert("Error", "Problem while building connection. Please try again later.", "Ok");
            }

            #region Hub functions

            hubConnection.On<string, string, string>("ReceivePersonalMessage", (sendingUser, receivingUser, message) =>
            {
                if (sendingUser == ReceivingUser && receivingUser == SendingUser)
                {
                    Messages.Insert(0, new ChatMessage() { SendingUser = ReceivingUser, ReceivingUser = SendingUser, Text = message});
                }
            });

            hubConnection.On<string>("ReceiveSystemMessage", (string systemmessage) =>
            {
                Messages.Insert(0, new ChatMessage() { SendingUser = "SYSTEM", Text = "CHATHUB\n\n" + systemmessage });
                //App.Current.MainPage.DisplayAlert("Chathub Message", systemmessage, "Ok");
                
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

        async Task Connect(string sendingUser)
        {
            string receivingUser = ReceivingUser;
            if(connectTries < 1)
            {
                connectTries++;
                try
                {
                    await hubConnection.StartAsync();
                    await hubConnection.InvokeAsync("JoinGroup", sendingUser, receivingUser);
                    IsConnected = true;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    //await App.Current.MainPage.DisplayAlert("Error", "Problem while connecting to server. Please try again later.", "Ok");
                }
                connectTries--;
            }
            
        }

        async Task SendPersonalMessage(string receivingUser, string message)
        {
            if (!string.IsNullOrEmpty(TextToSend))
            {
                //Log message, clear the entry and scroll to bottom.
                ChatMessage cm = new ChatMessage() { Text = TextToSend, SendingUser = SendingUser, ReceivingUser = receivingUser, TimeStamp = DateTime.Now};
                if (message!="!users" && message!="!groups" && message != "!errors")
                    Messages.Insert(0, cm); //Log the message only if it's not "!users"
                TextToSend = string.Empty;
                MessagingCenter.Send(this, "scrolltobottom"); //Send to view.

                //Send message to hub or store in local database for sending later.
                if(IsConnected)
                {
                    try
                    {
                        await hubConnection.InvokeAsync("SendPersonalMessage", receivingUser, message);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }

                    if (message == "!users" || message == "!groups" || message == "!errors")
                        return; //Don't store the message in any databases

                    MessagingCenter.Send("App", "updateAllMessages");

                    //Store message in SQLite database.
                    //Not implemented yet.

                    //Store message in MySQL database.
                    ReplyMessage rm1;
                    ReplyMessage rm2;

                    if (App.NetworkStatus)
                    {
                        rm1 = await ServerApi.StoreMessage(cm);
                        rm2 = await ServerApi.StoreNotification(new Notification() { ReceivingUser = ReceivingUser, SendingUser = App.CurrentUser, Title = "Message", Message = cm.Text, Timestamp = DateTime.Now });
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                        return;
                    }

                    #if DEBUG
                    if (!rm1.Status)
                        await App.Current.MainPage.DisplayAlert("Failure!", rm1.Message, "Ok");
                    if (!rm2.Status)
                        await App.Current.MainPage.DisplayAlert("Failure!", rm2.Message, "Ok");
                    #endif
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
            if(disconnectTries < 1)
            {
                disconnectTries++;
                try
                {
                    await hubConnection.InvokeAsync("LeaveGroup", user);
                }
                catch
                {
                    //await App.Current.MainPage.DisplayAlert("Error", "Problem while disconnecting from server. Please try again later.", "Ok");
                }
                IsConnected = false;
                Messages.Insert(0, new ChatMessage() { SendingUser = "SYSTEM", Text = "CHATHUB\n\n" + "You have been disconnected from the chat." });
                disconnectTries--;
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
