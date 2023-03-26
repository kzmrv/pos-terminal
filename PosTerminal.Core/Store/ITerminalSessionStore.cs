using System.Threading.Tasks;
using PosTerminal.Core.Model;

namespace PosTerminal.Core.Store
{
    public interface ITerminalSessionStore
    {
        Task<ShoppingSession> GetOrCreateSession(string sessionId);
        Task SaveSession(string sessionId, ShoppingSession session);
    }
}