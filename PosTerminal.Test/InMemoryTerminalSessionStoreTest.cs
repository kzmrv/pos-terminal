using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PosTerminal.Core.Model;
using PosTerminal.Core.Store;

namespace PosTerminal.Test
{
    public class InMemoryTerminalSessionStoreTest
    {
        [Test]
        public async Task TestCreatesSession()
        {
            var store = new InMemoryTerminalSessionStore();

            var session = await store.GetOrCreateSession("1");

            Assert.IsEmpty(session.Cart.Items);
            Assert.AreEqual(session.TotalPrice, 0.0);
        }

        [Test]
        public async Task TestPersistsSession()
        {
            var store = new InMemoryTerminalSessionStore();

            var cart = new Cart(new Dictionary<ProductId, CartItem>
            {
                [new ProductId("Banana")] = new(1, 2.0)
            });

            var outgoingSession = new ShoppingSession(cart, 2.0);
            await store.SaveSession("1", outgoingSession);

            var receivedSession = await store.GetOrCreateSession("1");

            var cartItem = receivedSession.Cart.Items.Single();

            Assert.AreEqual(cartItem.Key.Id, "Banana");
            Assert.AreEqual(cartItem.Value.Amount, 1.0);
            Assert.AreEqual(receivedSession.TotalPrice, 2.0);
        }
    }
}
