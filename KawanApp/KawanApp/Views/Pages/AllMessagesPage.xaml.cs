using KawanApp.Models;
using KawanApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    public partial class AllMessagesPage : ContentPage
    {
        public AllMessagesPage()
        {
            InitializeComponent();
            this.BindingContext = new AllMessagesPageViewModel();
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, "currentPage"); //Send to AppShell View Model
            base.OnAppearing();
        }

        private void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;

            ChatMessageItem cmi;
            cmi = (ChatMessageItem)e.Item;

            KawanUser ku = new KawanUser() { StudentId = cmi.StudentId, Pic = cmi.Pic, FirstName = cmi.FirstName };

            MessagingCenter.Send(this, "navigateToChatPage", ku); //Send to App.xaml.cs
        }
    }
}