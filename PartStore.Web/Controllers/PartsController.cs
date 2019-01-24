using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartStore.Core.StoreModels;
using PartStore.Web.Models;
//using System.Data.Entity.QueryableExtensions;

namespace PartStore.Web.Controllers
{
    public class PartsController : Controller
    {
        private readonly PartStoreContext _context;

        public PartsController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: Parts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parts.ToListAsync());
        }

        // GET: Parts/SaveList/5    (itemID)
        public async Task<IActionResult> SaveList(int? id)
        {
            var models = new PartsSaveListViewModel()
            {
                Item = await _context.Items.Include(p => p.Make).Include(p => p.Model).FirstOrDefaultAsync(m => m.ItemId == id),
                Parts = await _context.Parts
                                    //.IncludeFilter(p => p.ItemParts.Where(m => m.ItemId == id).ToList())
                                    .ToListAsync()
            };

            return View(models);
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Parts parts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parts);
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parts = await _context.Parts.FindAsync(id);
            if (parts == null)
            {
                return NotFound();
            }
            return View(parts);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Parts parts)
        {
            if (id != parts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartsExists(parts.Id))
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
            return View(parts);
        }

        // GET: Parts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parts = await _context.Parts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parts == null)
            {
                return NotFound();
            }

            return View(parts);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parts = await _context.Parts.FindAsync(id);
            _context.Parts.Remove(parts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartsExists(int id)
        {
            return _context.Parts.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Save([FromBody] List<ItemParts> parts)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.AddRange(parts);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("Result", new { id = parts.FirstOrDefault()?.ItemId, saved = true });
                }
                else
                {
                    return CreatedAtAction("Result", new { id = 0, saved = false });
                }
            }
            catch (Exception ex) { return CreatedAtAction("Result", new { id = ex.Message, saved = false }); }
        }
    }
}
