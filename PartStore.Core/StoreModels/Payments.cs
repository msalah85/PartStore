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
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Extra { get; set; }
        public decimal? Total { get; set; }
        public int? AccountId { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? OperationId { get; set; }
        public int? FromBankId { get; set; }
        public int? ToBankId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? AddDate { get; set; }
        public TimeSpan? AddTime { get; set; }
        public string RefNo { get; set; }
        public string Notes { get; set; }

        public Accounts Account { get; set; }

        //[ForeignKey("FromBankId")]
        //[InverseProperty("PaymentsFromBank")]
        public Banks FromBank { get; set; }

        public Invoices Invoice { get; set; }
        public Operations Operation { get; set; }
        public PaymentTypes PaymentType { get; set; }

        //[ForeignKey("ToBankId")]
        //[InverseProperty("PaymentsToBank")]
        public Banks ToBank { get; set; }
    }
}
