using Microsoft.AspNetCore.Mvc;
using Stopify.Services;
using Stopify.Services.Models;
using Stopify.Web.InputModels;
using Stopify.Web.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Stopify.Web.Areas.Administration.Controllers
{
    public class ProductController : AdminController
    {
        private readonly IProductService productService;

        private readonly ICloudinaryService cloudinaryService;

        public ProductController(IProductService productService, ICloudinaryService cloudinaryService)
        {
            this.productService = productService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet("/Administration/Product/Type/Create")]
        public async Task<IActionResult> CreateType()
        {
            return this.View("Type/Create");
        }

        [HttpPost("/Administration/Product/Type/Create")]
        public async Task<IActionResult> CreateType(ProductTypeCreateInputModel productTypeCreateInputModel)
        {
            var productTypeServiceModel = new ProductTypeServiceModel
            {
                Name = productTypeCreateInputModel.Name
            };

            await this.productService.CreateProductType(productTypeServiceModel);

            return this.Redirect("/");
        }

        [HttpGet(Name = "Create")]
        public async Task<IActionResult> Create()
        {
            var allProductTypes = await this.productService.GetAllProductTypes().ToListAsync();

            this.ViewData["types"] = allProductTypes.Select(productType => new ProductCreateProductTypeViewModel
            {
                Name = productType.Name
            }).ToList();

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateInputModel productCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var pictureUrl = await this.cloudinaryService
                .UploadPictureAsync(productCreateInputModel.Picture, productCreateInputModel.Name);

            var productServiceModel = new ProductServiceModel
            {
                Name = productCreateInputModel.Name,
                Price = productCreateInputModel.Price,
                ManufacturedOn = productCreateInputModel.ManufacturedOn,
                ProductType = new ProductTypeServiceModel
                {
                    Name = productCreateInputModel.ProductType
                },
                Picture = pictureUrl
            };

            await this.productService.Create(productServiceModel);

            return this.Redirect("/");
        }
    }
}