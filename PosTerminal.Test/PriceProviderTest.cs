using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PosTerminal.Core.Model;
using PosTerminal.Core.Services;
using PosTerminal.Core.Store;

namespace PosTerminal.Test
{
    public class PriceProviderTest
    {
        [Test]
        public async Task TestBasicRead()
        {
            var posTerminal = CreateProvider(new Dictionary<string, int>
            {
                ["Banana"] = 2
            });

            var price = await posTerminal.GetPrice(new ProductId("Banana"));
            Assert.AreEqual(price!.SingleItemPrice, 2);
        }

        [Test]
        public async Task TestOverwritePrice()
        {
            var posTerminal = CreateProvider(new Dictionary<string, int>
            {
                ["Banana"] = 2
            });

            SetPrice(posTerminal, "Banana", 3);

            var price = await posTerminal.GetPrice(new ProductId("Banana"));
            Assert.AreEqual(price!.SingleItemPrice, 3);
        }

        [Test]
        public async Task TestAddPrice()
        {
            var posTerminal = CreateProvider(new Dictionary<string, int>
            {
                ["Banana"] = 2
            });

            SetPrice(posTerminal, "Cream", 3);

            var creamPrice = await posTerminal.GetPrice(new ProductId("Cream"));
            var bananaPrice = await posTerminal.GetPrice(new ProductId("Banana"));

            Assert.AreEqual(creamPrice!.SingleItemPrice, 3);
            Assert.AreEqual(bananaPrice!.SingleItemPrice, 2);
        }

        static InMemoryPriceProvider CreateProvider(Dictionary<string, int> prices)
        {
            return new InMemoryPriceProvider(prices.ToDictionary(
                keyValue => new ProductId(keyValue.Key),
                keyValue => PriceItem.ItemWithoutDiscount(keyValue.Value)));
        }

        static void SetPrice(InMemoryPriceProvider provider, string name, int price)
        {
            provider.SetPrice(new ProductId(name), PriceItem.ItemWithoutDiscount(price));
        }
    }
}