using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Mapping;
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

            var product = Mapper.Map<Product>(productServiceModel);
            product.Id = Guid.NewGuid().ToString();
            product.ProductType = productTypeNameFromDb;

            this.context.Products.Add(product);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel)
        {
            var productType = Mapper.Map<ProductType>(productTypeServiceModel);

            this.context.ProductTypes.Add(productType);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteById(string id)
        {
            var product = await this.context.Products.FirstOrDefaultAsync(product => product.Id == id);

            this.context.Products.Remove(product);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ProductServiceModel> GetAllProducts()
        {
            var allProducts = this.context.Products.To<ProductServiceModel>();

            return allProducts;
        }

        public IQueryable<ProductTypeServiceModel> GetAllProductTypes()
        {
            var productTypes = this.context.ProductTypes.To<ProductTypeServiceModel>();

            return productTypes;
        }

        public ProductServiceModel GetById(string id)
        {
            var product = this.context.Products.To<ProductServiceModel>()
                .FirstOrDefault(product => product.Id == id);

            return product;
        }
    }
}
