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
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "BillingAddress");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address");
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email");
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "BillingAddress", timesheet.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address", timesheet.LocationId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email", timesheet.StaffId);
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "BillingAddress", timesheet.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address", timesheet.LocationId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email", timesheet.StaffId);
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "BillingAddress", timesheet.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address", timesheet.LocationId);
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
