using System.Threading.Tasks;
using PosTerminal.Core.Model;

namespace PosTerminal.Core.Services
{
    public interface ISaleTerminal
    {
        Task<(ScanResult scanResult, ShoppingSession? updatedSession)> Scan(ShoppingSession session, ProductId productId, double amount);
    }
}