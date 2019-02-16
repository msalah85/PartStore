using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartStore.Core.StoreModels;
using PartStore.Web.Models;

namespace PartStore.Web.Controllers
{
    public class BanksController : Controller
    {
        private readonly PartStoreContext _context;

        public BanksController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: Banks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Banks.ToListAsync());
        }

        // GET: Banks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banks = await _context.Banks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banks == null)
            {
                return NotFound();
            }

            return View(banks);
        }

        // GET: Banks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Banks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TypeId")] Banks banks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banks);
        }

        // GET: Banks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banks = await _context.Banks.FindAsync(id);
            if (banks == null)
            {
                return NotFound();
            }
            return View(banks);
        }

        // POST: Banks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TypeId")] Banks banks)
        {
            if (id != banks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BanksExists(banks.Id))
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
            return View(banks);
        }

        // GET: Banks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banks = await _context.Banks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banks == null)
            {
                return NotFound();
            }

            return View(banks);
        }

        // POST: Banks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banks = await _context.Banks.FindAsync(id);
            _context.Banks.Remove(banks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BanksExists(int id)
        {
            return _context.Banks.Any(e => e.Id == id);
        }

        // GET: Banks/Balances/5
        public async Task<IActionResult> Balances(int? id)
        {
            var data = new BankBalanceViewModel()
            {
                Bank = await _context.Banks.FirstOrDefaultAsync(m => m.Id == id),
                BankBalance = await _context.Transactions.LastOrDefaultAsync(m => m.BankId == id),
                PaymentsCredit = await _context.Payments.Include(p => p.Account).Include(p => p.FromBank).Include(p => p.Invoice).Include(p => p.Operation).Include(p => p.PaymentType).Include(p => p.ToBank)
                                .Where(p => p.ToBankId == id).OrderByDescending(a => a.Id).Take(100).ToListAsync(),
                PaymentsDebit = await _context.Payments.Include(p => p.Account).Include(p => p.FromBank).Include(p => p.Invoice).Include(p => p.Operation).Include(p => p.PaymentType).Include(p => p.ToBank)
                                .Where(p => p.FromBankId == id).OrderByDescending(a => a.Id).Take(100).ToListAsync()
            };

            return View(data);
        }
    }
}
