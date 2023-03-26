using NUnit.Framework;
using PosTerminal.Core.Model;
using PosTerminal.Core.Services;

namespace PosTerminal.Test
{
    public class PriceCalculatorTest
    {
        readonly DefaultPriceCalculator calculator = new ();

        [Test]
        public void TestSinglePrice()
        {
            var priceItem = PriceItem.ItemWithoutDiscount(3);
            var totalPrice = calculator.CalculatePrice(priceItem, 10);
            Assert.AreEqual(30, totalPrice);
        }

        [Test]
        public void TestDiscountedPriceLowVolume()
        {
            var priceItem = PriceItem.ItemWithDiscount(10, 3, 20);
            var totalPrice = calculator.CalculatePrice(priceItem, 1);
            Assert.AreEqual(10, totalPrice);
        }

        [Test]
        public void TestDiscountedPriceSingleBatch()
        {
            var priceItem = PriceItem.ItemWithDiscount(10, 3, 20);
            var totalPrice = calculator.CalculatePrice(priceItem, 3);
            Assert.AreEqual(20, totalPrice);
        }

        [Test]
        public void TestDiscountedPriceWithSingleAndDiscount()
        {
            var priceItem = PriceItem.ItemWithDiscount(10, 3, 20);
            var totalPrice = calculator.CalculatePrice(priceItem, 10);
            Assert.AreEqual(70, totalPrice);
        }
    }
}
