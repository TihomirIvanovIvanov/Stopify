using System;

namespace Stopify.Data.Models
{
    public class Order : BaseModel<string>
    {
        public DateTime IssuedOn { get; set; }

        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public string IssuerId { get; set; }

        public virtual StopifyUser Issuer { get; set; }

        public int StatusId { get; set; }

        public virtual OrderStatus Status { get; set; }
    }
}
