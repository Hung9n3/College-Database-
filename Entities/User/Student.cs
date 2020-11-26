using Entity.Course;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Entity.BaseModel;
using Entities.StudentCourses;

namespace Entity.User
{
    public class Student 
    {
        [Key]
        public int StudentID { get; set; }
        public string Id { get; set; }
        public int Bill { get; set; }
        public bool Paid { get; set; }
        public ICollection<StudentCourses> StudentCourses { get; set; }
        public UserModel UserModel { get; set; }
    }
}