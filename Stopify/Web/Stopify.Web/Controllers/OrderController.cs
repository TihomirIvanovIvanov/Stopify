using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Web.ViewModels.Order.Cart;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stopify.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IReceiptService receiptService;

        public OrderController(IOrderService orderService, IReceiptService receiptService)
        {
            this.orderService = orderService;
            this.receiptService = receiptService;
        }

        public async Task<IActionResult> Cart()
        {
            var orders = await this.orderService.GetAll()
                .To<OrderCartViewModel>()
                .ToListAsync();

            return this.View(orders);
        }

        [HttpPost("/Order/Complete")]
        public async Task<IActionResult> Complete()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var receiptId = await this.receiptService.CreateReceipt(userId);

            return this.Redirect($"/Receipt/Details/{receiptId}");
        }
    }
}