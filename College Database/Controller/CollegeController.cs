using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using College_Database.AutoMapper.DataObject;
using College_Database.AutoMapper.DataObject.Get;
using College_Database.AutoMapper.DataObject.Post;
using Contracts;
using Entities.User;
using Entity.Context;
using Entity.Course;
using Entity.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace College_Database.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        private UserManager<UserModel> _userManager;
        private SignInManager<UserModel> _signInManager;
        private readonly ApplicationSettings _appSettings;
        private RepoContext _repoContext;
        private IMapper _mapper;
        private IRepoTeacher _repoTeacher;
        private IRepoStudent _repoStudent;
        private IRepoCourses _repoCourses;
        private IRepoDepartment _repoDepartment;
        public CollegeController(IMapper mapper, IRepoStudent repoStudent, IRepoTeacher repoTeacher 
                                , RepoContext repoContext, UserManager<UserModel> userManager,
                                SignInManager<UserModel> signInManager, IOptions<ApplicationSettings> appSettings, IRepoCourses repoCourses,IRepoDepartment repoDepartment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _repoContext = repoContext;
            _repoCourses = repoCourses;
            _repoTeacher = repoTeacher;
            _repoStudent = repoStudent;
            _mapper = mapper;
            _repoDepartment = repoDepartment;
        }
       [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentPostDTO _department)
        {
            var department = _mapper.Map<Department>(_department);
            _repoContext.Add(department);
            await _repoContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddCourses(CoursesPostDTO _course)
        {
            var department = await _repoContext.Departments.FindAsync(_course.DepartmentId);
            var teacher = await _repoContext.Teachers.FindAsync(_course.TeacherId);
            _course.Department = department;
            _course.Teacher = teacher;
            _repoContext.Add(_mapper.Map<Courses>(_course));
            await _repoContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<List<CoursesDTO>> GetAllCourses()
        {
            var items = await _repoCourses.FindAll();
            List<CoursesDTO> _courses = new List<CoursesDTO>(); 
            foreach(Courses c in items)
            {
                _courses.Add(_mapper.Map<CoursesDTO>(c));
            }
            return _courses;
        }
        [HttpGet("{id}")]
        public async Task<CoursesDTO> GetCoursesById(int id)
        {
            var course = await _repoContext.Courses.Include(x => x.Teacher).Include(x => x.StudentCourses).ThenInclude(x => x.Student).Include(x => x.Department)
                .FirstAsync();
            var _course = _mapper.Map<CoursesDTO>(course);
            return _course;
        }
        [HttpGet]
        public async Task<List<Department>> GetDepartments()
        {
            var result = await _repoDepartment.FindAll();
            return result;
        }

        
        [HttpPost("{id}")]
        public async Task<IActionResult> TeacherCourses(List<int> listCourses, int id)
        {
            var teacher = await _repoTeacher.FindByIdAsync(id);
            foreach(int i in listCourses)
            {
                var course = await _repoContext.Courses.FindAsync(i);
                teacher.Courses.Add(course);
            }
            _repoTeacher.Update(teacher);
            await _repoTeacher.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{cityid}/{districtid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserInfo(ApplicationUserModel applicationUserModel, int cityid,int districtid)
        {

            var userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _repoContext.UserModel
                .FirstOrDefaultAsync(x => x.Id == userId);
            _mapper.Map(applicationUserModel, user);
            await _repoContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<List<TeacherDTO>> GetTeacherByDepartment(int id)
        {
            var Teacher = await _repoContext.Teachers.Include(x => x.Courses).Include(x => x.UserModel).Include(x => x.Department)
                .Where(x => x.Department.DepartmentId == id).ToListAsync();
            List<TeacherDTO> teacher = new List<TeacherDTO>();
            
            foreach (Teacher t in Teacher)
            {
                var _teacher = _mapper.Map<TeacherDTO>(t);
                
                teacher.Add(_teacher);
            }
          
            return teacher;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCourses(CoursesPostDTO _courses)
        {
            var teacher = await _repoTeacher.FindByIdAsync(_courses.TeacherId);
            _courses.Teacher = teacher;
            var department = await _repoContext.Departments.FindAsync(_courses.DepartmentId);
            _courses.Department = department;
            var course = await _repoCourses.FindByIdAsync(_courses.CoursesId);
            _mapper.Map(_courses, course);
            _repoCourses.Update(course);
            await _repoCourses.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<List<StudentDTO>> GetAllStudent()
        {
            var students = await _repoStudent.FindAll();
            List<StudentDTO> _students = new List<StudentDTO>();
            foreach(Student s in students)
            {
                var _student = _mapper.Map<StudentDTO>(s);
                _students.Add(_student);
            }
            return _students;
        }
        [HttpGet("{id}")]
        public async Task<List<StudentDTO>> GetStudentByDepartment(int id)
        {
            var students = await _repoContext.Students.Include(x => x.Department).Include(x => x.UserModel).Include(x => x.StudentCourses).ThenInclude(x => x.Courses)
                .ThenInclude(x => x.Teacher)
                .Where(x => x.Department.DepartmentId == id).ToListAsync();
            List<StudentDTO> _students = new List<StudentDTO>();
            foreach (Student s in students)
            {
                var _student = _mapper.Map<StudentDTO>(s);
                _students.Add(_student);
            }
            return _students;
        }
        [HttpGet]
        public async Task<List<TeacherDTO>> GetAllTeacher()
        {
            var teachers = await _repoTeacher.FindAll();
            List<TeacherDTO> _teachers = new List<TeacherDTO>();
            foreach (Teacher s in teachers)
            {
                var _teacher = _mapper.Map<TeacherDTO>(s);
                _teachers.Add(_teacher);
            }
            return _teachers;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo(ApplicationUserModel _user)
        {
            var user = await _repoContext.UserModel.FindAsync(_user.Id);
            _mapper.Map(_user, user);
            var department = await _repoContext.Departments.FindAsync(_user.DepartmentId);
            var _teacher = new Teacher();
            var _student = new Student();
            if(user.Role == "teacher")
            {
                var teacher = await _repoContext.Teachers.Include(x => x.Department).Where(x => x.UserModel.Id == user.Id).FirstAsync();
                teacher.Department = department;
                _mapper.Map(_user, teacher);
                _teacher = teacher;
            }
            if(user.Role == "student")
            {
                var student = await _repoContext.Students.Include(x => x.Department).Where(x => x.UserModel.Id == user.Id).FirstAsync();
                student.Department = department;
                _mapper.Map(_user, student);
                _student = student;
            }
            _repoContext.Update<Teacher>(_teacher);
            _repoContext.Update<Student>(_student);
            _repoContext.Update<UserModel>(user);
            await _repoContext.SaveChangesAsync();
            return Ok();
        }
    }
}
