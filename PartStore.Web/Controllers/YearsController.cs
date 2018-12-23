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
    public class YearsController : Controller
    {
        private readonly PartStoreContext _context;

        public YearsController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: Years
        public async Task<IActionResult> Index()
        {
            return View(await _context.Years.ToListAsync());
        }

        // GET: Years/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.Years
                .FirstOrDefaultAsync(m => m.YearId == id);
            if (years == null)
            {
                return NotFound();
            }

            return View(years);
        }

        // GET: Years/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Years/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("YearId")] Years years)
        {
            if (ModelState.IsValid)
            {
                _context.Add(years);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(years);
        }

        // GET: Years/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.Years.FindAsync(id);
            if (years == null)
            {
                return NotFound();
            }
            return View(years);
        }

        // POST: Years/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("YearId")] Years years)
        {
            if (id != years.YearId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(years);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearsExists(years.YearId))
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
            return View(years);
        }

        // GET: Years/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.Years
                .FirstOrDefaultAsync(m => m.YearId == id);
            if (years == null)
            {
                return NotFound();
            }

            return View(years);
        }

        // POST: Years/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var years = await _context.Years.FindAsync(id);
            _context.Years.Remove(years);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YearsExists(int id)
        {
            return _context.Years.Any(e => e.YearId == id);
        }
    }
}
