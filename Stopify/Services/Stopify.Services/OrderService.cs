﻿using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using System;
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

        public async Task<bool> CreateOrder(OrderServiceModel orderServiceModel)
        {
            var order = orderServiceModel.To<Order>();

            order.Status = await this.context.OrderStatuses.FirstOrDefaultAsync(status => status.Name == "Active");
            order.IssuedOn = DateTime.UtcNow;

            this.context.Orders.Add(order);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
