using PartStore.Core.StoreModels;
using System.Collections.Generic;

namespace PartStore.Web.Models
{
    public class InvoiceItemsModel
    {
        public Invoices Invoice { get; set; }
        public List<InvoiceDetails> Items { get; set; }
    }
}
