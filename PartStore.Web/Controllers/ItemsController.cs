using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DataTables.Queryable;
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
        public async Task<IActionResult> Create([Bind("ItemsPk,ItemId,Barcode,AvgCost,LastCost,LastPurchasedDate,Qty,Starred,MakeId,ModelId,YearId,Vin,SalePrice,Discount,NetPrice,Active,Photo,More,LotNo,SupplierCarNo,RefNo,Sold")] Items items)
        {
            if (ModelState.IsValid)
            {   // Incresing the item no
                long serial = 1;
                var latestItem = await _context.Items.LastOrDefaultAsync();
                if (latestItem != null) { serial = (latestItem.ItemsPk) + 1; }
                items.ItemsPk = serial;

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
        public async Task<IActionResult> Edit(long id, [Bind("ItemsPk,ItemId,Barcode,AvgCost,LastCost,LastPurchasedDate,Qty,Starred,MakeId,ModelId,YearId,Vin,SalePrice,Discount,NetPrice,Active,Photo,More,LotNo,SupplierCarNo,RefNo,Sold")] Items items)
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
                Invoices = await _context.InvoiceDetails.Include(v => v.Invoice).ThenInclude(v => v.InvoiceType).Where(i => i.ItemId == id).ToListAsync()
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
            if (id != null) // car id search
            {
                var data = await _context.Items.Include(i => i.Make).Include(i => i.Model).Include(p => p.Photos).Where(i => i.ItemId == id).ToListAsync();
                return View(data);
            }

            if (!string.IsNullOrEmpty(lot)) // lot search
            {
                var _car = await _context.Items.Include(i => i.Make).Include(i => i.Model).Include(p => p.Photos).Where(i => i.LotNo == lot).ToListAsync();
                return View(_car);
            }

            if (!string.IsNullOrEmpty(vin)) // chassis no (VIN) search
            {
                var _car = await _context.Items.Include(i => i.Make).Include(i => i.Model).Include(p => p.Photos).Where(i => i.Vin == vin).ToListAsync();
                return View(_car);
            }

            var nullRsult = await _context.Items.Where(p => p.ItemId == 0).ToListAsync();
            return View(nullRsult);
        }

        //[HttpPost]
        public async Task<JsonResult> LoadDT()
        {
            var query = HttpContext.Request.QueryString.Value;
            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(query);
            var request = new DataTablesRequest<Items>(queryDictionary);

            string searchTerm = request.GlobalSearchValue,
            soldFilter = queryDictionary.FirstOrDefault(p => p.Key == "sold").Value;
            int pageNum = request.PageNumber,
                pageSize = request.PageSize;

            var list = _context.Items.Include(i => i.Make).Include(i => i.Model)
                    .Include(i => i.Photos).Where(p => (string.IsNullOrEmpty(searchTerm) || p.Vin.ToLower().StartsWith(searchTerm.ToLower()) || p.ItemId.ToString().Equals(searchTerm)
                                || p.Make.MakeName.ToLower().StartsWith(searchTerm.ToLower()) || p.Model.ModelName.ToLower().StartsWith(searchTerm.ToLower())) &&
                                (string.IsNullOrEmpty(soldFilter) || p.Sold == Convert.ToBoolean(soldFilter)));

            return Json(new
            {
                draw = request.Draw,
                recordsFiltered = await list.CountAsync(),
                recordsTotal = await list.CountAsync(),
                data = await list.OrderByDescending(i => i.ItemId).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync()
            });

        }

        // GET: Invoices/Sale/5
        public async Task<IActionResult> Sale(int? id)
        {
            var data = await _context.Items.Include(i => i.Make)
                                .Include(d => d.Model)
                                .Include(d => d.Photos)
                                .FirstOrDefaultAsync(m => m.ItemId == id);

            return View(data);
        }

        // POST: Items/EditSale/5
        [HttpPost]
        public async Task<IActionResult> EditSale([FromBody] Items items)
        {
            try
            {
                var _item = _context.Items.Find(items.ItemId);
                _item.Sold = items.Sold;

                _context.Update(_item);
                await _context.SaveChangesAsync();

                return Ok(new { saved = true });
            }
            catch (Exception ex)
            {
                return Ok(new { saved = false, Error = ex.Message });
            }
        }
    }
}