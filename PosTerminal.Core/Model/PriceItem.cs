namespace PosTerminal.Core.Model
{
    public class PriceItem
    {
        public readonly double SingleItemPrice;

        public readonly double? AmountForDiscount;
        public readonly double? DiscountedPrice;

        public static PriceItem ItemWithoutDiscount(double priceValue)
        {
            return new(priceValue, null, null);
        }

        public static PriceItem ItemWithDiscount(double singleItemPriceValue, double amountForDiscount,
            double discountedPriceValue)
        {
            return new(singleItemPriceValue, amountForDiscount, discountedPriceValue);
        }

        PriceItem(double singleItemPrice, double? amountForDiscount, double? discountedPrice)
        {
            SingleItemPrice = singleItemPrice;
            AmountForDiscount = amountForDiscount;
            DiscountedPrice = discountedPrice;
        }
    }
}
