using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tomi_Ionel.Data;
using Tomi_Ionel.Models;
using Tomi_Ionel.Models.ShopViewModel;

namespace Tomi_Ionel.Controllers
{
    public class ManufacturersController : Controller
    {
        private readonly ShopContext _context;

        public ManufacturersController(ShopContext context)
        {
            _context = context;
        }

        // GET: Manufacturers
        public async Task<IActionResult> Index(int? id, int? instrumentID)
        {
            var viewModel = new ManufacturersIndexData();
            viewModel.Manufacturers = await _context.Manufacturers
            .Include(i => i.ManufacturedInstruments)
            .ThenInclude(i => i.Instrument)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.ManufacturersName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["ManufacturersID"] = id.Value;
                Manufacturers manufacturers = viewModel.Manufacturers.Where(
                i => i.ID == id.Value).Single();
                viewModel.Instruments = manufacturers.ManufacturedInstruments.Select(s => s.Instrument);
            }
            if (instrumentID != null)
            {
                ViewData["InstrumentID"] = instrumentID.Value;
                viewModel.Orders = viewModel.Instruments.Where(
                x => x.ID == instrumentID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Manufacturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturers = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (manufacturers == null)
            {
                return NotFound();
            }

            return View(manufacturers);
        }

        // GET: Manufacturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ManufacturersName,Adress")] Manufacturers manufacturers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manufacturers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturers);
        }

        // GET: Manufacturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manufacturer = await _context.Manufacturers
            .Include(i => i.ManufacturedInstruments).ThenInclude(i => i.Instrument)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            PopulateManufacturedInstrumentData (manufacturer);
            return View(manufacturer);

        }
        private void PopulateManufacturedInstrumentData(Manufacturers manufacturer)
        {
            var allInstruments = _context.Instruments;
            var manufactureInstrument = new HashSet<int>(manufacturer.ManufacturedInstruments.Select(c => c.InstrumentID));
            var viewModel = new List<ManufacturedInstrumentData>();
            foreach (var instrument in allInstruments)
            {
                viewModel.Add(new ManufacturedInstrumentData
                {
                    InstrumentID = instrument.ID,
                    Type = instrument.Type,
                    IsManufactured = manufactureInstrument.Contains(instrument.ID)
                });
            }
            ViewData["Instruments"] = viewModel;
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedInstruments)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manufacturerToUpdate = await _context.Manufacturers
            .Include(i => i.ManufacturedInstruments)
            .ThenInclude(i => i.Instrument)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Manufacturers>(
            manufacturerToUpdate,
            "",
            i => i.ManufacturersName, i => i.Adress))
            {
                UpdateManufacturedInstruments(selectedInstruments, manufacturerToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateManufacturedInstruments(selectedInstruments, manufacturerToUpdate);
            PopulateManufacturedInstrumentData(manufacturerToUpdate);
            return View(manufacturerToUpdate);
        }
        private void UpdateManufacturedInstruments(string[] selectedInstruments, Manufacturers manufacturerToUpdate)
        {
            if (selectedInstruments == null)
            {
                manufacturerToUpdate.ManufacturedInstruments = new List<ManufacturedInstruments>();
                return;
            }
            var selectedIntrumentsHS = new HashSet<string>(selectedInstruments);
            var manufacturedInstruments = new HashSet<int>
            (manufacturerToUpdate.ManufacturedInstruments.Select(c => c.Instrument.ID));
            foreach (var instrument in _context.Instruments)
            {
                if (selectedIntrumentsHS.Contains(instrument.ID.ToString()))
                {
                    if (!manufacturedInstruments.Contains(instrument.ID))
                    {
                        manufacturerToUpdate.ManufacturedInstruments.Add(new ManufacturedInstruments
                        {
                            ManufacturersID = manufacturerToUpdate.ID,
                            InstrumentID = instrument.ID
                        });
                    }
                }
                else
                {
                    if (manufacturedInstruments.Contains(instrument.ID))
                    {
                        ManufacturedInstruments instrumentToRemove = manufacturerToUpdate.ManufacturedInstruments.FirstOrDefault(i
                       => i.InstrumentID == instrument.ID);
                        _context.Remove(instrumentToRemove);
                    }
                }
            }
        }

        // GET: Manufacturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturers = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (manufacturers == null)
            {
                return NotFound();
            }

            return View(manufacturers);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manufacturers = await _context.Manufacturers.FindAsync(id);
            _context.Manufacturers.Remove(manufacturers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManufacturersExists(int id)
        {
            return _context.Manufacturers.Any(e => e.ID == id);
        }
    }
}
