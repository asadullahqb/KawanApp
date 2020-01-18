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
        public static string Country { get; set; }
        public static ObservableCollection<KawanUser> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            if (string.IsNullOrEmpty(normalizedQuery))
                return AllUsers;
            ObservableCollection<KawanUser> SearchResults = new ObservableCollection<KawanUser>(AllUsers.Where(f => f.Country.ToLowerInvariant().Contains(normalizedQuery)).ToList());
            return SearchResults;
        }
    }
}