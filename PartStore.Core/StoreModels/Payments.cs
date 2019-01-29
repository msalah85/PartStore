using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartStore.Core.StoreModels
{
    public partial class Payments
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        [Display(Name = "Amount", ResourceType = typeof(Lang))]
        public decimal Amount { get; set; }
        [Display(Name = "Discount", ResourceType = typeof(Lang))]
        public decimal? Discount { get; set; }
        [Display(Name = "Tax", ResourceType = typeof(Lang))]
        public decimal? Tax { get; set; }
        [Display(Name = "Extra", ResourceType = typeof(Lang))]
        public decimal? Extra { get; set; }
        [Display(Name = "Total", ResourceType = typeof(Lang))]
        public decimal? Total { get; set; }
        [Display(Name = "Account", ResourceType = typeof(Lang))]
        public int? AccountId { get; set; }
        [Display(Name = "PaymentType", ResourceType = typeof(Lang))]
        public int? PaymentTypeId { get; set; }
        [Display(Name = "Operation", ResourceType = typeof(Lang))]
        public int? OperationId { get; set; }
        [Display(Name = "FromBank", ResourceType = typeof(Lang))]
        public int? FromBankId { get; set; }
        [Display(Name = "ToBanks", ResourceType = typeof(Lang))]
        public int? ToBankId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}"), Display(Name = "Date", ResourceType = typeof(Lang))]
        public DateTime? AddDate { get; set; }
        public TimeSpan? AddTime { get; set; }
        [Display(Name = "RefNo", ResourceType = typeof(Lang))]
        public string RefNo { get; set; }
        [Display(Name = "Notes", ResourceType = typeof(Lang))]
        public string Notes { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Lang))]
        public bool? Active { get; set; }
        [Display(Name = "Deleted", ResourceType = typeof(Lang))]
        public bool? Deleted { get; set; }

        [Display(Name = "Account", ResourceType = typeof(Lang))]
        public Accounts Account { get; set; }

        //[ForeignKey("FromBankId")]
        //[InverseProperty("PaymentsFromBank")]
        public Banks FromBank { get; set; }

        [Display(Name = "Invoice", ResourceType = typeof(Lang))]
        public Invoices Invoice { get; set; }
        [Display(Name = "Operation", ResourceType = typeof(Lang))]
        public Operations Operation { get; set; }
        public PaymentTypes PaymentType { get; set; }

        //[ForeignKey("ToBankId")]
        //[InverseProperty("PaymentsToBank")]
        public Banks ToBank { get; set; }
    }
}
