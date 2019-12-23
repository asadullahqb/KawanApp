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

namespace KawanApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAProfilePage : PopupPage
    {
        public ViewAProfilePage(KawanUser KawanData)
        {
            InitializeComponent();
            this.BindingContext = new ViewAProfileViewModel(KawanData);
        }
    }
}