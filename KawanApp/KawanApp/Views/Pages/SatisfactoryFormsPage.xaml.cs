using KawanApp.Models;
using KawanApp.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views.Pages
{
    public partial class SatisfactoryFormsPage : ContentPage
    {
        public SatisfactoryFormsPage()
        {
            InitializeComponent();
            this.BindingContext = new SatisfactoryFormsPageViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }

        private void BackIcon_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
        }

        private void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;

            SatisfactoryForm satForm;
            satForm = (SatisfactoryForm)e.Item;

            MessagingCenter.Send(this, "navigateToUpdateSatisfactoryFormPage", satForm); //Send to App.xaml.cs
        }
    }
}