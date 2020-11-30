﻿using Entities.StudentCourses;
using Entity.Course;
using Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject.Post
{
    public class CoursesPostDTO
    {
        public int CoursesId { get; set; }
        public string CoursesName { get; set; }
        public Department Department { get; set; }
        public int TeacherId { get; set; }
        public int DepartmentId { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<StudentCourses> StudentCourses { get; set; }
        public int seat { get; set; }
        public int AvailableSeat { get; set; }
        public int Credits { get; set; }
        public int Periods { get; set; }
    }
}
