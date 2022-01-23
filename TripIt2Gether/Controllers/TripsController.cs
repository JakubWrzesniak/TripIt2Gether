using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TripIt2Gether.Data;

namespace TripIt2Gether.Models
{
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public TripsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Trips.Include(t => t.Form).Include(s => s.Applications).ThenInclude(s => s != null ? s.Participant : null).Include(s => s.TourOperators);
            if (!User.IsInRole("TourOperator"))
            {
                return View(await applicationDbContext.Where(s => s.Status == TripStatus.Aktywna).ToListAsync());
            }
            return View(await applicationDbContext.Where(s => s.TourOperators.Select(s => s.OperatorId).Contains(User.FindFirstValue(ClaimTypes.NameIdentifier))).ToListAsync());
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Form)
                .FirstOrDefaultAsync(m => m.TripNumber == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // GET: Trips/Create
        [Authorize(Roles = "TourOperator")]
        public IActionResult Create()
        {
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "TripId");
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(TripStatus)));
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> Create([Bind("TripNumber,Name,StartDate,EndDate,MaxNumberOfParticipants,Cost,Status,IFromImage,FormId,Description")] Trip trip)
        {
            trip.TourOperators = new List<TourOperator>();
            trip.TourOperators.Add(new TourOperator { OperatorId = User.FindFirstValue(ClaimTypes.NameIdentifier), TripId = trip.TripNumber });
            if (ModelState.IsValid)
            {
                trip.Image = UploadedFile(trip.IFromImage);
                _context.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
        }

        // GET: Trips/Edit/5
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "TripId", trip.FormId);
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> Edit(string id, [Bind("TripNumber,Name,StartDate,EndDate,MaxNumberOfParticipants,Status,Cost,Image,FormId,Description")] Trip trip)
        {
            if (id != trip.TripNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.TripNumber))
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
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "TripId", trip.FormId);
            return View(trip);
        }

        // GET: Trips/Delete/5
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Form)
                .FirstOrDefaultAsync(m => m.TripNumber == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trip = await _context.Trips.FindAsync(id);
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(string id)
        {
            return _context.Trips.Any(e => e.TripNumber == id);
        }

        private string UploadedFile(IFormFile IFromImage)
        {
            string uniqueFileName = null;

            if (IFromImage != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString()+".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    IFromImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
