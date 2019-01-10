using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartStore.Core.StoreModels;
using PartStore.Web.Models;

namespace PartStore.Web.Controllers
{
    public class ItemPartsController : Controller
    {
        private readonly PartStoreContext _context;

        public ItemPartsController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: ItemParts
        public async Task<IActionResult> Index(long? id)
        {
            var partStoreContext = new ItemPartsViewModel()
            {
                ItemParts = await _context.ItemParts.Include(i => i.Item).Where(p => p.ItemId == id).ToListAsync(),
                ItemPart = new ItemParts() { ItemId = (long)id },
                Car = await _context.Items.Include(p => p.Make).Include(p => p.Model).FirstOrDefaultAsync(i => i.ItemId == id)
            };

            return View(partStoreContext);
        }

        // GET: ItemParts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemParts = await _context.ItemParts.Include(i => i.Item).FirstOrDefaultAsync(m => m.PartId == id);
            if (itemParts == null)
            {
                return NotFound();
            }

            return View(itemParts);
        }

        // GET: ItemParts/Create
        public IActionResult Create()
        {
            //ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId");
            return View();
        }

        // POST: ItemParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartId,ItemId,PartName,Qty,Barcode,AvgCost,LastCost,SalePrice,More,Active,Starred,Deleted,LastPurchasedDate,AddDate")] ItemParts itemParts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemParts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = itemParts.ItemId });
            }
            else
            {
                var invalid = ModelState.Where(p => p.Value.ValidationState == ModelValidationState.Invalid).FirstOrDefault();
                ViewData["ErrorMessage"] = "Error:!" + string.Join(",", invalid.Value.Errors.Select(i => i.ErrorMessage));
            }

            var model = new ItemPartsViewModel()
            {
                ItemParts = await _context.ItemParts.Include(i => i.Item).Where(p => p.ItemId == itemParts.ItemId).ToListAsync(),
                ItemPart = new ItemParts() { ItemId = (long)itemParts.ItemId },
                Car = await _context.Items.Include(p => p.Make).Include(p => p.Model).FirstOrDefaultAsync(i => i.ItemId == itemParts.ItemId)
            };
            return View(nameof(Index), model);
        }

        // GET: ItemParts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemParts = await _context.ItemParts.FindAsync(id);
            if (itemParts == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", itemParts.ItemId);
            return View(itemParts);
        }

        // POST: ItemParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PartId,ItemId,PartName,Qty,Barcode,AvgCost,LastCost,SalePrice,More,Active,Starred,Deleted,LastPurchasedDate,AddDate")] ItemParts itemParts)
        {
            if (id != itemParts.PartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemParts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemPartsExists(itemParts.PartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = itemParts.ItemId });
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", itemParts.ItemId);
            return View(itemParts);
        }

        // GET: ItemParts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemParts = await _context.ItemParts.Include(i => i.Item).ThenInclude(i => i.Model).ThenInclude(i => i.Make)
                .FirstOrDefaultAsync(m => m.PartId == id);
            if (itemParts == null)
            {
                return NotFound();
            }

            return View(itemParts);
        }

        // POST: ItemParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var itemParts = await _context.ItemParts.FindAsync(id);
            _context.ItemParts.Remove(itemParts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = itemParts.ItemId });
        }

        private bool ItemPartsExists(long id)
        {
            return _context.ItemParts.Any(e => e.PartId == id);
        }
    }
}
