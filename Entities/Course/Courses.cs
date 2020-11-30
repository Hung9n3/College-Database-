
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Entities.StudentCourses;
using Entity.BaseModel;
using Entity.User;

namespace Entity.Course
{
    public class Courses 
    {
        [Key]
        public int CoursesId { get; set; }
        public string CoursesName { get; set; }
        public string Day { get; set; }
        public Department Department { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<StudentCourses> StudentCourses { get; set; }
        public int Size { get; set; }
        public int Rest { get; set; }
        public int Credits { get; set; }
        public string Room { get; set; }
        public int StartPeriod { get; set; }
        public int Periods { get; set; }
    }
}
