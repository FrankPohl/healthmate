using HealthMate.Services;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthMate.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            var micservice = DependencyService.Get<IMicService>();
            bool ismIcOK = await micservice.GetPermissionsAsync();
            if (ismIcOK)
            {
                var rc = new RecognitionService();
                await rc.Init();
            }

        }
    }
}