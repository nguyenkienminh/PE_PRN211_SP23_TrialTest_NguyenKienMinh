using StudentGroup_Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGroup_Repository.Repositories
{
    public class StudentGroupRepository
    {
        private readonly Prn231Su23StudentGroupDbContext _context;

        public StudentGroupRepository()
        {
            _context = new Prn231Su23StudentGroupDbContext();
        }

        public List<StudentGroup> GetAll() 
        {
           return _context.StudentGroups.ToList();
        }

        public StudentGroup FindByGroupName(string groupName) 
        {
            return _context.StudentGroups.SingleOrDefault(s => s.GroupName == groupName);
        }

        public StudentGroup FindById(int Id)
        {
            return _context.StudentGroups.SingleOrDefault(s => s.Id == Id);
        }
    }
}
