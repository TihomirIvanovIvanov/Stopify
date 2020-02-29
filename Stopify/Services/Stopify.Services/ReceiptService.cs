using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly StopifyDbContext context;
        private readonly IOrderService orderService;

        public ReceiptService(StopifyDbContext context, IOrderService orderService)
        {
            this.context = context;
            this.orderService = orderService;
        }

        public async Task<bool> CreateReceipt(string recipientId)
        {
            var orders = await this.context.Orders.Where(order => 
                order.IssuerId == recipientId && order.Status.Name == "Active").ToListAsync();

            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                IssuedOn = DateTime.UtcNow,
                RecipientId = recipientId,
                Orders = orders,
            };

            await this.context.Receipts.AddAsync(receipt);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ReceiptServiceModel> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ReceiptServiceModel> GetAllByRecipientId(string recipientId)
        {
            throw new System.NotImplementedException();
        }
    }
}
