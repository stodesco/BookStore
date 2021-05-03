using System.Collections.Generic;
using BookStore.Model;

namespace BookStore
{
    public interface ISettingsProvider
    {
        IEnumerable<Category> Categories { get; }
        IEnumerable<Book> Books { get; }
        decimal Gst { get; }
        decimal DeliveryFee { get; }
        decimal DeliveryThreshold { get; }
    }
}