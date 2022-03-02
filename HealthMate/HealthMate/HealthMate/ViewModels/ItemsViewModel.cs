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
        private void LoadMockData()
        {
            BloodPressureList.Add(new MeasuredItem() { MeasurementDateTime = DateTime.Now, DiaValue = 80, SysValue = 120 });
            BloodPressureList.Add(new MeasuredItem() { MeasurementDateTime = DateTime.Now, DiaValue = 80, SysValue = 120 });
            BloodPressureList.Add(new MeasuredItem() { MeasurementDateTime = DateTime.Now, DiaValue = 80, SysValue = 120 });
            BloodPressureList.Add(new MeasuredItem() { MeasurementDateTime = DateTime.Now, DiaValue = 80, SysValue = 120 });
            BloodPressureList.Add(new MeasuredItem() { MeasurementDateTime = DateTime.Now, DiaValue = 80, SysValue = 120 });
            BloodPressureList.Add(new MeasuredItem() { MeasurementDateTime = DateTime.Now, DiaValue = 80, SysValue = 120 });
            BloodPressureList.Add(new MeasuredItem() { MeasurementDateTime = DateTime.Now, DiaValue = 80, SysValue = 120 });
        }

        public ItemsViewModel()
        {
        }

        // Load all the values from the three files
        public void OnAppearing()
        {
                IsBusy = true;
            LoadMockData();

        }

        public ObservableCollection<MeasuredItem> BloodPressureList { get; } = new ObservableCollection<MeasuredItem>();
        public ObservableCollection<MeasuredItem> PulseList { get; } = new ObservableCollection<MeasuredItem>();
        public ObservableCollection<MeasuredItem> GlucoseList { get; } = new ObservableCollection<MeasuredItem>();

    }
}
