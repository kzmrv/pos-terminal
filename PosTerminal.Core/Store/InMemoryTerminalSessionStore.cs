using System.Collections.Generic;
using System.Threading.Tasks;
using PosTerminal.Core.Model;

namespace PosTerminal.Core.Store
{
    public class InMemoryTerminalSessionStore : ITerminalSessionStore
    {
        static readonly Dictionary<string, ShoppingSession> sessions = new();

        public Task<ShoppingSession> GetOrCreateSession(string sessionId)
        {
            if (!sessions.ContainsKey(sessionId))
            {
                return Task.FromResult(new ShoppingSession(new Cart(new Dictionary<ProductId, CartItem>()), 0));
            }

            return Task.FromResult(sessions[sessionId]);
        }

        public Task SaveSession(string sessionId, ShoppingSession session)
        {
            sessions[sessionId] = session;
            return Task.CompletedTask;
        }
    }
}
