using Stopify.Services.Models;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(OrderServiceModel orderServiceModel);
    }
}
