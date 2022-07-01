using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TripIt2Gether.CustomValidation;
using TripIt2Gether.Models;

namespace TripIt2Gether.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth {get; set;}

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string TripId { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            [DateBeforeToday]
            public DateTime DateOfBirth { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync(string TripId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            if (TripId != null && TripId.Length > 0)
            {
                TempData["TripId"] = TripId;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            if(Input.Surname != user.Surname)
            {
                user.Surname = Input.Surname;
            }

            if(Input.Address != user.Address)
            {
                user.Address = Input.Address;
            }

            if(Input.DateOfBirth != user.DateOfBirth){
                user.DateOfBirth = Input.DateOfBirth;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
