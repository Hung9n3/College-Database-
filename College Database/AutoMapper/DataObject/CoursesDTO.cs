using College_Database.AutoMapper.DataObject.Get;
using Entity.BaseModel;
using Entity.Course;
using Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject
{
    public class CoursesDTO
    {
        public int CoursesId { get; set; }
        public string CoursesName { get; set; }
        public Department Department { get; set; }
        public TeacherGetDTO Lecturer { get; set; }
        public ICollection<Student> Students { get; set; }
        public int seat { get; set; }
        public int AvailableSeat {get;set;}
    }
}
