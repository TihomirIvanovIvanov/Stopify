using Microsoft.EntityFrameworkCore;
using Stopify.Data;
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
            var productTypeNameFromDb = await this.context.ProductTypes
                .FirstOrDefaultAsync(productType => productType.Name == productServiceModel.ProductType.Name);

            var product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = productServiceModel.Name,
                Price = productServiceModel.Price,
                ManufacturedOn = productServiceModel.ManufacturedOn,
                ProductType = productTypeNameFromDb,
                Picture = productServiceModel.Picture
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

        public IQueryable<ProductServiceModel> GetAllProducts()
        {
            var allProducts = this.context.Products
                .Select(product => new ProductServiceModel
                {
                    Name = product.Name,
                    Picture = product.Picture,
                    Price = product.Price
                }).AsQueryable();

            return allProducts;
        }

        public IQueryable<ProductTypeServiceModel> GetAllProductTypes()
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
