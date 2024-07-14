using StudentGroup_Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGroup_Repository.Repositories
{
    public class UserRoleRepository
    {
        private readonly Prn231Su23StudentGroupDbContext _context;

        public UserRoleRepository()
        {
            _context = new Prn231Su23StudentGroupDbContext();
        }

        public UserRole? GetUserRoleByUserName(string userName)
        {
            return _context.UserRoles.SingleOrDefault(x => x.Username == userName);
        }
    }
}
