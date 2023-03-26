using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PosTerminal.Core.Model;

namespace PosTerminal.Core.Store
{
    public class InMemoryPriceProvider : IPriceProvider
    {
        readonly Dictionary<ProductId, PriceItem> prices;

        public InMemoryPriceProvider(Dictionary<ProductId, PriceItem> prices)
        {
            this.prices = prices;
        }

        public Task<PriceItem?> GetPrice(ProductId product) => Task.FromResult(prices.GetValueOrDefault(product));

        public Task SetPrice(ProductId productId, PriceItem price)
        {
            prices[productId] = price;
            return Task.CompletedTask;
        }

        public static Dictionary<ProductId, PriceItem> TestData =
            new Dictionary<string, (double single, double? discountBatchSize, double? discountBatchPrice)>
            {
                ["Banana"] = (2, null, null),
                ["Apple"] = (3, 10, 20),

            }.ToDictionary(x => new ProductId(x.Key), x => ToPriceItem(x.Value.single, x.Value.discountBatchSize, x.Value.discountBatchPrice));

        static PriceItem ToPriceItem(double singlePrice, double? discountBatchSize, double? discountBatchPrice)
        {
            if (discountBatchSize == null || discountBatchPrice == null)
            {
                return PriceItem.ItemWithoutDiscount(singlePrice);
            }

            return PriceItem.ItemWithDiscount(singlePrice, discountBatchSize.Value, discountBatchPrice!.Value);
        }
    }
}
