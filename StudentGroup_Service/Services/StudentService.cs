using StudentGroup_Repository.Models;
using StudentGroup_Repository.Repositories;
using StudentGroup_Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentGroup_Service.Services
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;
        private readonly StudentGroupRepository _studentGroupRepository;

        public StudentService()
        {
            _studentRepository = new StudentRepository();
            _studentGroupRepository = new StudentGroupRepository();
        }

        public List<StudentModel> GetAllStudents()
        {
            var students = _studentRepository.GetAllStudents();

            List<StudentModel> studentModels = students.Select(s => new StudentModel
            {
                Id = s.Id,
                FullName = s.FullName,
                DateOfBirth = s.DateOfBirth,
                Email = s.Email,
                GroupId  = s.GroupId,   
                GroupName = s.Group.GroupName,
            }).ToList();

            return studentModels;
        }

        public StudentModel GetStudentsById(int Id)
        {
            var student = _studentRepository.GetStudentsById(Id);

            if (student == null)
            {
                throw new Exception("Hoc sinh không tồn tại");
            }

            var Group = _studentGroupRepository.FindById((int)student.GroupId);

            if(Group == null)
            {
                throw new Exception("Nhóm học sinh không tồn tại");
            }

            StudentModel studentModel = new StudentModel()
            {
                Id = student.Id,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                GroupId = student.GroupId,
                GroupName = Group.GroupName,
            };

            return studentModel;    
        }

        public List<StudentModel> SearchDateOfBird(string yearStart, string yearEnd)
        {
            #region Check Year
            string pattern = @"^\d{4}$";
            Regex regex = new Regex(pattern);

            int start = 0;
            int end = 0;

            if (string.IsNullOrEmpty(yearStart) || string.IsNullOrEmpty(yearEnd))
            {
                throw new Exception("Missing input Year Start or End");
            }

            if (!regex.IsMatch(yearStart))
            {
                throw new Exception("Wrong year Format");
            }
            else
            {
                start = int.Parse(yearStart);
            }

            if (!regex.IsMatch(yearEnd))
            {
                throw new Exception("Wrong year Format");
            }
            else
            {
                end = int.Parse(yearEnd);
            }

            if (start - end > 0)
            {
                throw new Exception("Năm bắt đầu không thể lớn hơn Năm kết thúc");
            }
            #endregion


            var students = _studentRepository.SearchDateOfBirthStudents(start, end);

            if(!students.Any())
            {
                return new List<StudentModel>();
            }

            List<StudentModel> studentModels = students.Select(s => new StudentModel
            {
                Id = s.Id,
                FullName = s.FullName,
                DateOfBirth = s.DateOfBirth,
                Email = s.Email,
                GroupId = s.GroupId,
                GroupName = s.Group.GroupName,
            }).ToList();

            return studentModels;
        }

        public bool CheckValidStudent(StudentModel student, string dateOfBirth)
        {
            #region Email
            if (string.IsNullOrEmpty(student.Email))
            {
                throw new Exception("Email field is required");
            }
            else
            {
                if (IsValidEmail(student.Email) == false)
                {
                    throw new Exception("Email wrong formmat");
                }
            }
            #endregion

            #region FullName
            if (string.IsNullOrEmpty(student.FullName))
            {
                throw new Exception("FullName field is required");
            }
            else
            {
                if(student.FullName.Length < 5 || student.FullName.Length > 40) 
                {
                    throw new Exception("Length of Student FullName is in the range of 5 – 40 characters");
                }
                else
                {
                    string[] name = student.FullName.Split(" ");

                    foreach (var item in name)
                    {
                        if(char.IsUpper(item[0]) == false)
                        {
                            throw new Exception("Each word must begin with the capital letter");
                        }

                        for (int i = 0; i < item.Length; i++)
                        {
                            if (char.IsSymbol(item[i]) == true)
                            {
                                throw new Exception("Special characters are not allowed");
                            }
                        }
                    } 
                }
            }
            #endregion

            #region Date Of Birth

            if(string.IsNullOrEmpty(dateOfBirth))
            {
                throw new Exception("Date of Birth field is required");
            }
            else
            {
                if (!DateTime.TryParseExact(dateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                {
                    throw new ArgumentException("Date of Birth must be a valid date in format dd/MM/yyyy.");
                }

                // Kiểm tra ngày sinh phải trước ngày 1/1/2005
                if (dob >= new DateTime(2005, 1, 1))
                {
                    throw new ArgumentException("Date of Birth must be before 1/1/2005.");
                }
            }
            #endregion

            return true;
        }

        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var address = new MailAddress(email);
                return address.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public bool AddStudent(StudentModel student) 
        {

            Student add = new Student()
            {
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                GroupId = _studentGroupRepository.FindByGroupName(student.GroupName).Id
            };

            return _studentRepository.AddStudent(add);
        }

        public bool UpdateStudent(StudentModel student)
        {
            var studentPlace = _studentRepository.GetStudentsById(student.Id);
            
            if (studentPlace == null)
            {
                throw new Exception("Học sinh không tồn tại");
            }

            var groupModify = _studentGroupRepository.FindByGroupName(student.GroupName);

            if (groupModify != null)
            {

                studentPlace.FullName = student.FullName;
                studentPlace.DateOfBirth = student.DateOfBirth;
                studentPlace.Email = student.Email;
                studentPlace.GroupId = groupModify.Id;
            }
            return _studentRepository.UpdateStudent(studentPlace);
        }

        public bool DeleteStudent(int Id)
        {
            var student = _studentRepository.GetStudentsById(Id);

            if(student == null)
            {
                throw new Exception("Học sinh không tồn tại");
            }

            return _studentRepository.DeleteStudent(student);
        }
    }
}
