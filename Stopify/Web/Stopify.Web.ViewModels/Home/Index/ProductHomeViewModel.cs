using Stopify.Services.Mapping;
using Stopify.Services.Models;

namespace Stopify.Web.ViewModels.Home.Index
{
    public class ProductHomeViewModel : IMapFrom<ProductServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }
    }
}
