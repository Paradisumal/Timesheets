using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RebootIT.TimesheetApp.Data;

namespace RebootIT.TimesheetApp.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly TimesheetDbContext _context;

        public TimesheetController(TimesheetDbContext context)
        {
            _context = context;
        }

        // GET: Timesheet
        public async Task<IActionResult> Index()
        {
            var timesheetDbContext = _context.Timesheets.Include(t => t.Client).Include(t => t.Location).Include(t => t.Staff);
            return View(await timesheetDbContext.ToListAsync());
        }

        // GET: Timesheet (filtered by StaffId)
        public async Task<IActionResult> StaffFilteredIndex(int id)
        {
            var timesheetDbContext = _context.Timesheets.Include(t => t.Client).Include(t => t.Location).Include(t => t.Staff).Where(t => t.StaffId == id);

            var indexVm = new RebootIT.TimesheetApp.ViewModels.TimeSheets.Index(
                await timesheetDbContext.ToListAsync(),
                0,
                0,
                id);

            return View("Index", indexVm);
        }

        // GET: Timesheet (filtered by ClientId)
        public async Task<IActionResult> ClientFilteredIndex(int? id)
        {
            var timesheetDbContext = _context.Timesheets.Include(t => t.Client).Include(t => t.Location).Include(t => t.Staff).Where(t => t.ClientId == id);
            return View("Index", await timesheetDbContext.ToListAsync());
        }

        // GET: Timesheet (filtered by LocationId)
        public async Task<IActionResult> LocationFilteredIndex(int? id)
        {
            var timesheetDbContext = _context.Timesheets.Include(t => t.Client).Include(t => t.Location).Include(t => t.Staff).Where(t => t.LocationId == id);
            return View("Index", await timesheetDbContext.ToListAsync());
        }

        // GET: Timesheet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timesheet = await _context.Timesheets
                .Include(t => t.Client)
                .Include(t => t.Location)
                .Include(t => t.Staff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timesheet == null)
            {
                return NotFound();
            }

            return View(timesheet);
        }

        // GET: Timesheet/Create
        public IActionResult Create(int? staffId)
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "CompanyName");
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");

            var timesheet = new Timesheet() {
                StaffId = staffId.HasValue ? staffId.Value : 0
            };

            return View();
        }

        // POST: Timesheet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MinutesWorked,StaffId,ClientId,LocationId")] Timesheet timesheet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timesheet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "CompanyName", timesheet.ClientId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email", timesheet.StaffId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", timesheet.LocationId);
            return View(timesheet);
        }

        // GET: Timesheet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "CompanyName", timesheet.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address", timesheet.LocationId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Name", timesheet.StaffId);
            return View(timesheet);
        }

        // POST: Timesheet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MinutesWorked,StaffId,ClientId,LocationId")] Timesheet timesheet)
        {
            if (id != timesheet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timesheet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimesheetExists(timesheet.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "CompanyName", timesheet.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", timesheet.LocationId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email", timesheet.StaffId);
            return View(timesheet);
        }

        // GET: Timesheet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timesheet = await _context.Timesheets
                .Include(t => t.Client)
                .Include(t => t.Location)
                .Include(t => t.Staff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timesheet == null)
            {
                return NotFound();
            }

            return View(timesheet);
        }

        // POST: Timesheet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimesheetExists(int id)
        {
            return _context.Timesheets.Any(e => e.Id == id);
        }
    }
}
