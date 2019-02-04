using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartStore.Web.Models
{
    public enum PaymentTypesEnum
    {
        Debit = 1, // صرف
        Credit = 2, // قبض
        BankTransfer = 3,
        CehckDebit = 4, // صرف بشيك
        CheckCredit = 5 // قبض بشيك
    }
}
