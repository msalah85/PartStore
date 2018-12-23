using System;
using System.Collections.Generic;

namespace PartStore.Core.StoreModels
{
    public partial class Accounts
    {
        public Accounts()
        {
            Invoices = new HashSet<Invoices>();
            Payments = new HashSet<Payments>();
        }

        public int AccountId { get; set; }
        public byte? AccountTypeId { get; set; }
        public string Title { get; set; }
        public decimal? BalanceIn { get; set; }
        public decimal? BalanceOut { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string More { get; set; }

        public ICollection<Invoices> Invoices { get; set; }
        public ICollection<Payments> Payments { get; set; }
    }
}
