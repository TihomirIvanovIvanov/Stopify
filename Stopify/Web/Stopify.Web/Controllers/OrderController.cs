using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Web.ViewModels.Order.Cart;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stopify.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IActionResult> Cart()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orders = await this.orderService.GetAll()
                .Where(order => order.Status.Name == "Active" && order.IssuerId == userId)
                .To<OrderCartViewModel>()
                .ToListAsync();

            return this.View(orders);
        }

        [HttpPost]
        [Route("/Order/Cart/Complete")]
        public IActionResult Complete()
        {


            return this.View();
        }
    }
}