using Microsoft.EntityFrameworkCore;
using StudentGroup_Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGroup_Repository.Repositories
{
    public class StudentRepository
    {
        private readonly Prn231Su23StudentGroupDbContext _context;

        public StudentRepository()
        {
            _context = new Prn231Su23StudentGroupDbContext();
        }

        public List<Student> GetAllStudents()
        {
            var item =  _context.Students.Include(s => s.Group).ToList();
            return item;
        }

        public List<Student> SearchDateOfBirthStudents(int start, int end)
        {
            var item = _context.Students.Include(s => s.Group).Where(s => s.DateOfBirth.HasValue && 
                                                                     s.DateOfBirth.Value.Year >= start && 
                                                                     s.DateOfBirth.Value.Year <= end)
                                                              .OrderBy(s => s.DateOfBirth.Value.Date)
                                                              .ToList();
            return item;
        }

        public bool AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return true;
        }

        public Student? GetStudentsById(int Id)
        {
            return _context.Students.SingleOrDefault(s => s.Id == Id);
        }

        public bool UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();
            return true;    
        }

        public bool DeleteStudent(Student student)
        {
            _context.Students.Remove(student);
            _context.SaveChanges();
            return true;
        }
    }
}
