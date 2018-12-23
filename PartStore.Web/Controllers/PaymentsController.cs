using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartStore.Core.StoreModels;

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
            if (ModelState.IsValid)
            {
                _context.Add(payments);
                await _context.SaveChangesAsync();
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
            if (id != payments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payments);
                    await _context.SaveChangesAsync();
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
    }
}
