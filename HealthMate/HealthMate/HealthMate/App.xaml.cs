using HealthMate.Services;
using HealthMate.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthMate
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<FileDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
