﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using Stopify.Web.InputModels;
using Stopify.Web.ViewModels.Product.Create;
using Stopify.Web.ViewModels.Product.Delete;
using System.Linq;
using System.Threading.Tasks;

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
            var productTypeServiceModel = Mapper.Map<ProductTypeServiceModel>(productTypeCreateInputModel);

            await this.productService.CreateProductType(productTypeServiceModel);

            return this.Redirect("/");
        }

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
                var allProductTypes = await this.productService.GetAllProductTypes().ToListAsync();

                this.ViewData["types"] = allProductTypes.Select(productType => new ProductCreateProductTypeViewModel
                {
                    Name = productType.Name
                }).ToList();

                return this.View();
            }

            var pictureUrl = await this.cloudinaryService
                .UploadPictureAsync(productCreateInputModel.Picture, productCreateInputModel.Name);

            var productServiceModel = Mapper.Map<ProductServiceModel>(productCreateInputModel);
            productServiceModel.Picture = pictureUrl;

            await this.productService.Create(productServiceModel);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var productEditInputModel = (await this.productService.GetById(id)).To<ProductEditInputModel>();

            if (productEditInputModel == null)
            {
                //TODO: Error handling
                return this.Redirect("/");
            }

            var allProductTypes = await this.productService.GetAllProductTypes().ToListAsync();

            this.ViewData["types"] = allProductTypes.Select(productType => new ProductCreateProductTypeViewModel
            {
                Name = productType.Name
            }).ToList();

            return this.View(productEditInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ProductEditInputModel productEditInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var allProductTypes = await this.productService.GetAllProductTypes().ToListAsync();

                this.ViewData["types"] = allProductTypes.Select(productType => new ProductCreateProductTypeViewModel
                {
                    Name = productType.Name
                }).ToList();

                return this.View(productEditInputModel);
            }

            var pictureUrl = await this.cloudinaryService
                .UploadPictureAsync(productEditInputModel.Picture, productEditInputModel.Name);

            var productServiceModel = Mapper.Map<ProductServiceModel>(productEditInputModel);
            productServiceModel.Picture = pictureUrl;

            await this.productService.Edit(id, productServiceModel);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var productDeleteViewModel = (await this.productService.GetById(id)).To<ProductDeleteViewModel>();

            if (productDeleteViewModel == null)
            {
                //TODO: Error handling
                return this.Redirect("/");
            }

            var allProductTypes = await this.productService.GetAllProductTypes().ToListAsync();

            this.ViewData["types"] = allProductTypes.Select(productType => new ProductCreateProductTypeViewModel
            {
                Name = productType.Name
            }).ToList();

            return this.View(productDeleteViewModel);
        }

        [HttpPost]
        [Route("/Administration/Product/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            await this.productService.DeleteById(id);

            return this.Redirect("/");
        }
    }
}