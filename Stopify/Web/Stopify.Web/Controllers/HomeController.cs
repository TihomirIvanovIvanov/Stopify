using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Web.ViewModels.Home.Index;
using System.Threading.Tasks;

namespace Stopify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index([FromQuery]string criteria = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var products = await this.productService.GetAllProducts(criteria)
                    .To<ProductHomeViewModel>().ToListAsync();

                return this.View(products);
            }

            this.ViewData["criteria"] = criteria;

            return this.View();
        }
    }
}
