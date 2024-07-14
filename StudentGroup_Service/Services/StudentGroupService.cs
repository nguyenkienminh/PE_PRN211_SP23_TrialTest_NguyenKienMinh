using StudentGroup_Repository.Models;
using StudentGroup_Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGroup_Service.Services
{
    public class StudentGroupService
    {
        private readonly StudentGroupRepository _studentGroupRepository;

        public StudentGroupService()
        {
            _studentGroupRepository = new StudentGroupRepository();
        }

        public List<StudentGroup> GetAll()
        {
            return _studentGroupRepository.GetAll().ToList();
        }
    }
}
