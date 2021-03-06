﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartStore.Core.StoreModels;
using PartStore.Web.Models;

namespace PartStore.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly PartStoreContext _context;

        public InvoicesController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var partStoreContext = _context.Invoices.Include(i => i.Account).Include(i => i.InvoiceType);
            return View(await partStoreContext.OrderByDescending(a => a.Id).ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = new InvoicePrintModel()
            {
                Invoice = await _context.Invoices.Include(i => i.Account)
                                    .Include(i => i.InvoiceDetails).ThenInclude(d => d.Item).ThenInclude(d => d.Model).ThenInclude(d => d.Make)
                                    .FirstOrDefaultAsync(m => m.Id == id),
                CompanyInfo = await _context.Settings.Where(m => m.GroupName.ToLower().Equals("company")).ToListAsync()
            };

            return View(invoice);
        }

        // GET: Invoices/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null) // add
            {
                //ViewData["InvoiceTypeId"] = new SelectList(_context.InvoiceTypes, "Id", "Name");
                //ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a => a.AccountTypeId == 1), "AccountId", "Title"); // Clients
                // new refrence No
                var lastInv = _context.Invoices.LastOrDefault();
                int.TryParse(lastInv?.InvoiceNo, out int newInvNo);
                ViewBag.InvoiceNo = lastInv != null ? string.Format("{0}", newInvNo + 1) : "";
            }
            else // edit
            {
                var invoices = await _context.Invoices.Include(d => d.InvoiceDetails).FirstOrDefaultAsync(m => m.Id == id);
                if (invoices == null)
                {
                    return NotFound();
                }
                //ViewData["InvoiceTypeId"] = new SelectList(_context.InvoiceTypes, "Id", "Name", invoices.InvoiceTypeId);
                //ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a => a.AccountTypeId == 1), "AccountId", "Title", invoices.AccountId);
                ViewBag.InvoiceNo = invoices.InvoiceNo;
            }

            // available cars (Items) list
            //var _cars = await _context.Items.Include(c => c.Make).Include(c => c.Model)
            //    .Select(c => new { c.ItemId, Name = c.ItemId + " - " + c.Make.MakeName + " " + c.Model.ModelName + " " + c.YearId }).ToListAsync();
            //ViewData["Cars"] = new SelectList(_cars, "ItemId", "Name");
            ViewData["InvoiceTypeId"] = new SelectList(_context.InvoiceTypes, "Id", "Name");
            //ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a => a.AccountTypeId == 1), "AccountId", "Title");

            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,InvoiceNo,Notes,TotalAmount,Discount,Tax,NetAmount,UserId,Ip,IsCache,AddDate,AddTime,InvoiceTypeId")] Invoices invoices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a => a.AccountTypeId == 1), "AccountId", "Title", invoices.AccountId);
            return View(invoices);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<string>> Save([FromBody] Invoices invoice)
        {
            try
            {
                if (ModelState.IsValid)  //if (invoices != null)
                {
                    // set ip & user id
                    var exitsBefore = await _context.Invoices.FindAsync(invoice.Id);
                    if (exitsBefore != null)
                    { // update

                        exitsBefore.InvoiceNo = invoice.InvoiceNo;
                        exitsBefore.Notes = invoice.Notes;
                        exitsBefore.NetAmount = invoice.NetAmount;
                        exitsBefore.Discount = invoice.Discount;
                        exitsBefore.TotalAmount = invoice.TotalAmount;
                        exitsBefore.AccountId = invoice.AccountId;
                        exitsBefore.InvoiceTypeId = invoice.InvoiceTypeId;
                        exitsBefore.AddDate = invoice.AddDate;
                        _context.Update(exitsBefore);
                        _context.UpdateRange(invoice.InvoiceDetails);
                    }
                    else // insert
                    {
                        _context.Add(invoice);
                        foreach (var itm in invoice.InvoiceDetails)
                        {
                            itm.InvoiceId = invoice.Id;
                        }
                        _context.AddRange(invoice.InvoiceDetails);
                    }

                    await _context.SaveChangesAsync();
                    return CreatedAtAction("Result", new { id = invoice.Id, saved = true });
                }
                else
                {
                    return CreatedAtAction("Result", new { id = 0, saved = false });
                }
            }
            catch (Exception ex) { return CreatedAtAction("Result", new { id = ex.Message, saved = false }); }
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices
                .Include(a => a.Account)
                .Include(d => d.InvoiceDetails).ThenInclude(d => d.Item).ThenInclude(d => d.Model).ThenInclude(d => d.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
            return CreatedAtAction("Result", new { invoice = invoices, saved = true });
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,InvoiceNo,Notes,TotalAmount,Discount,Tax,NetAmount,UserId,Ip,IsCache,AddDate,AddTime")] Invoices invoices)
        {
            if (id != invoices.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoicesExists(invoices.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a => a.AccountTypeId == 1), "AccountId", "Title", invoices.AccountId);
            return View(invoices);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices
                .Include(i => i.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoices == null)
            {
                return NotFound();
            }

            return View(invoices);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoices = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoices);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoicesExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }


        // GET: Invoices/Client/5
        public async Task<IActionResult> Client(int? id)
        {
            var data = new ClientInvoicesViewModel()
            {
                Client = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountId == id),
                Invoices = await _context.Set<ClientInvoicesPayments>()
                            .FromSql("dbo.ClientStatement @ID = {0}, @From = NULL, @To = NULL", id).ToListAsync() // await _context.Invoices.Include(i => i.InvoiceType).Where(m => m.AccountId == id).ToListAsync()
            };

            return View(data);
        }

        [HttpPost, ActionName("Search")]
        public async Task<IActionResult> SearchClientStatement(int? id, DateTime? from = null, DateTime? to = null)
        {
            string _from = string.Format("{0:MM/dd/yyyy}", from),
            _to = string.Format("{0:MM/dd/yyyy}", to);

            _from = string.IsNullOrEmpty(_from) ? "NULL" : "'" + _from + "'";
            _to = string.IsNullOrEmpty(_to) ? "NULL" : "'" + _to + "'";

            string query = string.Format("EXEC dbo.ClientStatement @ID = {0}, @From={1}, @To={2}", id, _from, _to);

            var data = new ClientInvoicesViewModel()
            {
                Client = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountId == id),
                Invoices = await _context.ClientInvoicesPayments.FromSql(query).ToListAsync()
                //Invoices = await _context.Set<ClientInvoicesPayments>().FromSql("dbo.ClientStatement @ID = {0}, @From={1}, @To={2}", id, _from, _to).ToListAsync() // await _context.Invoices.Include(i => i.InvoiceType).Where(m => m.AccountId == id).ToListAsync()
            };

            return View(nameof(Client), data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCars(string searchTerm, int pageSize, int pageNum)
        {
            var list = _context.Items.Where(p => string.IsNullOrEmpty(searchTerm) || p.Vin.ToLower().StartsWith(searchTerm.ToLower()) || p.ItemId.ToString().Equals(searchTerm)
                                || p.Make.MakeName.ToLower().StartsWith(searchTerm.ToLower()) || p.Model.ModelName.ToLower().StartsWith(searchTerm.ToLower()));

            var result = new Select2Result()
            {
                Results = await list.Select(a => new Select2Model { id = a.ItemId.ToString(), text = string.Format("{0} - {1} {2} {3}", a.ItemId, a.Make.MakeName, a.Model.ModelName, a.YearId) })
                                    .Skip((pageNum * pageSize) - 10).Take(pageSize).ToListAsync(),
                Total = await list.CountAsync()
            };

            return Ok(result);
        }
    }
}