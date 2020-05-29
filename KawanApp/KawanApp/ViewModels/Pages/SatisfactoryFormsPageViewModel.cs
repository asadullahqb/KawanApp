using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KawanApp.ViewModels.Pages
{
    public class SatisfactoryFormsPageViewModel : BaseViewModel
    {
        private ObservableCollection<SatisfactoryForm> _allSatisfactoryForms;
        private bool _isRefreshing;
        public ObservableCollection<SatisfactoryForm> AllSatisfactoryForms
        {
            get => _allSatisfactoryForms;
            set
            {
                _allSatisfactoryForms = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    if (App.NetworkStatus)
                    {
                        await FetchAllSatisfactoryForms();
                    }

                    IsRefreshing = false;
                });
            }
        }

        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);

        public SatisfactoryFormsPageViewModel()
        {
            MessagingCenter.Subscribe<UpdateSatisfactoryFormPageViewModel, SatisfactoryForm>(this, "updateSatisfactoryForm", (sender, UpdatedSatisfactoryForm) => { var asf = AllSatisfactoryForms; asf[UpdatedSatisfactoryForm.Index] = UpdatedSatisfactoryForm; AllSatisfactoryForms = new ObservableCollection<SatisfactoryForm>(); AllSatisfactoryForms = asf; });
            MessagingCenter.Subscribe<string>(this, "updateAllSatisfactoryForms", async(sender) => await FetchAllSatisfactoryForms());
            FetchAllSatisfactoryForms();
        }

        private async Task FetchAllSatisfactoryForms()
        {
            List<SatisfactoryForm> AllSatisfactoryFormsFromDb;
            if (App.NetworkStatus)
                AllSatisfactoryFormsFromDb = await ServerApi.FetchAllSatisfactoryForms(new User() { StudentId = App.CurrentUser });
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
            ObservableCollection<SatisfactoryForm> temp = new ObservableCollection<SatisfactoryForm>(AllSatisfactoryFormsFromDb);
            AllSatisfactoryForms = temp;
        }
    }
}