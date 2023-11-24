using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Supervisor_Logic.Models
{
    public class UserModel : IUserModel
    {
        private string _firstName;
        private string _lastName;
        private string _email;

        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
        public string Email { get { return _email; } }

        public UserModel(string firstName, string lastName, string email)
        {
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
        }
    }
}
