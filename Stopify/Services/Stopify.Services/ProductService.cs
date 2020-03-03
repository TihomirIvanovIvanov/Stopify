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
        private const string PriceLowestToHighest = "price-lowest-to-highest";

        private const string PriceHighestToLowest = "price-highest-to-lowest";

        private const string DateOldestToLatest = "date-oldest-to-latest";

        private const string DateLatestToOldest = "date-latest-to-oldest";

        private readonly StopifyDbContext context;

        public ProductService(StopifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Create(ProductServiceModel productServiceModel)
        {
            var productTypeNameFromDb = await this.context.ProductTypes
                .FirstOrDefaultAsync(productType => productType.Name == productServiceModel.ProductType.Name);

            if (productTypeNameFromDb == null)
            {
                throw new ArgumentNullException(nameof(productTypeNameFromDb));
            }

            var product = Mapper.Map<Product>(productServiceModel);
            product.Id = Guid.NewGuid().ToString();
            product.ProductType = productTypeNameFromDb;

            await this.context.Products.AddAsync(product);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel)
        {
            var productType = Mapper.Map<ProductType>(productTypeServiceModel);

            await this.context.ProductTypes.AddAsync(productType);
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

        public IQueryable<ProductServiceModel> GetAllProducts(string criteria = null)
        {
            switch (criteria)
            {
                case PriceLowestToHighest:
                    return this.GetAllProductsByPriceAscending().To<ProductServiceModel>();

                case PriceHighestToLowest:
                    return this.GetAllProductsByPriceDescending().To<ProductServiceModel>();

                case DateOldestToLatest:
                    return this.GetAllProductsByManufacturedOnAscending().To<ProductServiceModel>();

                case DateLatestToOldest:
                    return this.GetAllProductsByManufacturedOnDescending().To<ProductServiceModel>();
            }

            var allProducts = this.context.Products.To<ProductServiceModel>();

            return allProducts;
        }

        public IQueryable<ProductTypeServiceModel> GetAllProductTypes()
        {
            var productTypes = this.context.ProductTypes.To<ProductTypeServiceModel>();

            return productTypes;
        }

        public async Task<ProductServiceModel> GetById(string id)
        {
            var product = await this.context.Products.To<ProductServiceModel>()
                .FirstOrDefaultAsync(product => product.Id == id);

            return product;
        }

        private IQueryable<Product> GetAllProductsByPriceAscending()
        {
            return this.context.Products.OrderBy(product => product.Price);
        }

        private IQueryable<Product> GetAllProductsByPriceDescending()
        {
            return this.context.Products.OrderByDescending(product => product.Price);
        }

        private IQueryable<Product> GetAllProductsByManufacturedOnAscending()
        {
            return this.context.Products.OrderBy(product => product.ManufacturedOn);
        }

        private IQueryable<Product> GetAllProductsByManufacturedOnDescending()
        {
            return this.context.Products.OrderByDescending(product => product.ManufacturedOn);
        }

        public async Task<bool> Edit(string id, ProductServiceModel productServiceModel)
        {
            var productTypeNameFromDb = await this.context.ProductTypes
                .FirstOrDefaultAsync(productType => productType.Name == productServiceModel.ProductType.Name);

            if (productTypeNameFromDb == null)
            {
                throw new ArgumentNullException(nameof(productTypeNameFromDb));
            }

            var product = await this.context.Products.FirstOrDefaultAsync(product => product.Id == id);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.Name = productServiceModel.Name;
            product.Price = productServiceModel.Price;
            product.ManufacturedOn = productServiceModel.ManufacturedOn;
            product.Picture = productServiceModel.Picture;
            product.ProductType = productTypeNameFromDb;

            this.context.Products.Update(product);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
