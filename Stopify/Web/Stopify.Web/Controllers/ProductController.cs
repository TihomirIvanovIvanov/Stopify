﻿using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Details(string id)
        {
            var productDetailsViewModel = this.productService.GetById(id).To<ProductDetailsViewModel>();

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