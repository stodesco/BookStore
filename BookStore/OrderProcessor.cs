using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.Model;

namespace BookStore
{
    public class OrderProcessor
    {
        private readonly ISettingsProvider _settings;

        public OrderProcessor(ISettingsProvider settings)
        {
            _settings = settings;
        }

        public decimal CalculateTotalCost(IEnumerable<Order> orders, bool includeTax)
        {
            var total = 0m;
            foreach (var order in orders)
            {
                // find book
                var book = _settings.Books.FirstOrDefault(c =>
                    c.Title.ToLower().Trim() == order.Title.ToLower().Trim());
                if (book != null)
                {
                    //find category
                    var category =
                        _settings.Categories.FirstOrDefault(
                            c => c.Genre.ToLower().Trim() == book.Genre.ToLower().Trim());
                    if (category != null)
                    {
                        //calculate price of book(s)
                        var price = (order.Quantity * book.UnitPrice) *
                                    ((100 - category.Discount)/100);
                        total += price;
                    }
                }
            }
            //calculate tax (assuming we do not need to calculate gst on delivery)
            if (includeTax)
            {
                total = (total * (100 + _settings.Gst)) / 100;
            }
            //calculate delivery
            if (total > _settings.DeliveryThreshold)
            {
                total += _settings.DeliveryFee;
            }
            return Math.Round(total, 2);
        }
    }
}