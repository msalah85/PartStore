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
        public string InvoiceNo { get; set; }
        public string Notes { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal NetAmount { get; set; }
        public int? UserId { get; set; }
        public string Ip { get; set; }
        public bool? IsCache { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime AddDate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:hh:mm:ss}")]
        public TimeSpan? AddTime { get; set; }

        public Accounts Account { get; set; }
        public ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public ICollection<Payments> Payments { get; set; }
    }
}
