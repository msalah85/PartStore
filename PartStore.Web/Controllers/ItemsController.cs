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
    public class ItemsController : Controller
    {
        private readonly PartStoreContext _context;

        public ItemsController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var partStoreContext = await _context.Items.Include(i => i.Make).Include(i => i.Model).Include(i => i.Year).
                Include(i => i.Photos).OrderByDescending(i => i.ItemId).ToListAsync();
            return View(partStoreContext);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items.Include(i => i.Make).Include(i => i.Model).Include(i => i.Year).FirstOrDefaultAsync(m => m.ItemId == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["MakeId"] = new SelectList(_context.Makes, "MakeId", "MakeName");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            ViewData["YearId"] = new SelectList(_context.Years, "YearId", "YearId");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemsPk,ItemId,Barcode,AvgCost,LastCost,LastPurchasedDate,Qty,Starred,MakeId,ModelId,YearId,Vin,SalePrice,Discount,NetPrice,Active,Photo,More")] Items items)
        {
            if (ModelState.IsValid)
            {   // Incresing the item no
                //var latestItem = await _context.Items.LastOrDefaultAsync();
                //if (latestItem != null) { items.ItemId = latestItem.ItemId + 1; }

                _context.Add(items);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.Makes, "MakeId", "MakeName", items.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", items.MakeId);
            ViewData["YearId"] = new SelectList(_context.Years, "YearId", "YearId", items.YearId);
            return View(items);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items.FindAsync(id);
            if (items == null)
            {
                return NotFound();
            }
            ViewData["MakeId"] = new SelectList(_context.Makes, "MakeId", "MakeName", items.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", items.ModelId);
            ViewData["YearId"] = new SelectList(_context.Years, "YearId", "YearId", items.YearId);
            return View(items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ItemsPk,ItemId,Barcode,AvgCost,LastCost,LastPurchasedDate,Qty,Starred,MakeId,ModelId,YearId,Vin,SalePrice,Discount,NetPrice,Active,Photo,More")] Items items)
        {
            if (id != items.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(items);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemsExists(items.ItemId))
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
            ViewData["MakeId"] = new SelectList(_context.Makes, "MakeId", "MakeName", items.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", items.ModelId);
            ViewData["YearId"] = new SelectList(_context.Years, "YearId", "YearId", items.YearId);
            return View(items);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items.Include(i => i.Make).Include(i => i.Model).Include(i => i.Year).FirstOrDefaultAsync(m => m.ItemId == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var items = await _context.Items.FindAsync(id);
            _context.Items.Remove(items);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemsExists(long id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }


        // GET: Items/Car/5
        public async Task<IActionResult> Car(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = new CarInvoicesItemsModel()
            {
                Car = await _context.Items.Include(i => i.Make).Include(i => i.Model).FirstOrDefaultAsync(i => i.ItemId == id),
                Invoices = await _context.InvoiceDetails.Where(i => i.ItemId == id).ToListAsync()
            };

            return View(items);
        }
    }
}