using Entities.StudentCourses;
using Entity.Course;
using Entity.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string StudentId { get; set; }
        public Department Department { get; set; }
        public int Bill { get; set; }
        public bool Paid { get; set; }
        public int salary { get; set; }
        public int Phone { get; set; }
        public string Role { get; set; }
        public DateTime BirthDate { get; set; }
        public City City { get; set; }
        public Districts District { get; set; }
        public ICollection<Courses> Courses { get; set; } 
    }
}
