using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.Queryable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartStore.Core.StoreModels;
using PartStore.Web.Models;
using PartStore.Web.Services;

namespace PartStore.Web.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly PartStoreContext _context;

        public PaymentsController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var partStoreContext = _context.Payments.Include(p => p.Account).Include(p => p.FromBank).Include(p => p.Invoice).Include(p => p.Operation).Include(p => p.PaymentType).Include(p => p.ToBank);
            return View(await partStoreContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payments = await _context.Payments
                .Include(p => p.Account)
                .Include(p => p.FromBank)
                .Include(p => p.Invoice)
                .Include(p => p.Operation)
                .Include(p => p.PaymentType)
                .Include(p => p.ToBank)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payments == null)
            {
                return NotFound();
            }

            return View(payments);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Title");
            ViewData["FromBankId"] = new SelectList(_context.Banks, "Id", "Name");
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "InvoiceNo");
            ViewData["OperationId"] = new SelectList(_context.Operations, "Id", "Name");
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name");
            ViewData["ToBankId"] = new SelectList(_context.Banks, "Id", "Name");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceId,Amount,Discount,Tax,Extra,Total,AccountId,PaymentTypeId,OperationId,FromBankId,ToBankId,AddDate,AddTime,RefNo,Notes")] Payments payments)
        {
            var isValidtaedAmount = new TransactionsService(_context).ValidateBankBalance(payments.FromBankId, payments.Amount, payments.OperationId);
            if (ModelState.IsValid && isValidtaedAmount)
            {
                _context.Add(payments);
                await _context.SaveChangesAsync();

                #region "Update (Account/Banks) Balances"
                decimal _credit = 0, _debit = 0, _toCredit = 0, _toDebit = 0;
                var _trans = new TransactionsService(_context);

                // قبض
                if (payments.OperationId == (int)PaymentTypesEnum.Credit || payments.OperationId == (int)PaymentTypesEnum.CheckCredit)
                {
                    _credit = _toCredit = payments.Amount;
                    //_debit = _toDebit = 0;
                }
                // صرف
                else if (payments.OperationId == (int)PaymentTypesEnum.Debit || payments.OperationId == (int)PaymentTypesEnum.CehckDebit)
                {
                    _debit = _toDebit = payments.Amount;
                    _credit = _toCredit = 0;
                }
                // تحويل بنكى من بنك إلى بنك
                else if (payments.OperationId == (int)PaymentTypesEnum.BankTransfer)
                {
                    _debit = payments.Amount; // Debit from
                    _toCredit = payments.Amount; // Debit to.

                    _toDebit = _credit = 0;
                }

                // Update account balance
                if (payments.AccountId > 0)
                {
                    var at = new Transactions()
                    {
                        AccountId = payments.AccountId,
                        AddDate = DateTime.Now,
                        Credit = _credit,
                        Debit = _debit,
                        TransactionId = payments.Id.ToString()
                    };
                    await _trans.UpdateAccountAccountAsync(at);
                }
                // Update FromBank balance.
                if (payments.FromBankId > 0)
                {
                    var bt = new Transactions()
                    {
                        BankId = payments.FromBankId,
                        AddDate = DateTime.Now,
                        Credit = _credit,
                        Debit = _debit,
                        TransactionId = payments.Id.ToString()
                    };
                    await _trans.UpdateBankAccountAsync(bt);
                }
                // Update ToBank balance.
                if (payments.ToBankId > 0)
                {
                    var bt = new Transactions()
                    {
                        BankId = payments.ToBankId,
                        AddDate = DateTime.Now,
                        Credit = _toCredit,
                        Debit = _toDebit,
                        TransactionId = payments.Id.ToString()
                    };
                    await _trans.UpdateBankAccountAsync(bt);
                }
                #endregion


                // Archive client transactions in case he paying to us (قبض - قبض بشيك).
                if (payments.AccountId > 0 && (payments.OperationId == (int)PaymentTypesEnum.Credit || payments.OperationId == (int)PaymentTypesEnum.CheckCredit))
                    await _trans.ArchiveAccountInvoicesPaymanetsAsync(payments.AccountId);


                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Title", payments.AccountId);
            ViewData["FromBankId"] = new SelectList(_context.Banks, "Id", "Name", payments.FromBankId);
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "InvoiceNo", payments.InvoiceId);
            ViewData["OperationId"] = new SelectList(_context.Operations, "Id", "Name", payments.OperationId);
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name", payments.PaymentTypeId);
            ViewData["ToBankId"] = new SelectList(_context.Banks, "Id", "Name", payments.ToBankId);
            if (!isValidtaedAmount)
                ViewData["BankAmountMessage"] = SysLanguages.Lang.ValidateBankBalanceMessage;


            return View(payments);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payments = await _context.Payments.FindAsync(id);
            if (payments == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Title", payments.AccountId);
            ViewData["FromBankId"] = new SelectList(_context.Banks, "Id", "Name", payments.FromBankId);
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "InvoiceNo", payments.InvoiceId);
            ViewData["OperationId"] = new SelectList(_context.Operations, "Id", "Name", payments.OperationId);
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name", payments.PaymentTypeId);
            ViewData["ToBankId"] = new SelectList(_context.Banks, "Id", "Name", payments.ToBankId);
            return View(payments);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceId,Amount,Discount,Tax,Extra,Total,AccountId,PaymentTypeId,OperationId,FromBankId,ToBankId,AddDate,AddTime,RefNo,Notes")] Payments payments)
        {
            if (id != payments.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    payments.AddTime = DateTime.UtcNow;
                    _context.Update(payments);
                    await _context.SaveChangesAsync();

                    // Archive client transactions in case he paying to us (قبض - قبض بشيك).
                    if (payments.AccountId > 0 && (payments.OperationId == (int)PaymentTypesEnum.Credit || payments.OperationId == (int)PaymentTypesEnum.CheckCredit))
                        await new TransactionsService(_context).ArchiveAccountInvoicesPaymanetsAsync(payments.AccountId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentsExists(payments.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "Title", payments.AccountId);
            ViewData["FromBankId"] = new SelectList(_context.Banks, "Id", "Name", payments.FromBankId);
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "InvoiceNo", payments.InvoiceId);
            ViewData["OperationId"] = new SelectList(_context.Operations, "Id", "Name", payments.OperationId);
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name", payments.PaymentTypeId);
            ViewData["ToBankId"] = new SelectList(_context.Banks, "Id", "Name", payments.ToBankId);

            return View(payments);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payments = await _context.Payments
                .Include(p => p.Account)
                .Include(p => p.FromBank)
                .Include(p => p.Invoice)
                .Include(p => p.Operation)
                .Include(p => p.PaymentType)
                .Include(p => p.ToBank)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payments == null)
            {
                return NotFound();
            }

            return View(payments);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payments = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentsExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }

        [ActionName("LoadDT")]
        public async Task<JsonResult> ClientStatement()
        {
            var query = HttpContext.Request.QueryString.Value;
            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(query);
            var request = new DataTablesRequest<Items>(queryDictionary);

            string searchTerm = request.GlobalSearchValue,
            _filter = queryDictionary.FirstOrDefault(p => p.Key == "id").Value; // accountID
            int pageNum = request.PageNumber,
                pageSize = request.PageSize;

            var list = _context.Set<ClientInvoicesPayments>().FromSql("dbo.ClientStatement @ID={0}, @Archived=1", _filter).OrderByDescending(p => p.RowNo);

            return Json(new
            {
                draw = request.Draw,
                recordsFiltered = await list.CountAsync(),
                recordsTotal = await list.CountAsync(),
                data = await list.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync()
            });
        }

    }
}
