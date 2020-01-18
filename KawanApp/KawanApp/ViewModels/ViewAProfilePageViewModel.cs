using KawanApp.Helpers;
using KawanApp.Interfaces;
using KawanApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class ViewAProfilePageViewModel : BaseViewModel
    {
        private KawanUser _kawanUser;
        public KawanUser KawanUser
        {
            get => _kawanUser;
            set
            {
                _kawanUser = value;
                OnPropertyChanged();
            }
        }

        public ViewAProfilePageViewModel(KawanUser KawanData)
        {
            KawanUser = KawanData;
        }
    }
}