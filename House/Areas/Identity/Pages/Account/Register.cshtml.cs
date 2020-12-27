using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using House.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using House.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using House.Models;

namespace House.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        //Toevoegen Selectlist (nog eens te overlopen, kan volgens mij simpeler)
        //https://stackoverflow.com/questions/55190931/asp-net-core-identity-model-customization-select-list
        private readonly HouseContext _context;

        public RegisterModel(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            HouseContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IEnumerable<SelectListItem> Professions { get; set;  }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            List<Profession> availableProfessions = _context.Profession.ToList();
            Professions = availableProfessions.Select(x => new SelectListItem() { Text = x.Description, Value = x.ProfessionID.ToString() }).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            List<Profession> availableProfessions = _context.Profession.ToList();
            Professions = availableProfessions.Select(x => new SelectListItem() { Text = x.Description, Value = x.ProfessionID.ToString() }).ToList();
            if (ModelState.IsValid)
            {
                var user = new CustomUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Customer = new Customer
                    {
                        Firstname = Input.Firstname,
                        Lastname = Input.Lastname,
                        Profession = _context.Profession.Where(x => x.ProfessionID == Input.ProfessionID).FirstOrDefault()
                    }
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                user.Customer.UserID = user.Id;

                await _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
