using PartStore.Core.StoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartStore.Web.Models
{
    public class InvoicePrintModel
    {
        public Invoices Invoice { get; set; }
        public List<Settings> CompanyInfo { get; set; }
    }
}
