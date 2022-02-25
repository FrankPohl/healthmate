using HealthMate.ViewModels;
using HealthMate.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HealthMate
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        }

    }
}
