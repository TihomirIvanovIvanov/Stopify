using AutoMapper;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using System;
using System.Linq;

namespace Stopify.Web.ViewModels.Receipt.Profile
{
    public class ReceiptProfileViewModel : IMapFrom<ReceiptServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public decimal Total { get; set; }

        public int Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ReceiptServiceModel, ReceiptProfileViewModel>()
                    .ForMember(destination => destination.Total,
                                options => options.MapFrom(origin =>
                               origin.Orders.Sum(order => order.Product.Price * order.Quantity)))
                    .ForMember(destination => destination.Products,
                                options => options.MapFrom(origin => 
                               origin.Orders.Sum(order => order.Quantity)));
        }
    }
}
