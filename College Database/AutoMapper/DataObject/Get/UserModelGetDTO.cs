using Entity.Course;
using Entity.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject.Get
{
    public class UserModelGetDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }          
        public int Phone { get; set; }
        public string Role { get; set; }
        public DateTime BirthDate { get; set; }
        public AddressGetDTO Address { get; set; }
        public ICollection<Courses> Courses { get; set; }
    }
}
