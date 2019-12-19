using KawanApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KawanApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAllProfilesPage : ContentPage
    {
        public ViewAllProfilesPage()
        {
            InitializeComponent();
            this.BindingContext = new ViewAllProfilesViewModel();
        }

        private object ViewAllProfilesViewModel()
        {
            throw new NotImplementedException();
        }
    }
}