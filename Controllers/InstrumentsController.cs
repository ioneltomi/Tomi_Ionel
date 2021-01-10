using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tomi_Ionel.Data;
using Tomi_Ionel.Models;

namespace Tomi_Ionel.Controllers
{
    public class InstrumentsController : Controller
    {
        private readonly ShopContext _context;

        public InstrumentsController(ShopContext context)
        {
            _context = context;
        }

        // GET: Instruments
        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString,int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var instruments = from b in _context.Instruments
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                instruments = instruments.Where(s => s.Type.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "type_desc":
                    instruments = instruments.OrderByDescending(b => b.Type);
                    break;
                case "Price":
                    instruments = instruments.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    instruments = instruments.OrderByDescending(b => b.Price);
                    break;
                default:
                    instruments = instruments.OrderBy(b => b.Type);
                    break;
            }
            int pageSize = 2;
            return View(await SelectedList<Instrument>.CreateAsync(instruments.AsNoTracking(), pageNumber ??
           1, pageSize));
        }

        // GET: Instruments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Instrument = await _context.Instruments.Include(s => s.Orders).ThenInclude(e => e.Customer).AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            /*
                        var instrument = await _context.Instruments
                            .FirstOrDefaultAsync(m => m.ID == id);
                            */
            if (Instrument == null)
            {
                return NotFound();
            }

            return View(Instrument);
        }

        // GET: Instruments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instruments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Brand,Price")] Instrument instrument)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(instrument);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(instrument);
        }

        // GET: Instruments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instruments.FindAsync(id);
            if (instrument == null)
            {
                return NotFound();
            }
            return View(instrument);
        }

        // POST: Instruments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Instruments.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Instrument>(
            studentToUpdate,
            "",
            s => s.Brand, s => s.Type, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Instruments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instruments.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instrument == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(instrument);
        }

        // POST: Instruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrument = await _context.Instruments.FindAsync(id);
            if (instrument == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Instruments.Remove(instrument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }


        }
    }
}
