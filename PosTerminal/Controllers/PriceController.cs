using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PosTerminal.Core.Model;
using PosTerminal.Core.Store;

namespace PosTerminal.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceController : ControllerBase
    {
        readonly IPriceProvider priceProvider;

        public PriceController(IPriceProvider priceProvider)
        {
            this.priceProvider = priceProvider;
        }

        [HttpPut("set")]
        public async Task<IActionResult> SetPrice([FromBody] SetPriceCommand command)
        {
            if (command.SingleItemPrice <= 0 || command.DiscountBatchSize < 0 || command.DiscountedPrice < 0)
            {
                return BadRequest("Invalid price");
            }

            var priceItem = command.DiscountBatchSize == null || command.DiscountedPrice == null
                ? PriceItem.ItemWithoutDiscount(command.SingleItemPrice)
                : PriceItem.ItemWithDiscount(command.SingleItemPrice, command.DiscountBatchSize.Value,
                    command.DiscountedPrice.Value);
            await priceProvider.SetPrice(new ProductId(command.ProductId), priceItem);
            return Ok();
        }

        public record SetPriceCommand(string ProductId, double SingleItemPrice, double? DiscountBatchSize,
            double? DiscountedPrice);
    }
}
