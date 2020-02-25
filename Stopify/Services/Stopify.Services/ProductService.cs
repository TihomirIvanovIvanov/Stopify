using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Models;
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
            var product = new Product
            {
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
    }
}
