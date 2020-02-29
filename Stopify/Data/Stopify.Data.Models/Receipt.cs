using System;
using System.Collections.Generic;

namespace Stopify.Data.Models
{
    public class Receipt : BaseModel<string>
    {
        public Receipt()
        {
            this.Orders = new List<Order>();
        }

        public DateTime IssuedOn { get; set; }

        public virtual List<Order> Orders { get; set; }

        public string RecipientId { get; set; }

        public virtual StopifyUser Recipient { get; set; }
    }
}
