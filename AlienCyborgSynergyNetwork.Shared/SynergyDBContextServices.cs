using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    public class SynergyDBContextServices
    {
        private readonly SynergyDBContext _context;

        public SynergyDBContextServices(SynergyDBContext context)
        {
            _context = context;
        }

        public async Task<string?> RegisterUserAsync(RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return "Passwords do not match.";
            }

            var newUser = new User
            {
                Email = model.Email,
                Password = PasswordHasher.Hash(model.Password),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return null;

            }
            catch (Exception ex)
            {
                return "Please contact an administrator. Error: " + ex.Message;

            }
        }
    }
}
