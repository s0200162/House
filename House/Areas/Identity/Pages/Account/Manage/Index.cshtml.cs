using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using House.Areas.Identity.Data;
using House.Data;
using House.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace House.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly HouseContext _context;

        public IndexModel(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            HouseContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IEnumerable<SelectListItem> Professions { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Firstname")]
            public string Firstname { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Lastname")]
            public string Lastname { get; set; }

            public int ProfessionID { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(CustomUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Customer customer = await _context.Customer.FirstOrDefaultAsync(x => x.UserID == user.Id);
            user.Customer = customer;

            Input = new InputModel
            {
                Firstname = user.Customer.Firstname,
                Lastname = user.Customer.Lastname,
                ProfessionID = user.Customer.ProfessionID,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            List<Profession> availableProfessions = _context.Profession.ToList();
            Professions = availableProfessions.Select(x => new SelectListItem() { Text = x.Description, Value = x.ProfessionID.ToString() }).ToList();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            List<Profession> availableProfessions = _context.Profession.ToList();
            Professions = availableProfessions.Select(x => new SelectListItem() { Text = x.Description, Value = x.ProfessionID.ToString() }).ToList();
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

            Customer customer = await _context.Customer.FirstOrDefaultAsync(x => x.UserID == user.Id);
            user.Customer = customer;

            user.Customer.Firstname = Input.Firstname;
            user.Customer.Lastname = Input.Lastname;
            user.Customer.Profession = _context.Profession.Where(x => x.ProfessionID == Input.ProfessionID).FirstOrDefault();

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
