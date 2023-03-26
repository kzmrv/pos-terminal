using System.Collections.Generic;
using System.Threading.Tasks;
using PosTerminal.Core.Model;
using PosTerminal.Core.Store;

namespace PosTerminal.Core.Services
{
    public class DefaultSaleTerminal : ISaleTerminal
    {
        readonly IPriceCalculator priceCalculator;
        readonly IPriceProvider priceProvider;

        public DefaultSaleTerminal(IPriceCalculator priceCalculator, IPriceProvider priceProvider)
        {
            this.priceCalculator = priceCalculator;
            this.priceProvider = priceProvider;
        }

        public async Task<(ScanResult scanResult, ShoppingSession? updatedSession)> Scan(ShoppingSession session, ProductId productId, double amount)
        {
            var price = await priceProvider.GetPrice(productId);
            if (price == null)
            {
                return (ScanResult.ProductNotFound, null);
            }

            var oldItem = session.Cart.Items.GetValueOrDefault(productId);
            var oldAmount = oldItem?.Amount ?? 0;
            var oldPrice = oldItem?.CurrentPrice ?? 0;

            var newAmount = oldAmount + amount;
            var newPrice = priceCalculator.CalculatePrice(price, newAmount);

            var newTotalPrice = session.TotalPrice + (newPrice - oldPrice);

            var newItem = new CartItem(newAmount, newPrice);
            var newItems = new Dictionary<ProductId, CartItem>(session.Cart.Items)
            {
                [productId] = newItem
            };

            return (ScanResult.Ok, new ShoppingSession(new Cart(newItems), newTotalPrice));
        }
    }

    public enum ScanResult
    {
        Ok,
        ProductNotFound
    }
}
