using System;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    // Client Statement
    public class ClientInvoicesPayments
    {
        [Key]
        public long RowNo { get; set; }
        public int ID { get; set; }
        public DateTime AddDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal Amount { get; set; }
        //[DisplayFormat(DataFormatString = "{0:0,0.0}")]
        public decimal InAmount { get; set; }
        //[DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal OutAmount { get; set; }
        public int Kind { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal Balance { get; set; }
    }
}
