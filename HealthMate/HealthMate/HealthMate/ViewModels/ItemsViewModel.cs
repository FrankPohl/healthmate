using HealthMate.Models;
using HealthMate.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HealthMate.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private MeasuredItem _selectedItem;

        public ObservableCollection<ItemsViewModel> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<ItemsViewModel> ItemTapped { get; }

        public ItemsViewModel()
        {
            Items = new ObservableCollection<ItemsViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        //    ItemTapped = new Command<ItemsViewModel>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
 //                   Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public MeasuredItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(MeasuredItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(MessageDetailViewModel.Message)}={item.MeasurementType}");
        }
    }
}