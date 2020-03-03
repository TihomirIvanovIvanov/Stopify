using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using Stopify.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Stopify.Tests.Services
{
    public class ProductServiceTests
    {
        private IProductService productService;

        public ProductServiceTests()
        {
            MapperInitializer.InitialzeMapper();
        }

        [Fact]
        public async Task GetAllProducts_WithDummyData_ShouldReturnCorrectResults()
        {
            var errorMsgPrefix = "ProductService GetAllProducts() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            SeedData(context);
            this.productService = new ProductService(context);


            var actualResult = await this.productService.GetAllProducts().ToListAsync();
            var expectedResult = GetDummyData().To<ProductServiceModel>().ToList();

            for (int i = 0; i < expectedResult.Count; i++)
            {
                var expectedEntry = expectedResult[i];
                var actualEntry = actualResult[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMsgPrefix + " Name is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, errorMsgPrefix + " Price is not returned properly.");
                Assert.True(expectedEntry.Picture == actualEntry.Picture, errorMsgPrefix + " Picture is not returned properly.");
                Assert.True(expectedEntry.ProductType.Name == actualEntry.ProductType.Name, errorMsgPrefix + " ProductType is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProducts_WithZeroData_ShouldReturnEmptyResults()
        {
            var errorMsgPrefix = "ProductService GetAllProducts() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(context);


            var actualResult = await this.productService.GetAllProducts().ToListAsync();

            Assert.True(actualResult.Count == 0, errorMsgPrefix);
        }

        [Fact]
        public async Task GetById_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMsgPrefix = "ProductService GetById() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            SeedData(context);
            this.productService = new ProductService(context);

            var expectedData = context.Products.First().To<ProductServiceModel>();
            var actualData = await this.productService.GetById(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, errorMsgPrefix + " Id is not returned properly.");
            Assert.True(expectedData.Name == actualData.Name, errorMsgPrefix + " Name is not returned properly.");
            Assert.True(expectedData.Price == actualData.Price, errorMsgPrefix + " Price is not returned properly.");
            Assert.True(expectedData.Picture == actualData.Picture, errorMsgPrefix + " Picture is not returned properly.");
            Assert.True(expectedData.ProductType.Name == actualData.ProductType.Name, errorMsgPrefix + " ProductType is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonExistentId_ShouldReturnNull()
        {
            var errorMsgPrefix = "ProductService GetById() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            SeedData(context);
            this.productService = new ProductService(context);

            var actualData = await this.productService.GetById("prakash");

            Assert.True(actualData == null, errorMsgPrefix + " Id is not returned properly.");
        }

        [Fact]
        public async Task Create_WithCorrectData_ShouldSuccessfullyCreate()
        {
            var errorMsgPrefix = "ProductService Create() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            SeedData(context);
            this.productService = new ProductService(context);

            var testProductService = new ProductServiceModel
            {
                Name = "Pesho",
                Price = 5,
                ManufacturedOn = DateTime.UtcNow,
                Picture = "src/res/default.png",
                ProductType = new ProductTypeServiceModel
                {
                    Name = "Television",
                }
            };

            var actualResult = await this.productService.Create(testProductService);

            Assert.True(actualResult, errorMsgPrefix);
        }

        private List<Product> GetDummyData()
        {
            return new List<Product>()
            {
                new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Melissa AirConditioner XSQ-500",
                    Price = 5000.99M,
                    ManufacturedOn = DateTime.UtcNow.AddDays(-15),
                    Picture = "src/pics/something/airconditioner",
                    ProductType = new ProductType
                    {
                        Name = "AirConditioner"
                    }
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Samsung STY Plazma",
                    Price = 25000.00M,
                    ManufacturedOn = DateTime.UtcNow.AddDays(-45),
                    Picture = "src/pics/something/tv",
                    ProductType = new ProductType
                    {
                        Name = "Television"
                    }
                }
            };
        }

        private async void SeedData(StopifyDbContext context)
        {
            context.AddRange(GetDummyData());
            await context.SaveChangesAsync();
        }
    }
}