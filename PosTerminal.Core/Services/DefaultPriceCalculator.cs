using System;
using PosTerminal.Core.Model;

namespace PosTerminal.Core.Services
{
    public class DefaultPriceCalculator : IPriceCalculator
    {
        public double CalculatePrice(PriceItem registryPrice, double amount)
        {
            if (registryPrice.AmountForDiscount == null || registryPrice.DiscountedPrice == null)
            {
                return amount * registryPrice.SingleItemPrice;
            }

            var instancesWithDiscount = (int) Math.Floor(amount / registryPrice.AmountForDiscount.Value);
            var amountSoldWithDiscount = instancesWithDiscount * registryPrice.AmountForDiscount.Value;
            var restAmount = amount - amountSoldWithDiscount;
            var totalPrice = instancesWithDiscount * registryPrice.DiscountedPrice!.Value
                             + (restAmount * registryPrice.SingleItemPrice);
            return totalPrice;
        }
    }
}
