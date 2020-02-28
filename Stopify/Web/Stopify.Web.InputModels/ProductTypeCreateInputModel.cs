using Stopify.Services.Mapping;
using Stopify.Services.Models;

namespace Stopify.Web.InputModels
{
    public class ProductTypeCreateInputModel : IMapTo<ProductTypeServiceModel>
    {
        public string Name { get; set; }
    }
}
