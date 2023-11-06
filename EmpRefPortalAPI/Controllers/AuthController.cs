using Microsoft.AspNetCore.Mvc;
using Data_Access_Layer.Models;
using Data_Access_Layer.Models.DTO;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Data_Access_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Data_Access_Layer;
using Business_Logic_Layer.Exceptions;

namespace EmpRefPortalAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public static User user = new User();
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly AuthDataLayer _dal;

        public AuthController(ApplicationDbContext db, IConfiguration configuration, AuthDataLayer authDataLayer)
        {
            _db = db;
            _dal = authDataLayer;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public Task<ActionResult<User>> Register(UserDTO request)
        {
            return _dal.Register(request);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO request)
        {
            var user = await _dal.FindUserByUsernameAsync(request);

            if (!VerifyPaswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new BadRequestException("Password Incorrec, please try again");
            }
            
            string token = CreateToken(user);
            return Ok(token);
        }

        private bool VerifyPaswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
                };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims : claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        //Claims -> describes about the user, within a token
    }
}
