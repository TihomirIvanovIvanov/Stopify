using Stopify.Services.Mapping;
using Stopify.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Stopify.Web.InputModels
{
    public class ProductOrderInputModel : IMapTo<OrderServiceModel>
    {
        public string ProductId { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
