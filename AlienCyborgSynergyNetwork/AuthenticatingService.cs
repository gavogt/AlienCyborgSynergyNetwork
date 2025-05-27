using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlienCyborgSynergyNetwork
{
    /// <summary>
    /// Service for authenticating users against the stored password hash
    /// </summary>
    internal class AuthenticatingService
    {
        private readonly SynergyDBContext _context;

        public AuthenticatingService(SynergyDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the authenticated user, or null if authentication fails.
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="password">The plaintext password</param>
        /// <returns></returns>
        public async Task<User?> ValidateCredentialsAsync(string email, string password)
        {
            // Check if the user exists with the provided email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            // If user is found and password matches, return the user
            if (user is not null && PasswordHasher.Verify(user.Password, password))
            {
                return user;
            }

            return null;
        }
    }
}
