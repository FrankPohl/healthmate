using HealthMate.Services;
using HealthMate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthMate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputByVoicePage : ContentPage
    {
        InputByVoiceViewModel _viewModel;
        public InputByVoicePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new InputByVoiceViewModel();

        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var micservice = DependencyService.Get<IMicService>();
            bool ismIcOK = await micservice.GetPermissionsAsync();
            if (ismIcOK)
            {
                _viewModel.OnAppearing();
        }


    }

}
}