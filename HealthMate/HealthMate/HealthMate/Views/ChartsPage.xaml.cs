using HealthMate.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace HealthMate.Views
{
    public partial class ChartsPage : ContentPage
    {
        public ChartsPage()
        {
            InitializeComponent();
            BindingContext = new MessageDetailViewModel();
        }
    }
}