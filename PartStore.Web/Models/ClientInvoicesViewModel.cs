using PartStore.Core.StoreModels;
using System.Collections.Generic;

namespace PartStore.Web.Models
{
    public class ClientInvoicesViewModel
    {
        public Accounts Client { get; set; }
        public List<ClientInvoicesPayments> Invoices { get; set; }
    }

    //public class ClientInvoicesPayments
    //{
    //    public int ID { get; set; }
    //    public DateTime AddDate { get; set; }
    //    public TimeSpan? AddTime { get; set; }
    //    public decimal Amount { get; set; }
    //    public decimal InAmount { get; set; }
    //    public decimal OutAmount { get; set; }
    //    public int Kind { get; set; }
    //    public decimal Balance { get; set; }
    //    public int AccountID { get; set; }
    //    public int RowNo { get; set; }
    //}
}
