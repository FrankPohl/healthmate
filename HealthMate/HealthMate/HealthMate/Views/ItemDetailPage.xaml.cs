﻿using HealthMate.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace HealthMate.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new MessageDetailViewModel();
        }
    }
}