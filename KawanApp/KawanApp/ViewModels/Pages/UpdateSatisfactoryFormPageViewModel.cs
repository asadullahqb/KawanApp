using KawanApp.Interfaces;
using KawanApp.Models;
using KawanApp.Views.Pages;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class UpdateSatisfactoryFormPageViewModel : BaseViewModel
    {
        private SatisfactoryForm _satForm = new SatisfactoryForm();
        private ObservableCollection<FeedbackClass> _listOfCompliments = new ObservableCollection<FeedbackClass>()
        {
                new FeedbackClass(){ Index = 0, Feedback = "Good Communication"},
                new FeedbackClass(){ Index = 1, Feedback = "Great Mutual Engagement"},
                new FeedbackClass(){ Index = 2, Feedback = "Excellent Human Relation"},
                new FeedbackClass(){ Index = 3, Feedback = "Admirable Welfare Support"},
                new FeedbackClass(){ Index = 4, Feedback = "Amazing Social Needs"},
                new FeedbackClass(){ Index = 5, Feedback = "Friendly"},
                new FeedbackClass(){ Index = 6, Feedback = "Well Recommended"}
        };
        private ObservableCollection<FeedbackClass> _listOfCriticisms = new ObservableCollection<FeedbackClass>()
        {
               new FeedbackClass(){ Index = 0, Feedback = "Brush Up Communication"},
                new FeedbackClass(){ Index = 1, Feedback = "Improve Mutual Engagement"},
                new FeedbackClass(){ Index = 2, Feedback = "Refine Human Relation"},
                new FeedbackClass(){ Index = 3, Feedback = "Boost Welfare Support"},
                new FeedbackClass(){ Index = 4, Feedback = "Raise Social Needs"},
                new FeedbackClass(){ Index = 5, Feedback = "Increase friendliness"},
                new FeedbackClass(){ Index = 6, Feedback = "Better Recommendation"}
        };
        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);
        public SatisfactoryForm SatForm
        {
            get => _satForm;
            set
            {
                _satForm = value;
                OnPropertyChanged();
            }
        }

        public ICommand SubmitCommand { get; set; }


        public ObservableCollection<FeedbackClass> ListOfCompliments
        {
            get => _listOfCompliments;
            set
            {
                _listOfCompliments = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FeedbackClass> ListOfCriticisms
        {
            get => _listOfCriticisms;
            set
            {
                _listOfCriticisms = value;
                OnPropertyChanged();
            }
        }

        public UpdateSatisfactoryFormPageViewModel(SatisfactoryForm sf)
        {
            SubmitCommand = new Command(() => Submit());

            //Update the rating when the star behaviour is changed.
            MessagingCenter.Subscribe<string, int>(this, "updateRating", (sender, Rating) =>
            { 
                if (!SatForm.IsFilled) //Dont update any ratings for Filled forms
                    SatForm.Rating = Rating; 
            });

            //Functionality for when the list view row is clicked to update the check mark.
            MessagingCenter.Subscribe<UpdateSatisfactoryFormPage, FeedbackClass>(this, "updateFeedback", (sender, FC) =>
            {
                if (SatForm.Rating == 5)
                {
                    var loc = ListOfCompliments;
                    loc[FC.Index].IsChecked = FC.IsChecked;
                    ListOfCompliments = new ObservableCollection<FeedbackClass>();
                    ListOfCompliments = loc;
                }
                else
                {
                    var loc = ListOfCriticisms;
                    loc[FC.Index].IsChecked = FC.IsChecked;
                    ListOfCriticisms = new ObservableCollection<FeedbackClass>();
                    ListOfCriticisms = loc;
                }
            });

            //Initialise the SatForm.
            SatForm = sf;
            if(SatForm.IsFilled)
            {
                if(SatForm.Rating == 5)
                {
                    foreach (FeedbackClass fc in ListOfCompliments)
                        if (SatForm.ListLiked.Contains(fc.Feedback))
                            fc.IsChecked = true;
                }
                else
                {
                    foreach (FeedbackClass fc in ListOfCriticisms)
                        if (SatForm.ListImprovements.Contains(fc.Feedback))
                            fc.IsChecked = true;
                }
            }
        }

        private async void Submit()
        {
            //Build the feedback array
            var i = 1;
            SatForm.ListLiked = "";
            SatForm.ListImprovements = "";
            if (SatForm.Rating == 5)
            {
                int numChecked = 0;
                foreach (FeedbackClass fc in ListOfCompliments)
                    if (fc.IsChecked) numChecked++;
                var loc = new ObservableCollection<FeedbackClass>(ListOfCompliments.OrderByDescending(x => x.IsChecked).ToList());
                foreach (FeedbackClass fc in loc)
                {
                    SatForm.ListLiked += (fc.IsChecked) ? fc.Feedback : "";
                    if (numChecked == i)
                        break; //Don't append the comma
                    SatForm.ListLiked += (fc.IsChecked) ? ", " : "";
                    
                    i++;
                }
            }
            else
            {
                int numChecked = 0;
                foreach (FeedbackClass fc in ListOfCriticisms)
                    if (fc.IsChecked) numChecked++;
                var loc = new ObservableCollection<FeedbackClass>(ListOfCriticisms.OrderByDescending(x => x.IsChecked).ToList());
                foreach (FeedbackClass fc in loc)
                {
                    SatForm.ListImprovements += (fc.IsChecked) ? fc.Feedback : "";
                    if (numChecked == i)
                        break; //Don't append the comma
                    SatForm.ListImprovements += (fc.IsChecked) ? ", " : "";

                    i++;
                }

            }

            //Set date of form filled to today.
            SatForm.Date = DateTime.Now;

            //Store the SatForm in the database
            ReplyMessage rm;
            if (App.NetworkStatus)
                rm = await ServerApi.UpdateSatisfactoryForm(SatForm);
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            
            //Responses from server
            if (rm.Status)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Satisfactory form updated successfully!", "Ok");
                SatForm.IsFilled = true;
                MessagingCenter.Send(this, "updateSatisfactoryForm", SatForm); //Send to SatisfactoryFormPageViewModel to update the filled satisfactory form on the all satisfactory form page
            }
            else
                await App.Current.MainPage.DisplayAlert("Error", rm.Message, "Ok");
        }
    }
}