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
    public partial class UpdateSatisfactoryFormPage : ContentPage
    {
        public UpdateSatisfactoryFormPage(SatisfactoryForm sf)
        {
            InitializeComponent();
            
            if (sf.IsFilled)
            {
                switch (sf.Rating)
                {
                    case 0:
                    case 1:
                        starOne.IsStarred = true;
                        break;
                    case 2:
                        starTwo.IsStarred = true;
                        break;
                    case 3:
                        starThree.IsStarred = true;
                        break;
                    case 4:
                        starFour.IsStarred = true;
                        break;
                    case 5:
                        starFive.IsStarred = true;
                        break;
                }
            }
            else
            {
                starFive.IsStarred = true;
                sf.Rating = 5;
            }
            
            this.BindingContext = new UpdateSatisfactoryFormPageViewModel(sf);
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, "clearRatings"); //Send to StarBehaviour
            base.OnDisappearing();
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

            FeedbackClass fc = (FeedbackClass)e.Item;
            fc.IsChecked = !fc.IsChecked;

            MessagingCenter.Send(this, "updateFeedback", fc); //Send to view
        }

    }
}