using SysLanguages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Accounts
    {
        public Accounts()
        {
            Invoices = new HashSet<Invoices>();
            Payments = new HashSet<Payments>();
            Items = new HashSet<Items>();
            Transactions = new HashSet<Transactions>();
        }

        public int AccountId { get; set; }
        [Display(Name = "AccountType", ResourceType = typeof(Lang))]
        public byte? AccountTypeId { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Lang))]
        public string Title { get; set; }
        [Display(Name = "BalanceIn", ResourceType = typeof(Lang))]
        public decimal? BalanceIn { get; set; }
        [Display(Name = "BalanceOut", ResourceType = typeof(Lang))]
        public decimal? BalanceOut { get; set; }
        [Display(Name = "Phone", ResourceType = typeof(Lang))]
        public string Phone { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Lang))]
        public string Address { get; set; }
        [Display(Name = "Group", ResourceType = typeof(Lang))]
        public string Category { get; set; }
        [Display(Name = "Code", ResourceType = typeof(Lang))]
        public string Code { get; set; }
        [Display(Name = "More", ResourceType = typeof(Lang))]
        public string More { get; set; }

        public ICollection<Invoices> Invoices { get; set; }
        public ICollection<Items> Items { get; set; }
        public ICollection<Payments> Payments { get; set; }
        public AccountTypes AccountType { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
