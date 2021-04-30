using System.Collections.Generic;
using BookStore.Model;

namespace BookStore
{
    public class SettingsProvider
    {
        public IEnumerable<Category> Categories { get; }
        public IEnumerable<Book> Books { get; }
        public decimal Gst { get; }
        public decimal DeliveryFee { get; }
        public decimal DeliveryThreshold { get; }

        public SettingsProvider(IEnumerable<Category> categories, IEnumerable<Book> books, decimal gst,
            decimal deliveryFee, decimal deliveryThreshold)
        {
            Categories = categories;
            Books = books;
            Gst = gst;
            DeliveryFee = deliveryFee;
            DeliveryThreshold = deliveryThreshold;
        }
    }
}