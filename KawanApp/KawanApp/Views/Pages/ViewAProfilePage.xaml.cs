using KawanApp.Models;
using Rg.Plugins.Popup.Pages;
using KawanApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;

namespace KawanApp.Views.Pages
{
    public partial class ViewAProfilePage : ContentPage
    {
        public ViewAProfilePage(KawanUser KawanData)
        {
            InitializeComponent();
            this.BindingContext = new ViewAProfileViewModel(KawanData);
        }
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send<ViewAProfilePage>(this, "navigateBack"); //Send to App.xaml.cs
            return true;
        }

        private void BackIcon_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send<ViewAProfilePage>(this, "navigateBack"); //Send to App.xaml.cs
        }
    }
}