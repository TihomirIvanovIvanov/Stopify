using Stopify.Services.Models;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public interface IProductService
    {
        Task<bool> Create(ProductServiceModel productServiceModel);

        Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel);
    }
}
