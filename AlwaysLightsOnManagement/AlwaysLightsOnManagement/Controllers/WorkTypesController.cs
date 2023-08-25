using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlwaysLightsOnManagement;

namespace AlwaysLightsOnManagement.Controllers
{
    public class WorkTypesController : Controller
    {
        private readonly DBServices _context;

        public WorkTypesController(DBServices context)
        {
            _context = context;
        }

        // GET: WorkTypes
        public async Task<IActionResult> Index()
        {
              return _context.WorkTypes != null ? 
                          View(await _context.WorkTypes.ToListAsync()) :
                          Problem("Entity set 'DBServices.WorkTypes'  is null.");
        }

        // GET: WorkTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkTypes == null)
            {
                return NotFound();
            }

            var workType = await _context.WorkTypes
                .FirstOrDefaultAsync(m => m.WorkTypeId == id);
            if (workType == null)
            {
                return NotFound();
            }

            return View(workType);
        }

        // GET: WorkTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkTypeId,WorkTypeDescription")] WorkType workType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workType);
        }

        // GET: WorkTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkTypes == null)
            {
                return NotFound();
            }

            var workType = await _context.WorkTypes.FindAsync(id);
            if (workType == null)
            {
                return NotFound();
            }
            return View(workType);
        }

        // POST: WorkTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkTypeId,WorkTypeDescription")] WorkType workType)
        {
            if (id != workType.WorkTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkTypeExists(workType.WorkTypeId))
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
            return View(workType);
        }

        // GET: WorkTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkTypes == null)
            {
                return NotFound();
            }

            var workType = await _context.WorkTypes
                .FirstOrDefaultAsync(m => m.WorkTypeId == id);
            if (workType == null)
            {
                return NotFound();
            }

            return View(workType);
        }

        // POST: WorkTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WorkTypes == null)
            {
                return Problem("Entity set 'DBServices.WorkTypes'  is null.");
            }
            var workType = await _context.WorkTypes.FindAsync(id);
            if (workType != null)
            {
                _context.WorkTypes.Remove(workType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkTypeExists(int id)
        {
          return (_context.WorkTypes?.Any(e => e.WorkTypeId == id)).GetValueOrDefault();
        }
    }
}
