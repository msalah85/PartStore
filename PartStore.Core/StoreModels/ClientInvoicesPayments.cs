using System;

namespace PartStore.Core.StoreModels
{
    // Client Statement
    public class ClientInvoicesPayments
    {
        public int ID { get; set; }
        public DateTime AddDate { get; set; }
        //public TimeSpan? AddTime { get; set; }
        public decimal Amount { get; set; }
        public decimal InAmount { get; set; }
        public decimal OutAmount { get; set; }
        public int Kind { get; set; }
        public decimal Balance { get; set; }
        public long RowNo { get; set; }
    }
}
