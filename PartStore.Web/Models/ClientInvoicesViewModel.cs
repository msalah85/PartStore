using PartStore.Core.StoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartStore.Web.Models
{
    public class ClientInvoicesViewModel
    {
        public Accounts Client { get; set; }
        public List<Invoices> Invoices { get; set; }
    }
}
