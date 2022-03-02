using HealthMate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HealthMate.Services
{
    public class FileDataStore : IDataStore<MeasuredItem>
    {
        readonly string appDataDir;
        readonly List<MeasuredItem> items;

        public FileDataStore()
        {
            appDataDir = FileSystem.AppDataDirectory;
        }

        public async Task<bool> AddItemAsync(MeasuredItem item)
        {
            var fileName = Path.Combine(appDataDir, $"{item.MeasurementType}.csv");
            // Create the file and the first headerline on first occurence
            // using csv because we can use it for Excel export too
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
                using (var stream = File.OpenWrite(fileName))
                {

                    using (var streamWriter = new StreamWriter(stream))
                    {
                        if (item.MeasurementType == Measurement.BloodPressure)
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
                        if (item.MeasurementType == Measurement.BloodPressure)
                        {
                            str += $"{item.SysValue};{item.DiaValue}";
                        }
                        else
                        {
                            str += $"{item.MeasuredValue}";
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

        /// <summary>
        /// Reads a certain type of measurements from the data store
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MeasuredItem>> GetItemsAsync(Measurement Type)
        {

            var fileName = Path.Combine(appDataDir, $"{Type}.csv");
            // Create the file and the first headerline on first occurence
            // using csv because we can use it for Excel export too
            if (File.Exists(fileName))
            {
                using (var stream = File.OpenRead(fileName))
                {

                    using (var reader = new StreamReader(stream))
                    {
                        var content = await reader.ReadToEndAsync();
                        foreach (var record in content.Split())
                        {
                            items.Add(new MeasuredItem() { MeasuredValue = record[1], MeasurementDateTime=Convert.ToDateTime(record[2]) });
                        }
                    }
                }
            }
            return await Task.FromResult(items);
        }
    }
}