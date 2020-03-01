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
    public class OrderService : IOrderService
    {
        private readonly StopifyDbContext context;

        public OrderService(StopifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CompleteOrder(string orderId)
        {
            var orderFromDb = await this.context.Orders
                .FirstOrDefaultAsync(order => order.Id == orderId);

            //TODO: Validate that the requisted order is existent and with status "Active"

            orderFromDb.Status = await this.context.OrderStatuses
                .FirstOrDefaultAsync(status => status.Name == "Completed");

            this.context.Update(orderFromDb);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateOrder(OrderServiceModel orderServiceModel)
        {
            var order = orderServiceModel.To<Order>();

            order.Id = Guid.NewGuid().ToString();
            order.Status = await this.context.OrderStatuses.FirstOrDefaultAsync(status => status.Name == "Active");
            order.IssuedOn = DateTime.UtcNow;

            await this.context.Orders.AddAsync(order);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<OrderServiceModel> GetAll()
        {
            var allActiveOrders = this.context.Orders
                .Where(order => order.Status.Name == "Active")
                .To<OrderServiceModel>();

            return allActiveOrders;
        }

        public async Task<bool> IncreaseQuantity(string orderId)
        {
            var orderFromDb = await this.context.Orders
                .FirstOrDefaultAsync(order => order.Id == orderId);

            orderFromDb.Quantity++;

            this.context.Update(orderFromDb);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ReduceQuantity(string orderId)
        {
            var orderFromDb = await this.context.Orders
                   .FirstOrDefaultAsync(order => order.Id == orderId);

            orderFromDb.Quantity--;

            this.context.Update(orderFromDb);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task SetOrdersToReceipt(Receipt receipt)
        {
            receipt.Orders = await this.context.Orders.Where(order =>
                order.IssuerId == receipt.RecipientId && order.Status.Name == "Active").ToListAsync();
        }
    }
}
