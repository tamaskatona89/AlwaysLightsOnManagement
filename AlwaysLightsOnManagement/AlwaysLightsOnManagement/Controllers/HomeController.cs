/**
 *  Author:           Katona Tamás
 *  E-mail:           katonatomi@msn.com
 *  Course:           CUBIX - C# és .NET fejlesztés alapok, 2023.June - Sept
 *  Project Name:     MINDIG FÉNYES KFT, Company's Working and Issue Management Software
 *  Project Github:   https://github.com/tamaskatona89/AlwaysLightsOnManagement
 *  Project Duration: 2023.08.23....2023.09.06
 */
using AlwaysLightsOnManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlwaysLightsOnManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBServices _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = new DBServices();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET: ReportedIssues/Create
        public IActionResult Create()
        {
            ViewBag.SuccessMessage = " has been created successfully!";
            return View();
        }

        // POST: ReportedIssues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("IssueId,ZipCode,Address,ReportedDateTime,IsFixed")] ReportedIssue reportedIssue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportedIssue);

                if (await _context.SaveChangesAsync() > 0)
                {
                    //SAVING TO DB IS OK
                    ViewBag.SuccessMessage = string.Format("Sikeres bejelentés!");
                }
                
                //return RedirectToAction(nameof(Index));
                return View(reportedIssue);
            }
            return View(reportedIssue);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}