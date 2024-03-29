﻿using KawanApp.Models;
using KawanApp.ViewModels.Pages;
using Xamarin.Forms;

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
            MessagingCenter.Send(this, "currentPageApp"); //Send to App.xaml.cs
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, "clearCurrentPage");
            base.OnDisappearing();
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