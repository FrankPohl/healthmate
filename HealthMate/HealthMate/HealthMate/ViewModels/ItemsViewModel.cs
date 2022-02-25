using HealthMate.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HealthMate.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public ItemsViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
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
    }
}
