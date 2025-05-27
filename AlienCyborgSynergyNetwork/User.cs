using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    internal class User
    {
        [Key]
        private Guid Guid { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private int Age { get; set; }

        public User(string Email, string Password, string FirstName, string LastName, int Age)
        {
            this.Guid = Guid.NewGuid();
            this.Email = Email;
            this.Password = Password;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Age = Age;

        }
    }
}
