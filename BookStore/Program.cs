using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BookStore.Model;
using CsvHelper;
using Microsoft.Extensions.Configuration;

namespace BookStore
{
    class Program
    {
        static void Main(string[] args)
        {
            var includeTax = args != null && args.Contains("-t");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true,true)
                .Build();
            var appSettings = configuration.GetSection("AppSettings");
            var books = ReadCsvFromConfig<Book>(appSettings["BookCollectionFile"]);
            var categories = ReadCsvFromConfig<Category>(appSettings["CategoryFile"]);
            var gst = decimal.Parse(appSettings["Gst"]);
            var deliveryFee = decimal.Parse(appSettings["DeliveryFee"]);
            var deliveryThreshold = decimal.Parse(appSettings["DeliveryThreshold"]);

            ISettingsProvider settingsProvider = new SettingsProvider(categories, books, gst, deliveryFee, deliveryThreshold);

            var orders = ReadCsvFromConfig<Order>(appSettings["DefaultOrderFile"]);

            var orderProcessor = new OrderProcessor(settingsProvider);
            var total = orderProcessor.CalculateTotalCost(orders, includeTax);
            Console.WriteLine($"Total: ${total:##,###.00}");
        }

        private static IEnumerable<T> ReadCsvFromConfig<T>(string path)
        {
            IEnumerable<T> ret = null;
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        ret = csv.GetRecords<T>().ToList();
                    }
                }
            }
            return ret;
        }
    }
}