using HealthMate.Models;
using HealthMate.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace HealthMate.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        public ItemsViewModel()
        {
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        public void OnAppearing()
        {
                IsBusy = true;
        }

        public ObservableCollection<MeasuredItem> BloodPressureList { get; }
        public ObservableCollection<MeasuredItem> PulseList { get; }
        public ObservableCollection<MeasuredItem> GlucoseList { get; }

    }
}
