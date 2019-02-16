using PartStore.Core.StoreModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartStore.Web.Models
{
    public class BankBalanceViewModel
    {
        public Banks Bank { get; set; }
        public Transactions BankBalance { get; set; }
        public List<Payments> PaymentsCredit { get; set; }
        public List<Payments> PaymentsDebit { get; set; }
    }
}
