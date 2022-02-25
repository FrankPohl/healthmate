using HealthMate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HealthMate.Services
{
    public class MockDataStore : IDataStore<MeasuredItem>
    {
        readonly string appDataDir;
        readonly List<MeasuredItem> items;

        public MockDataStore()
        {
            appDataDir = FileSystem.AppDataDirectory;
            //items = new List<MeasuredItem>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};
        }

        public async Task<bool> AddItemAsync(MeasuredItem item)
        {
            var fileName = $"{item.MeasurementType}.csv";
            // Create the file and the first headerline on first occurence
            // using csv because we can use it for Excel export too
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
                using (var stream = File.OpenWrite(fileName))
                {

                    using (var streamWriter = new StreamWriter(stream))
                    {
                        if (item.MeasurementType == Intent.BloodPressure)
                        {
                            streamWriter.WriteLineAsync("ID;MeasredType;MeasurmentDateTime;Sys;Dia");
                        }
                        else
                        {
                            streamWriter.WriteLineAsync("ID;MeasredType;MeasurmentDateTime;Measaurement");
                        }
                    }
                }
            }
            else
            {
                using (var stream = File.OpenWrite(fileName))
                {

                    using (var streamWriter = new StreamWriter(stream))
                    {
                        string str = $"{item.Id};{item.MeasurementType};{item.MeasurementDateTime}";
                        if (item.MeasurementType == Intent.BloodPressure)
                        {
                            str += $"{item.SysValue};{item.DiaValue}";
                        }
                        else
                        {
                            str += $"{item.Measurement}";
                        }
                        await streamWriter.WriteLineAsync(str);
                    }
                }

            }
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(MeasuredItem item)
        {
            var oldItem = items.Where((MeasuredItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((MeasuredItem arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<MeasuredItem> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<MeasuredItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}