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
    public class WorkListsController : Controller
    {
        private readonly DBServices _context;

        public WorkListsController(DBServices context)
        {
            _context = context;
        }

        // GET: WorkLists
        public async Task<IActionResult> Index()
        {
            var dBServices = _context.WorkLists.Include(w => w.Issue).Include(w => w.WorkType).Include(w => w.Worker);
            return View(await dBServices.ToListAsync());
        }

        // GET: WorkLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkLists == null)
            {
                return NotFound();
            }

            var workList = await _context.WorkLists
                .Include(w => w.Issue)
                .Include(w => w.WorkType)
                .Include(w => w.Worker)
                .FirstOrDefaultAsync(m => m.WorkListId == id);
            if (workList == null)
            {
                return NotFound();
            }

            return View(workList);
        }

        // GET: WorkLists/Create
        public IActionResult Create()
        {
            ViewData["IssueId"] = new SelectList(_context.ReportedIssues, "IssueId", "Address");
            ViewData["WorkTypeId"] = new SelectList(_context.WorkTypes, "WorkTypeId", "WorkTypeDescription");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "FullName");
            return View();
        }

        // POST: WorkLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkListId,IssueId,WorkTypeId,WorkerId,FixingDateTime")] WorkList workList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IssueId"] = new SelectList(_context.ReportedIssues, "IssueId", "Address", workList.IssueId);
            ViewData["WorkTypeId"] = new SelectList(_context.WorkTypes, "WorkTypeId", "WorkTypeDescription", workList.WorkTypeId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "FullName", workList.WorkerId);
            return View(workList);
        }

        // GET: WorkLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkLists == null)
            {
                return NotFound();
            }

            var workList = await _context.WorkLists.FindAsync(id);
            if (workList == null)
            {
                return NotFound();
            }
            ViewData["IssueId"] = new SelectList(_context.ReportedIssues, "IssueId", "Address", workList.IssueId);
            ViewData["WorkTypeId"] = new SelectList(_context.WorkTypes, "WorkTypeId", "WorkTypeDescription", workList.WorkTypeId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "FullName", workList.WorkerId);
            return View(workList);
        }

        // POST: WorkLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkListId,IssueId,WorkTypeId,WorkerId,FixingDateTime")] WorkList workList)
        {
            if (id != workList.WorkListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkListExists(workList.WorkListId))
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
            ViewData["IssueId"] = new SelectList(_context.ReportedIssues, "IssueId", "Address", workList.IssueId);
            ViewData["WorkTypeId"] = new SelectList(_context.WorkTypes, "WorkTypeId", "WorkTypeDescription", workList.WorkTypeId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "FullName", workList.WorkerId);
            return View(workList);
        }

        // GET: WorkLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkLists == null)
            {
                return NotFound();
            }

            var workList = await _context.WorkLists
                .Include(w => w.Issue)
                .Include(w => w.WorkType)
                .Include(w => w.Worker)
                .FirstOrDefaultAsync(m => m.WorkListId == id);
            if (workList == null)
            {
                return NotFound();
            }

            return View(workList);
        }

        // POST: WorkLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WorkLists == null)
            {
                return Problem("Entity set 'DBServices.WorkLists'  is null.");
            }
            var workList = await _context.WorkLists.FindAsync(id);
            if (workList != null)
            {
                _context.WorkLists.Remove(workList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkListExists(int id)
        {
          return (_context.WorkLists?.Any(e => e.WorkListId == id)).GetValueOrDefault();
        }
    }
}
