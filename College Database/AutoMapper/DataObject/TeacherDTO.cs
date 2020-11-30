using Entity.Course;
using Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject
{
    public class TeacherDTO
    {
        public int TeacherId { get; set; }
        public bool IsHead { get; set; } = false;
        public int Salary { get; set; }
        public List<CoursesShow> Courses { get; set; }
        public UserDTO UserModel { get; set; }
    }
}
