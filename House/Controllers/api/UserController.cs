using House.Areas.Identity.Data;
using House.Data;
using House.Entities;
using House.Helpers;
using House.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace House.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly HouseContext _context;

        public UserController(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            IOptions<AppSettings> appSettings,
            HouseContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _context = context;
        }


        [HttpPost("authenticate")]
        public async Task<object> Authenticate([FromBody] ApiUser apiUser)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(
                apiUser.Username, apiUser.Password, false, false);

            if (signInResult.Succeeded)
            {
                CustomUser customUser = _userManager.Users.SingleOrDefault(r => r.Email == apiUser.Username);
                apiUser.Token = GenerateJwtToken(apiUser.Username, customUser).ToString();
                Customer customer = _context.Customer.SingleOrDefault(x => x.CustomUser.UserName == apiUser.Username);
                apiUser.CustomerID = customer.CustomerID;

                return apiUser;
            }

            return BadRequest(new { message = "Username or password is incorrect" });
        }

        private string GenerateJwtToken(string email, CustomUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}