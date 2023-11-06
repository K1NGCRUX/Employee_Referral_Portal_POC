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

namespace Data_Access_Layer
{
    public class AuthDataLayer : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AuthDataLayer(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ActionResult<User>> Register(UserDTO request)
        {
                // Data Validation: Check if the username already exists
                var userExists = _db.Users.Any(u => u.Username == request.Username);
            //throw new TimeoutException();
                if (userExists)
                {
                    // Return a bad request with a message indicating the username is already taken.
                    throw new ConflictException("User already exists");
                }

                // Create a new user
                var user = new User();

                // Create password hash and salt
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Set user properties
                user.Username = request.Username;
                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;
                user.Role = request.Role;

                // Save the user to the database
                _db.Add(user);
                _db.SaveChanges();

                return Ok(user);
        }

        public async Task<User> FindUserByUsernameAsync(UserDTO request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if(user == null)
            {
                throw new NotFoundException("User does not exist, please try again");
            }
            return user;
        }
    }
}
