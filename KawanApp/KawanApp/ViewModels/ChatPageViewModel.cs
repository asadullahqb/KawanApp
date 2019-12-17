using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using KawanApp.Models;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged
    {
        private bool _showScrollTap = false;
        private bool _lastMessageVisible = true;
        private int _pendingMessageCount = 0;
        private bool _pendingMessageCountVisible;
        private Queue<Message> _delayedMessages = new Queue<Message>();
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        private string _textToSend = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
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
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }

        public ChatPageViewModel()
        {
            Messages.Insert(0, new Message() { Text = "Hi. \na\nb\nc\nd\ne\nf\ng\nh\ni\nj\nk\nl\nm\nn\no\np\nq\nr\ns\nt\nu\nv\nw\nMy name is Asadullah Qamar Bin Qamar Siddique Bhatti. I want to test how long of a message can this UI handle. \nIf it's long enough, then I will use this UI. So let's see now. Hehehe \n\nRegards,\nAsadullah Qamar" });
            Messages.Insert(0, new Message() { Text = "How are you?", User = App.User });
            Messages.Insert(0, new Message() { Text = "What's new?" });
            Messages.Insert(0, new Message() { Text = "How is your family", User = App.User });
            Messages.Insert(0, new Message() { Text = "How is your dog?", User = App.User });
            Messages.Insert(0, new Message() { Text = "How is your cat?", User = App.User });
            Messages.Insert(0, new Message() { Text = "How is your sister?" });
            Messages.Insert(0, new Message() { Text = "When we are going to meet?" });
            Messages.Insert(0, new Message() { Text = "I want to buy a laptop" });
            Messages.Insert(0, new Message() { Text = "Where I can find a good one?" });
            Messages.Insert(0, new Message() { Text = "Also I'm testing this chat" });
            Messages.Insert(0, new Message() { Text = "Oh My God!" });
            Messages.Insert(0, new Message() { Text = " No Problem", User = App.User });
            Messages.Insert(0, new Message() { Text = "Hugs and Kisses", User = App.User });
            Messages.Insert(0, new Message() { Text = "When we are going to meet?" });
            Messages.Insert(0, new Message() { Text = "I want to buy a laptop" });
            Messages.Insert(0, new Message() { Text = "Where I can find a good one?" });
            Messages.Insert(0, new Message() { Text = "Also I'm testing this chat" });
            Messages.Insert(0, new Message() { Text = "Oh My God!" });
            Messages.Insert(0, new Message() { Text = " No Problem" });
            Messages.Insert(0, new Message() { Text = "Hugs and Kisses" });

            MessageAppearingCommand = new Command<Message>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<Message>(OnMessageDisappearing);

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    Messages.Insert(0, new Message() { Text = TextToSend, User = App.User });
                    TextToSend = string.Empty;
                    MessagingCenter.Send<ChatPageViewModel>(this, "scrolltobottom");
                }

            });

            //Code to simulate receiving a new message procces
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                if (LastMessageVisible)
                {
                    Messages.Insert(0, new Message() { Text = "New message test", User = "Mario" });
                }
                else
                {
                    DelayedMessages.Enqueue(new Message() { Text = "New message test", User = "Mario" });
                    PendingMessageCount++;
                }
                return true;
            });
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
