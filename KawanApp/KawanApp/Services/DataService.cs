using KawanApp.Models;
using KawanApp.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KawanApp.Services
{
    public static class DataService
    {
        public static ObservableCollection<KawanUser> AllUsers { get; set; }
        public static KawanUser KawanUser { get; set; }
        public static KawanUser FilterFields { get; set; }
        public static string Country { get; set; }
        public static ObservableCollection<KawanUser> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            if (string.IsNullOrEmpty(normalizedQuery))
                return AllUsers;
            ObservableCollection<KawanUser> SearchResults = new ObservableCollection<KawanUser>(AllUsers.Where(f => f.Country.ToLowerInvariant().Contains(normalizedQuery)).ToList());
            return SearchResults;
        }

        public static ObservableCollection<KawanUser> GetFilteredResults(KawanUser filterFields)
        {
            var normalizedFields = filterFields.NormaliseFilterFields();
            ObservableCollection<KawanUser> SearchResults;
            if (!normalizedFields.IsAnyFilterFieldsNotNull) //If all fields are null
            {
                if (Country != null)
                    SearchResults = GetSearchResults(Country);
                else
                    SearchResults = AllUsers;
            }
            else //if any of the fields has something
            {
                if (Country != null)
                    SearchResults = GetSearchResults(Country); //Filter by country first then filter from there
                else
                    SearchResults = AllUsers;
                
                //Filter by all the filter fields
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.FullName.ToLowerInvariant().Contains(normalizedFields.FirstName)).ToList());
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.Email.ToLowerInvariant().Contains(normalizedFields.Email)).ToList());
                if (!string.IsNullOrEmpty(FilterFields.Email))
                    for (int i = 0; i < SearchResults.Count; i++)
                        if (SearchResults[i].FriendStatus != 3)
                        {
                            SearchResults.RemoveAt(i); //Filter out phone numbers filtered if they are not friends
                            i--;
                        }
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.Gender.ToLowerInvariant().Contains(normalizedFields.Gender)).ToList());
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.PhoneNum.ToLowerInvariant().Contains(normalizedFields.PhoneNum)).ToList());
                if(!string.IsNullOrEmpty(FilterFields.PhoneNum))
                    for(int i=0; i< SearchResults.Count; i++)
                        if (SearchResults[i].FriendStatus != 3)
                        {
                            SearchResults.RemoveAt(i); //Filter out phone numbers filtered if they are not friends
                            i--;
                        } 
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.Campus.ToLowerInvariant().Contains(normalizedFields.Campus)).ToList());
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.School.ToLowerInvariant().Contains(normalizedFields.School)).ToList());
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.Country.ToLowerInvariant().Contains(normalizedFields.Country)).ToList());
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.AboutMe.ToLowerInvariant().Contains(normalizedFields.AboutMe)).ToList());
                SearchResults = new ObservableCollection<KawanUser>(SearchResults.Where(f => f.AverageResponseTime.ToLowerInvariant().Contains(normalizedFields.AverageResponseTime)).ToList());
            }
            return SearchResults;
        }
    }
}