using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TripIt2Gether.Data;
using TripIt2Gether.Models;
using TripIt2Gether.ViewModels;

namespace TripIt2Gether.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index(ParticipationStatus? participationStatus, string TripId)
        {
            IQueryable<Application> queryable = null;
            if (TripId != null && TripId.Length > 0)
            {
                queryable =  _context.Applications.Where(s => s.TripId == TripId);
            }
            if (participationStatus.HasValue) {
                queryable = queryable == null ? _context.Applications.Where(s => s.Status == participationStatus) : queryable.Where(s => s.Status == participationStatus);
            }
            var applicationDbContext = queryable==null ? _context.Applications.Include(a => a.Participant).Include(a => a.Trip) : queryable.Include(a => a.Participant).Include(a => a.Trip);
            List<SelectListItem> items = new SelectList(_context.Trips, "TripNumber", "Name").ToList();
            items.Insert(0, new SelectListItem { Text = "All Trips", Value = "" });
            ViewBag.Trips = items;
            @ViewData["SelectedTrip"] = TripId;
            return View(await applicationDbContext.OrderByDescending(s=> s.AddedDate).ToListAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Participant)
                .Include(a => a.Trip)
                .FirstOrDefaultAsync(m => m.TripId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        public async Task<IActionResult> ShowPreviewData(string id)
        {
            Trip trip = await _context.Trips.FirstOrDefaultAsync(x => x.TripNumber == id);
            ApplicationUser appUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            ApplicationPreviewDataViewModel previewDataViewModel = GetApplicationpreviewModel(appUser);
            previewDataViewModel.TripNumber = id;
            ViewBag.TripName = trip.Name;
            ViewBag.StartDate = trip.StartDate;
            ViewBag.EndDate = trip.EndDate;
            ViewBag.Description = trip.Description;
            ViewBag.Cost = trip.Cost;
            ViewBag.Image = trip.Image;
            return View(previewDataViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowPreviewDataAsync([Bind("userId,TripNumber,Name,Surname,Address,DateOfBirth,Email,PhoneNumber")] ApplicationPreviewDataViewModel previewDataViewModel)
        {
            Trip trip = await _context.Trips.FirstOrDefaultAsync(x => x.TripNumber == previewDataViewModel.TripNumber);
            ApplicationUser appUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create), new { id = previewDataViewModel.TripNumber});
            }
           
            ViewBag.TripName = trip.Name;
            ViewBag.StartDate = trip.StartDate;
            ViewBag.EndDate = trip.EndDate;
            ViewBag.Description = trip.Description;
            ViewBag.Cost = trip.Cost;
            ViewBag.Image = trip.Image;
            return View(previewDataViewModel);
        }


        // GET: Applications/Create
        public async Task<IActionResult> Create(string id)
        {
            Trip trip = await _context.Trips.Include(f => f.Form).Include(f => f.Form.Questions).FirstOrDefaultAsync(x => x.TripNumber == id);
            ApplicationUser appUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            Application application = new Application();
            application.TripId = id;
            application.Trip = trip;
            application.Participant = appUser;
            application.UserId = appUser.Id;
            return View(application);
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,UserId,AdditionInformation,IsPaid,Price,AddedDate,Status")] Application application)
        {
            if(_context.Applications.Find(new object[] { application.TripId, application.UserId }) != null)
            {
                ViewData["ErrorMessage"] = "Już jesteś zapisany na tą wycieczkę";
                return View(application);
            }
            application.Answers = new List<Answer>();
            application.Trip = await _context.Trips.Include(f => f.Form).ThenInclude(f => f != null ? f.Questions : null).FirstOrDefaultAsync(x => x.TripNumber == application.TripId);
            application.Participant = await _context.Users.FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var answears = Request.Form;
            if(application.Trip.Form != null) { 
                foreach(var questionId in application.Trip.Form.Questions.Select(q => q.QuestionId))
                {
                    var answear = answears[$"question[{questionId}]"];
                    application.Answers.Add(new Answer { ParticipantId = application.UserId, Content = answear, QuestionId = questionId, TripNumber = application.TripId});
                }
            }
            bool incorrectAnsw = false;

            foreach(var answ in application.Answers)
            {
                if (!ValidAnswear(answ))
                {
                    incorrectAnsw = true;
                }
            }

            application.AddedDate = DateTime.Now;

                if (ModelState.IsValid && !incorrectAnsw)
                {
                    _context.Add(application);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), "Trips");
                }
            return View(application);
        }

        // GET: Applications/Edit/5
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> Edit(string tripId, string userId)
        {
            if (tripId == null || userId == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.Where(s => s.TripId == tripId && s.UserId == userId).Include(s => s.Trip).ThenInclude(s => s.Applications).Include(s => s.Participant).Include(s => s.Answers).ThenInclude(s => s.Question).FirstAsync();
            if (application == null)
            {
                return NotFound();
            }
            if (application.Trip.Applications.Where(s => s.Status == ParticipationStatus.Zaakaceptowana).ToList().Count >= application.Trip.MaxNumberOfParticipants)
            {
                TempData["ErrorMessage"] = $"Osiągnięto maksymalną liczę uczestników: {application.Trip.MaxNumberOfParticipants}";
            }
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> Edit(string TripId, string UserId, ParticipationStatus participationStatus, bool? IsVerified)
        { 
            if (TripId == null || UserId == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.Where(s => s.TripId == TripId && s.UserId == UserId).Include(s => s.Trip).Include(s => s.Participant).Include(s => s.Answers).ThenInclude(s => s.Question).FirstAsync();


            if (IsVerified != null)
            {
               // bool IsVerifiedB = IsVerified.Equals("on") ? true : false;
                if (application.Participant.IsVerified != IsVerified.Value)
                {
                    application.Participant.IsVerified = IsVerified.Value;
                    try
                    {
                        _context.Update(application.Participant);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
            }
            if (application.Trip.Applications.Where(s => s.Status == ParticipationStatus.Zaakaceptowana).ToList().Count >= application.Trip.MaxNumberOfParticipants)
            {
                TempData["ErrorMessage"] = $"Osiągnięto maksymalną liczę uczestników: {application.Trip.MaxNumberOfParticipants}";
                return View(nameof(Edit), application);
            }

            application.Status = participationStatus;

            if (!application.Participant.IsVerified && application.Status == ParticipationStatus.Zaakaceptowana)
            {
                TempData["UnverifyUser"] = "Użytkownik nie został Zweryfikowany";
                return View(nameof(Edit), application);
            }

            if (validApplicationModel(application))
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.TripId))
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

            return View(nameof(Edit), application);
        }

        // GET: Applications/Delete/5
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Participant)
                .Include(a => a.Trip)
                .FirstOrDefaultAsync(m => m.TripId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TourOperator")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var application = await _context.Applications.FindAsync(id);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(string id)
        {
            return _context.Applications.Any(e => e.TripId == id);
        }

        private ApplicationPreviewDataViewModel GetApplicationpreviewModel (ApplicationUser appUser)
        {
            ApplicationPreviewDataViewModel previewDataViewModel = new ApplicationPreviewDataViewModel();
            previewDataViewModel.userId = appUser.Id;
            previewDataViewModel.Name = appUser.Name;
            previewDataViewModel.Surname = appUser.Surname;
            previewDataViewModel.Email = appUser.Email;
            previewDataViewModel.Address = appUser.Address;
            previewDataViewModel.DateOfBirth = appUser.DateOfBirth;
            previewDataViewModel.PhoneNumber = appUser.PhoneNumber;
            return previewDataViewModel;
        }

        private bool validPreviewModel(ApplicationPreviewDataViewModel applicationPreviewDataViewModel)
        {
            var context = new ValidationContext(applicationPreviewDataViewModel, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(applicationPreviewDataViewModel, context, validationResults, true);
        }

        private bool validApplicationModel(Application application)
        {
            var context = new ValidationContext(application, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(application, context, validationResults, true);
        }

        private bool ValidAnswear(Answer answer)
        {
            var context = new ValidationContext(answer, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(answer, context, validationResults, true);
        }
    }
}
