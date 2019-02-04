using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Invoices
    {
        public Invoices()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();
            Payments = new HashSet<Payments>();
        }

        public int Id { get; set; }
        public int? AccountId { get; set; }
        [Display(Name = "RefNo", ResourceType = typeof(Lang))]
        public string InvoiceNo { get; set; }
        [Display(Name = "Notes", ResourceType = typeof(Lang))]
        public string Notes { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        [Display(Name = "Total", ResourceType = typeof(Lang))]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Discount", ResourceType = typeof(Lang))]
        public decimal Discount { get; set; }
        [Display(Name = "Tax", ResourceType = typeof(Lang))]
        public decimal? Tax { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0}"), Display(Name = "NetAmount", ResourceType = typeof(Lang))]
        public decimal NetAmount { get; set; }
        public int? UserId { get; set; }
        public string Ip { get; set; }
        public bool? IsCache { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Lang))]
        public bool? Active { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}"), Display(Name = "Date", ResourceType = typeof(Lang))]
        public DateTime AddDate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:hh:mm:ss}")]
        public DateTime? AddTime { get; set; } // = DateTime.UtcNow;
        public bool? Archived { get; set; } = false;

        [Display(Name = "InvoiceType", ResourceType = typeof(Lang))]
        public int? InvoiceTypeId { get; set; }
        [Display(Name = "Account", ResourceType = typeof(Lang))]
        public Accounts Account { get; set; }
        [Display(Name = "InvoiceType", ResourceType = typeof(Lang))]
        public InvoiceTypes InvoiceType { get; set; }
        public ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public ICollection<Payments> Payments { get; set; }
    }
}
