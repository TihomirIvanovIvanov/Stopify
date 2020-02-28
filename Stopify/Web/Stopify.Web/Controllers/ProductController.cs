using Microsoft.AspNetCore.Mvc;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using Stopify.Web.InputModels;
using Stopify.Web.ViewModels.Product.Details;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stopify.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        private readonly IOrderService orderService;

        public ProductController(IProductService productService, IOrderService orderService)
        {
            this.productService = productService;
            this.orderService = orderService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var productDetails = await this.productService.GetById(id);
            
            var productDetailsViewModel = new ProductDetailsViewModel
            {
                Id = productDetails.Id,
                Name = productDetails.Name,
                ManufacturedOn = productDetails.ManufacturedOn,
                Picture = productDetails.Picture,
                Price = productDetails.Price,
                ProductTypeName = productDetails.ProductType.Name
            };

            return View(productDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Order(ProductOrderInputModel productOrderInputModel)
        {
            var orderServiceModel = productOrderInputModel.To<OrderServiceModel>();

            orderServiceModel.IssuerId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.orderService.CreateOrder(orderServiceModel);

            return this.Redirect("/");
        }
    }
}