using PosTerminal.Core.Model;

namespace PosTerminal.Core.Services
{
    public interface IPriceCalculator
    {
        double CalculatePrice(PriceItem registryPrice, double amount);
    }
}