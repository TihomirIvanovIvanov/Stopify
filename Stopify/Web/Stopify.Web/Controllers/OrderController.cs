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

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IActionResult> Cart()
        {
            var orders = await this.orderService.GetAll()
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