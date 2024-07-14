using StudentGroup_Repository.Models;
using StudentGroup_Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGroup_Service.Services
{
    public class UserRoleService
    {
        private readonly UserRoleRepository _userRoleRepository;

        public UserRoleService()
        {
            _userRoleRepository = new UserRoleRepository();
        }

        public UserRole Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Field Input are required");
            }

            var user = _userRoleRepository.GetUserRoleByUserName(username);

            if (user == null)
            {
                throw new Exception("Invalid Username or Password!");
            }

            if (user.Passphrase != password)
            {
                throw new Exception("Invalid Username or Password!");
            }

            return user;
        }

        public bool CheckRoleUser(UserRole user)
        {
            if (user != null) 
            { 
                if(user.UserRole1 == 2)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
