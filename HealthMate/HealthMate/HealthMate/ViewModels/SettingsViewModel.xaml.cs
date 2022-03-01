using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthMate.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsViewModel : ContentPage
    {
        public SettingsViewModel()
        {
            InitializeComponent();
        }
            
        // Load the settings here
        public async void OnAppearing()
        {
            IsBusy = true;
            IsBusy = false;
        }

    }
}