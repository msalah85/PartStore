using PartStore.Core.StoreModels;
using PartStore.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PartStore.Web.Services
{
    public class TransactionsService
    {
        private readonly PartStoreContext _context;

        public TransactionsService(PartStoreContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateBankBalanceAsync(Transactions transaction)
        {
            if (transaction == null)
                return false;


            // get latest bank balance in (transactions).
            var latestTrans = _context.Transactions.Where(t => t.Deleted == false && t.BankId == transaction.BankId).LastOrDefault();
            if (latestTrans != null)
            {
                transaction.Balance = latestTrans.Balance;
            }
            else {
                latestTrans = new Transactions();
            }

            // Add +Credit and - Debit.
            transaction.Balance = (latestTrans.Balance + transaction.Credit - transaction.Debit);


            // add as a new payment transaction.
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();


            return true;
        }

        public async Task<bool> UpdateAccountBalanceAsync(Transactions transaction)
        {
            if (transaction == null)
                return false;


            // get latest account balance. (transactions).
            var latestTrans = _context.Transactions.Where(t => t.Deleted == false && t.AccountId == transaction.AccountId).LastOrDefault();
            if (latestTrans != null)
            {
                transaction.Balance = latestTrans.Balance;
            }
            else { latestTrans = new Transactions(); }

            // Add +Credit and - Debit.
            transaction.Balance = (latestTrans.Balance + transaction.Credit - transaction.Debit);


            // add as a new payment transaction.
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task ArchiveAccountInvoicesPaymanetsAsync(int? accountID)
        {
            // Insure that payment is saved.
            // Archive all payments and invoices for the account in case the balance is Zero.
            decimal accountPayments = _context.Payments.Where(p => p.Archived == false && p.AccountId == accountID)?.Sum(p => p.Amount) ?? 0;
            decimal accountInvoices = _context.Invoices.Where(p => p.Archived == false && p.AccountId == accountID)?.Sum(p => p.NetAmount) ?? 0;
            decimal accountBalances = (accountInvoices - accountPayments);


            if (accountBalances <= 0)
            {
                _context.Invoices.Where(x => x.Archived == false && x.AccountId == accountID).ToList().ForEach(a => a.Archived = true);
                _context.Payments.Where(x => x.Archived == false && x.AccountId == accountID).ToList().ForEach(a => a.Archived = true);
                await _context.SaveChangesAsync();
            }
        }

        public bool ValidateBankBalance(int? bankId, decimal AmountToValidate, int? transactionType)
        {
            // فقط فى حال صرف مبلغ
            if (transactionType == (int)PaymentTypesEnum.Credit || transactionType == (int)PaymentTypesEnum.CheckCredit)
                return true;

            // يجب أن يكون المبلغ المصروف أقل من أو يساوى الرصيد بالخزينة/البنك
            var _latestBankBalance = _context.Transactions.Where(b => b.BankId == bankId).LastOrDefault();
            if (_latestBankBalance != null)
            {
                if (_latestBankBalance.Balance >= AmountToValidate)
                    return true;
            }

            return false;
        }

    }
}
