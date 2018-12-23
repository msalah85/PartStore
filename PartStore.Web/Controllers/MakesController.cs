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
    public class MakesController : Controller
    {
        private readonly PartStoreContext _context;

        public MakesController(PartStoreContext context)
        {
            _context = context;
        }

        // GET: Makes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Makes.ToListAsync());
        }

        // GET: Makes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makes = await _context.Makes
                .FirstOrDefaultAsync(m => m.MakeId == id);
            if (makes == null)
            {
                return NotFound();
            }

            return View(makes);
        }

        // GET: Makes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Makes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MakeId,MakeName")] Makes makes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(makes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(makes);
        }

        // GET: Makes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makes = await _context.Makes.FindAsync(id);
            if (makes == null)
            {
                return NotFound();
            }
            return View(makes);
        }

        // POST: Makes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MakeId,MakeName")] Makes makes)
        {
            if (id != makes.MakeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(makes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MakesExists(makes.MakeId))
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
            return View(makes);
        }

        // GET: Makes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makes = await _context.Makes
                .FirstOrDefaultAsync(m => m.MakeId == id);
            if (makes == null)
            {
                return NotFound();
            }

            return View(makes);
        }

        // POST: Makes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var makes = await _context.Makes.FindAsync(id);
            _context.Makes.Remove(makes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MakesExists(int id)
        {
            return _context.Makes.Any(e => e.MakeId == id);
        }
    }
}
