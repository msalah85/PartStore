using System;
using System.Collections.Generic;

namespace PartStore.Core.StoreModels
{
    public partial class InvoiceDetails
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public long? ItemId { get; set; }
        public string PartName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; } = 0;
        public decimal SubTotal { get; set; }

        public Invoices Invoice { get; set; }
        public Items Item { get; set; }
    }
}
