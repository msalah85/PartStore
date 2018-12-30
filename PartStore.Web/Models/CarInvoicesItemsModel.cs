using PartStore.Core.StoreModels;
using System.Collections.Generic;

namespace PartStore.Web.Models
{
    public class CarInvoicesItemsModel
    {
        public Items Car { get; set; }
        public List<InvoiceDetails> Invoices { get; set; }
    }
}
