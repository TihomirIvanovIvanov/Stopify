using Microsoft.AspNetCore.Mvc;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Web.InputModels;
using Stopify.Web.ViewModels.Product.Details;
using System.Threading.Tasks;

namespace Stopify.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet(Name = "Details")]
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

        [HttpPost(Name = "Order")]
        public async Task<IActionResult> Order(ProductOrderInputModel productOrderInputModel)
        {


            return this.Redirect("/");
        }
    }
}