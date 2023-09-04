/**
 *  Author:           Katona Tamás
 *  E-mail:           katonatomi@msn.com
 *  Course:           CUBIX - C# és .NET fejlesztés alapok, 2023.June - Sept
 *  Project Name:     MINDIG FÉNYES KFT, Company's Working and Issue Management Software
 *  Project Github:   https://github.com/tamaskatona89/AlwaysLightsOnManagement
 *  Project Duration: 2023.08.23....2023.09.06
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlwaysLightsOnManagement.Controllers
{
    public class ReportedIssuesController : Controller
    {
        private readonly DBServices _context;

        public ReportedIssuesController(DBServices context)
        {
            _context = context;
        }

        // GET: ReportedIssues
        public async Task<IActionResult> Index()
        {
              return _context.ReportedIssues != null ? 
                          View(await _context.ReportedIssues.ToListAsync()) :
                          Problem("Entity set 'DBServices.ReportedIssues'  is null.");
        }

        // GET: ReportedIssues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReportedIssues == null)
            {
                return NotFound();
            }

            var reportedIssue = await _context.ReportedIssues
                .FirstOrDefaultAsync(m => m.IssueId == id);
            if (reportedIssue == null)
            {
                return NotFound();
            }

            return View(reportedIssue);
        }

        // GET: ReportedIssues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReportedIssues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IssueId,ZipCode,Address,ReportedDateTime,IsFixed")] ReportedIssue reportedIssue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportedIssue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reportedIssue);
        }

        // GET: ReportedIssues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReportedIssues == null)
            {
                return NotFound();
            }

            var reportedIssue = await _context.ReportedIssues.FindAsync(id);
            if (reportedIssue == null)
            {
                return NotFound();
            }
            return View(reportedIssue);
        }

        // POST: ReportedIssues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueId,ZipCode,Address,ReportedDateTime,IsFixed")] ReportedIssue reportedIssue)
        {
            if (id != reportedIssue.IssueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportedIssue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportedIssueExists(reportedIssue.IssueId))
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
            return View(reportedIssue);
        }

        // GET: ReportedIssues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReportedIssues == null)
            {
                return NotFound();
            }

            var reportedIssue = await _context.ReportedIssues
                .FirstOrDefaultAsync(m => m.IssueId == id);
            if (reportedIssue == null)
            {
                return NotFound();
            }

            return View(reportedIssue);
        }

        // POST: ReportedIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReportedIssues == null)
            {
                return Problem("Entity set 'DBServices.ReportedIssues'  is null.");
            }
            var reportedIssue = await _context.ReportedIssues.FindAsync(id);
            if (reportedIssue != null)
            {
                _context.ReportedIssues.Remove(reportedIssue);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportedIssueExists(int id)
        {
          return (_context.ReportedIssues?.Any(e => e.IssueId == id)).GetValueOrDefault();
        }
    }
}
