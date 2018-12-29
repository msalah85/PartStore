using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartStore.Core.StoreModels;
using PartStore.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PartStore.Web.Controllers
{
    public class PhotosController : Controller
    {
        private readonly PartStoreContext _context;
        private IHostingEnvironment _environment;

        public PhotosController(PartStoreContext context, IHostingEnvironment Environment)
        {
            _context = context;
            _environment = Environment;
        }

        // GET: Photos/5    --ItemID
        public async Task<IActionResult> Index(long? id)
        {
            var imgsModel = new PhotoModel
            {
                Photo = await _context.Photos.Where(w => w.ItemId == id).OrderByDescending(i => i.PhotoId).ToListAsync(),
                Car = await _context.Items.Include(p => p.Model).ThenInclude(p => p.Make).FirstOrDefaultAsync(m => m.ItemId == id)
            };

            ViewBag.id = id;
            return View(imgsModel);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos
                .Include(p => p.Item)
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photos == null)
            {
                return NotFound();
            }

            return View(photos);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var photos = await _context.Photos.FindAsync(id);
            var itemId = photos.ItemId;

            _context.Photos.Remove(photos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id= itemId });
        }

        private bool PhotosExists(long id)
        {
            return _context.Photos.Any(e => e.PhotoId == id);
        }

        // POST: Adm/Images/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(PhotoModel item)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "Public");
            List<Photos> Photos = new List<Photos>();
            var _files = HttpContext.Request.Form.Files;
            long stockId = item.Car.ItemId;

            for (int i = 0; i < _files.Count(); i++)
            {
                var file = _files[i];
                string
                    fileExtension = Path.GetExtension(file.FileName),
                    fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fileExtension);


                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream); // Save it to the server

                        Photos.Add(new Photos
                        { // Save it to Db
                            Active = true,
                            Main = false,
                            Url = fileName,
                            ItemId = stockId
                        });
                    }
                }
            }

            // Submit saving to Db
            _context.AddRange(Photos);
            await _context.SaveChangesAsync();

            //return RedirectToAction(nameof(Index), new { id = stockId });
            return Ok(new { count = _files.Count });
        }
    }
}
