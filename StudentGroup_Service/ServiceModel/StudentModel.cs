using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGroup_Service.ServiceModel
{
    public class StudentModel
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? GroupId { get; set; }

        public virtual string? GroupName { get; set; }
    }
}
