using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PosTerminal.Core.Model;
using PosTerminal.Core.Services;
using PosTerminal.Core.Store;

namespace PosTerminal.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TerminalController : ControllerBase
    {
        readonly ITerminalSessionStore sessionStore;
        readonly ISaleTerminal terminal;

        public TerminalController(ITerminalSessionStore sessionStore, ISaleTerminal terminal)
        {
            this.sessionStore = sessionStore;
            this.terminal = terminal;
        }

        [HttpGet("scan")]
        public async Task<IActionResult> Scan(string productId, double amount, string sessionId)
        {
            var session = await sessionStore.GetOrCreateSession(sessionId);

            var (scanningResult, newSession) = await terminal.Scan(session, new ProductId(productId), amount);
            if (scanningResult != ScanResult.Ok)
            {
                return BadRequest(new
                {
                    ErrorMessage = "Error scanning the product",
                    ErrorCode = scanningResult.ToString()
                });
            }

            await sessionStore.SaveSession(sessionId, newSession!);

            return Ok(new ScanResponse(scanningResult.ToString(), newSession?.TotalPrice,
                newSession?.Cart.Items.ToDictionary(x => x.Key.Id, x => x.Value)));
        }

        record ScanResponse(string Status, double? TotalPrice, Dictionary<string, CartItem>? Cart);
    }
}