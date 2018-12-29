using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class InvoiceTypes
    {
        public InvoiceTypes()
        {
            Invoices = new HashSet<Invoices>();
        }

        public int Id { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Lang))]
        public string Name { get; set; }

        public ICollection<Invoices> Invoices { get; set; }
    }
}
