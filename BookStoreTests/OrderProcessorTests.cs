using BookStore;
using BookStore.Model;
using NUnit.Framework;

namespace BookStoreTests
{
    public class OrderProcessorTests
    {
        private ISettingsProvider _settings;
        private OrderProcessor _processor;
        
        [SetUp]
        public void Setup()
        {
            _settings = new SettingsProvider(
                new[]
                {
                    new Category {Genre = "C1", Discount = 0}, 
                    new Category {Genre = "C2", Discount = 5}
                }, new[]
                {
                    new Book{Title = "B1", Genre = "C1", UnitPrice = 5},
                    new Book{Title = "B2", Genre = "C1", UnitPrice = 15},
                    new Book{Title = "B3", Genre = "C2", UnitPrice = 35},
                    new Book{Title = "B4", Genre = "C2", UnitPrice = 50},
                },
                10, 20, 30);
            _processor = new OrderProcessor(_settings);
        }
        [Test]
        public void CheckSimpleTotalling()
        {
            Assert.AreEqual(20m, 
                _processor.CalculateTotalCost(
                    new []
                    {
                        new Order{Title = "B1", Quantity = 1}, 
                        new Order{Title = "B2", Quantity = 1}
                    }, false));
        }
        [Test]
        public void CheckQuantityTotalling()
        {
            Assert.AreEqual(25m, 
                _processor.CalculateTotalCost(
                    new []
                    {
                        new Order{Title = "B1", Quantity = 2}, 
                        new Order{Title = "B2", Quantity = 1}
                    }, false));
        }
        [Test]
        public void CheckTax()
        {
            Assert.AreEqual(22m, 
                _processor.CalculateTotalCost(
                    new []
                    {
                        new Order{Title = "B1", Quantity = 1}, 
                        new Order{Title = "B2", Quantity = 1}
                    }, true));
        }
        [Test]
        public void CheckDiscountAndDelivery()
        {
            Assert.AreEqual(67.5m, 
                _processor.CalculateTotalCost(
                    new []
                    {
                        new Order{Title = "B4", Quantity = 1}
                    }, false));
        }
        [Test]
        public void CheckDiscountAndDeliveryAndTax()
        {
            Assert.AreEqual(130.82m, 
                _processor.CalculateTotalCost(
                    new []
                    {
                        new Order{Title = "B1", Quantity = 1},
                        new Order{Title = "B2", Quantity = 1},
                        new Order{Title = "B3", Quantity = 1},
                        new Order{Title = "B4", Quantity = 1}
                    }, true));
        }
    }
}