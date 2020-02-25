using Microsoft.AspNetCore.Mvc;
using Stopify.Services.Models;
using Stopify.Web.InputModels;
using System.Threading.Tasks;

namespace Stopify.Web.Areas.Administration.Controllers
{
    public class ProductController : AdminController
    {
        [HttpGet]
        [Route("/Type/Create")]
        public async Task<IActionResult> CreateType()
        {
            return this.View("Type/Create");
        }

        [HttpPost]
        [Route("/Type/Create")]
        public async Task<IActionResult> CreateType(ProductTypeCreateInputModel productTypeCreateInputModel)
        {
            var productTypeServiceModel = new ProductTypeServiceModel
            {
                Name = productTypeCreateInputModel.Name
            };

            return this.Redirect("/");
        }

        [HttpGet(Name = "Create")]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateInputModel productCreateInputModel)
        {
            var productServiceModel = new ProductServiceModel
            {
                Name = productCreateInputModel.Name,
                Price = productCreateInputModel.Price,
                ManufacturedOn = productCreateInputModel.ManufacturedOn,
                ProductType = new ProductTypeServiceModel
                {
                    Name = productCreateInputModel.ProductType
                }
            };
            return this.Redirect("/");
        }
    }
}