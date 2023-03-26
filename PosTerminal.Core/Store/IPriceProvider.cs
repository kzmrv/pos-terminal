using System.Threading.Tasks;
using PosTerminal.Core.Model;

namespace PosTerminal.Core.Store
{
    public interface IPriceProvider
    {
        Task<PriceItem?> GetPrice(ProductId product);
        Task SetPrice(ProductId productId, PriceItem price);
    }
}