using Entity.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.User
{
    public class ApplicationUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int Phone { get; set; }
        public Address Address { get;set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; }
        public int DepartmentId { get; set; }
        public string StudentId { get; set; }
    }
}