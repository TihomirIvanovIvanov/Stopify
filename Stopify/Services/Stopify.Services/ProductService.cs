﻿using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public class ProductService : IProductService
    {
        private readonly StopifyDbContext context;

        public ProductService(StopifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Create(ProductServiceModel productServiceModel)
        {
            var productTypeId = this.context.ProductTypes
                .Where(productType => productType.Id == productServiceModel.ProductId)
                .Select(productTypeId => productTypeId.Id)
                .FirstOrDefault();

            var product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                ProductTypeId = productTypeId,
                Name = productServiceModel.Name,
                Price = productServiceModel.Price,
                ManufacturedOn = productServiceModel.ManufacturedOn,
            };

            this.context.Products.Add(product);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel)
        {
            var productType = new ProductType
            {
                Name = productTypeServiceModel.Name,
            };

            this.context.ProductTypes.Add(productType);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IQueryable<ProductTypeServiceModel>> GetAllProductTypes()
        {
            var productTypes = this.context.ProductTypes
                .Select(productType => new ProductTypeServiceModel
                {
                    Id = productType.Id,
                    Name = productType.Name
                }).AsQueryable();

            return productTypes;
        }
    }
}
