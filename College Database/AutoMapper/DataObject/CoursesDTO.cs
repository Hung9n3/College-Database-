using Entity.BaseModel;
using Entity.Course;
using Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject
{
    public class CoursesDTO : BaseModels
    {
        public Department Department { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; }
        public int seat { get; set; }
        public int AvailableSeat {get;set;}
    }
}
