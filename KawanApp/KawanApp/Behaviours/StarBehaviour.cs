using KawanApp.ViewModels.Pages;
using KawanApp.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KawanApp.Behaviours
{
    public class StarBehaviour : Behavior<View>
    {
        TapGestureRecognizer tapRecognizer;
        static List<StarBehaviour> defaultbehaviours = new List<StarBehaviour>();
        static Dictionary<string, List<StarBehaviour>> starGroups = new Dictionary<string, List<StarBehaviour>>();

        public static readonly BindableProperty GroupNameProperty =
            BindableProperty.Create("GroupName",
                                    typeof(string),
                                    typeof(StarBehaviour),
                                    null,
                                    propertyChanged: OnGroupNameChanged);

        public string GroupName
        {
            set { SetValue(GroupNameProperty, value); }
            get { return (string)GetValue(GroupNameProperty); }
        }

        public static readonly BindableProperty RatingProperty =
           BindableProperty.Create("Rating",
                                   typeof(int),
                                   typeof(StarBehaviour), default(int));

        public int Rating
        {
            set { SetValue(RatingProperty, value); }
            get { return (int)GetValue(RatingProperty); }
        }

        static void OnGroupNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StarBehaviour behaviour = (StarBehaviour)bindable;
            string oldGroupName = (string)oldValue;
            string newGroupName = (string)newValue;

            // Remove existing behaviour from Group
            if (String.IsNullOrEmpty(oldGroupName))
            {
                defaultbehaviours.Remove(behaviour);
            }
            else
            {
                List<StarBehaviour> behaviours = starGroups[oldGroupName];
                behaviours.Remove(behaviour);

                if (behaviours.Count == 0)
                {
                    starGroups.Remove(oldGroupName);
                }
            }

            // Add New behaviour to the group
            if (String.IsNullOrEmpty(newGroupName))
            {
                defaultbehaviours.Add(behaviour);
            }
            else
            {
                List<StarBehaviour> behaviours = null;

                if (starGroups.ContainsKey(newGroupName))
                {
                    behaviours = starGroups[newGroupName];
                }
                else
                {
                    behaviours = new List<StarBehaviour>();
                    starGroups.Add(newGroupName, behaviours);
                }

                behaviours.Add(behaviour);
            }

        }


        public static readonly BindableProperty IsStarredProperty =
            BindableProperty.Create("IsStarred",
                                    typeof(bool),
                                    typeof(StarBehaviour),
                                    false,
                                    propertyChanged: OnIsStarredChanged);

        public bool IsStarred
        {
            set { SetValue(IsStarredProperty, value); }
            get { return (bool)GetValue(IsStarredProperty); }
        }

        static void OnIsStarredChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StarBehaviour behaviour = (StarBehaviour)bindable;

            if ((bool)newValue)
            {
                string groupName = behaviour.GroupName;
                List<StarBehaviour> behaviours;
                
                if (string.IsNullOrEmpty(groupName))
                {
                    behaviours = defaultbehaviours;
                }
                else
                {
                    behaviours = starGroups[groupName];
                }

                bool itemReached = false;
                int count = 1, position = 0;
                // all positions to left IsStarred = true and all position to the right is false
                foreach (var item in behaviours)
                {
                    if (item != behaviour && !itemReached)
                    {
                        item.IsStarred = true;
                    }
                    if (item == behaviour)
                    {
                        itemReached = true;
                        item.IsStarred = true;
                        position = count;
                    }
                    if (item != behaviour && itemReached)
                        item.IsStarred = false;

                    item.Rating = position;
                    count++;
                    if (count == 0)
                        count = 0;
                    else if (count % 5 == 0)
                        count = 5;
                    else
                        count %= 5;
                }

                MessagingCenter.Send("StarBehaviour", "updateRating", behaviours[4].Rating);
            }
        }

        public StarBehaviour()
        {
            //Clear the ratings every time the page disappears:
            MessagingCenter.Subscribe<UpdateSatisfactoryFormPage>(this, "clearRatings", (sender) => { defaultbehaviours = new List<StarBehaviour>(); starGroups = new Dictionary<string, List<StarBehaviour>>(); });
            
            defaultbehaviours.Add(this);
        }

        protected override void OnAttachedTo(View view)
        {
            tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += OnTapRecognizerTapped;
            view.GestureRecognizers.Add(tapRecognizer);
        }

        protected override void OnDetachingFrom(View view)
        {
            view.GestureRecognizers.Remove(tapRecognizer);
            tapRecognizer.Tapped -= OnTapRecognizerTapped;
        }

        void OnTapRecognizerTapped(object sender, EventArgs args)
        {
            //HACK: PropertyChanged does not fire, if the value is not changed :-(
            IsStarred = false;
            IsStarred = true;
        }
    }
}
