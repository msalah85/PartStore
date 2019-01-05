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
        public async Task<IActionResult> Create([Bind("ItemsPk,ItemId,Barcode,AvgCost,LastCost,LastPurchasedDate,Qty,Starred,MakeId,ModelId,YearId,Vin,SalePrice,Discount,NetPrice,Active,Photo,More,LotNo")] Items items)
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
        public async Task<IActionResult> Edit(long id, [Bind("ItemsPk,ItemId,Barcode,AvgCost,LastCost,LastPurchasedDate,Qty,Starred,MakeId,ModelId,YearId,Vin,SalePrice,Discount,NetPrice,Active,Photo,More,LotNo")] Items items)
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

        /// <summary>
        /// Search/Get entered car parts by car id or lot or vin.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lot"></param>
        /// <param name="vin"></param>
        /// <returns></returns>
        public async Task<IActionResult> CarInfo(long? id, string lot, string vin)
        {
            var items = new ItemPartsViewModel();
            if (id != null) // car id search
            {
                items.Car = await _context.Items.Include(i => i.Make).Include(i => i.Model).FirstOrDefaultAsync(i => i.ItemId == id);
                items.ItemParts = await _context.ItemParts.Where(i => i.ItemId == id).ToListAsync();
                return View(items);
            }

            if (!string.IsNullOrEmpty(lot)) // lot search
            {
                var _car = await _context.Items.Include(i => i.Make).Include(i => i.Model).FirstOrDefaultAsync(i => i.LotNo == lot);
                if (_car != null)
                {
                    items.Car = _car;
                    items.ItemParts = await (from i in _context.ItemParts join t in _context.Items on i.ItemId equals t.ItemId
                                             where t.LotNo == lot select i).ToListAsync();
                    return View(items);
                }
            }

            if (!string.IsNullOrEmpty(vin)) // chassis no (VIN) search
            {
                var _car = await _context.Items.Include(i => i.Make).Include(i => i.Model).FirstOrDefaultAsync(i => i.Vin == vin);
                if (_car != null)
                {
                    items.Car = _car;
                    items.ItemParts = await (from i in _context.ItemParts join t in _context.Items on i.ItemId equals t.ItemId
                                              where t.Vin == vin select i).ToListAsync();
                    return View(items);
                }
            }

            return View(items);
        }
    }
}