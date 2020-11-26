using Entity.Course;
using Entity.Location;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.User
{
    public class UserModel : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Phone { get; set; }
        public Address Address { get; set; }
        [Required(ErrorMessage ="Lack of Role =.=")]
        public string Role { get; set; }
    }
}
