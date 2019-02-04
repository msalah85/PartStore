using System;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Transactions
    {
        [Key]
        public long Id { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;
        public int? BankId { get; set; }
        public int? AccountId { get; set; }
        public decimal Credit { get; set; } = 0;
        public decimal Debit { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal Balance { get; set; } = 0;
        public bool Deleted { get; set; } = false;
        public string TransactionId { get; set; }

        public Accounts Account { get; set; }
        public Banks Bank { get; set; }
    }
}
