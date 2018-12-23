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
    public class ModelsController : Controller
    {
        private readonly PartStoreContext _context;

        public ModelsController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: Models
        public async Task<IActionResult> Index()
        {
            var partStoreContext = _context.Models.Include(m => m.Make);
            return View(await partStoreContext.ToListAsync());
        }

        // GET: Models/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var models = await _context.Models
                .Include(m => m.Make)
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (models == null)
            {
                return NotFound();
            }

            return View(models);
        }

        // GET: Models/Create
        public IActionResult Create()
        {
            ViewData["MakeId"] = new SelectList(_context.Makes, "MakeId", "MakeName");
            return View();
        }

        // POST: Models/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,MakeId,ModelName")] Core.StoreModels.Models models)
        {
            if (ModelState.IsValid)
            {
                _context.Add(models);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.Makes, "MakeId", "MakeName", models.MakeId);
            return View(models);
        }

        // GET: Models/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var models = await _context.Models.FindAsync(id);
            if (models == null)
            {
                return NotFound();
            }
            ViewData["MakeId"] = new SelectList(_context.Makes, "MakeId", "MakeName", models.MakeId);
            return View(models);
        }

        // POST: Models/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelId,MakeId,ModelName")] Core.StoreModels.Models  models)
        {
            if (id != models.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(models);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelsExists(models.ModelId))
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
            ViewData["MakeId"] = new SelectList(_context.Makes, "MakeId", "MakeName", models.MakeId);
            return View(models);
        }

        // GET: Models/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var models = await _context.Models
                .Include(m => m.Make)
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (models == null)
            {
                return NotFound();
            }

            return View(models);
        }

        // POST: Models/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var models = await _context.Models.FindAsync(id);
            _context.Models.Remove(models);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelsExists(int id)
        {
            return _context.Models.Any(e => e.ModelId == id);
        }
    }
}
