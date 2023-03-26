using System.Collections.Generic;

namespace PosTerminal.Core.Model
{
    public record Cart(IReadOnlyDictionary<ProductId, CartItem> Items);

    public record CartItem(double Amount, double CurrentPrice);
}
