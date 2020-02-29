using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Mapping;
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

        public async Task<string> CreateReceipt(string recipientId)
        {
            var receipt = new Receipt
            {
                Id = Guid.NewGuid().ToString(),
                IssuedOn = DateTime.UtcNow,
                RecipientId = recipientId,
                Orders = this.context.Orders.Where(order =>
                    order.IssuerId == recipientId && order.Status.Name == "Active").ToList()
            };

            await this.orderService.SetOrdersToReceipt(receipt);

            foreach (var order in receipt.Orders)
            {
                await this.orderService.CompleteOrder(order.Id);
            }

            await this.context.Receipts.AddAsync(receipt);
            await this.context.SaveChangesAsync();

            return receipt.Id;
        }

        public IQueryable<ReceiptServiceModel> GetAll()
        {
            var receipts = this.context.Receipts.To<ReceiptServiceModel>();

            return receipts;
        }

        public IQueryable<ReceiptServiceModel> GetAllByRecipientId(string recipientId)
        {
            var receipts = this.context.Receipts
                .Where(receipt => receipt.RecipientId == recipientId)
                .To<ReceiptServiceModel>();

            return receipts;
        }
    }
}
