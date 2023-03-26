using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PosTerminal.Core.Model;
using PosTerminal.Core.Services;
using PosTerminal.Core.Store;

namespace PosTerminal.Test
{
    class SaleTerminalTest
    {
        [Test]
        public async Task TestInvalidProduct()
        {
            var priceCalculatorMock = new Mock<IPriceCalculator>();
            var saleTerminal = new DefaultSaleTerminal(priceCalculatorMock.Object, PriceProvider());

            var result = await saleTerminal.Scan(Session(), new ProductId("Blabla"), 5);

            Assert.AreEqual(result.scanResult, ScanResult.ProductNotFound);
            Assert.AreEqual(result.updatedSession, null);
        }

        [Test]
        public async Task TestAddingProducts()
        {
            var priceCalculatorMock = new Mock<IPriceCalculator>();
            priceCalculatorMock.Setup(x => x.CalculatePrice(It.IsAny<PriceItem>(), It.IsAny<double>())).Returns(12);

            var saleTerminal = new DefaultSaleTerminal(priceCalculatorMock.Object, PriceProvider());

            var result = await saleTerminal.Scan(Session(), new ProductId("Banana"), 5);

            Assert.AreEqual(result.scanResult, ScanResult.Ok);
            Assert.AreEqual(result.updatedSession.TotalPrice, 17);
            Assert.AreEqual(result.updatedSession.Cart.Items[new ProductId("Banana")].Amount, 6);
        }

        IPriceProvider PriceProvider()
        {
            var priceProviderMock = new Mock<IPriceProvider>();

            priceProviderMock.Setup(x => x.GetPrice(It.Is<ProductId>(y => y.Id == "Banana"))).ReturnsAsync(PriceItem.ItemWithoutDiscount(2));
            priceProviderMock.Setup(x => x.GetPrice(It.Is<ProductId>(y => y.Id == "Apple"))).ReturnsAsync(PriceItem.ItemWithDiscount(3, 3, 6));

            return priceProviderMock.Object;
        }

        ShoppingSession Session()
        {
            var cart = new Cart(new Dictionary<ProductId, CartItem>
            {
                [new ProductId("Banana")] = new(1, 2),
                [new ProductId("Apple")] = new(1, 3)
            });

            return new ShoppingSession(cart, 7);
        }
    }
}
