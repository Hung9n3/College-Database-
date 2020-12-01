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
        public async Task<IActionResult> AddDepartment(DepartmentDTO _department)
        {
            var department = _mapper.Map<Department>(_department);
            _repoContext.Add(department);
            await _repoContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> AddCourses(CoursesPostDTO _course, int id)
        {
            var department = await _repoContext.Departments.FindAsync(id);
            _course.Department = department;
            _repoContext.Add(_mapper.Map<Courses>(_course));
            await _repoContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<List<Courses>> GetAllCourses()
        {
            var items = await _repoCourses.FindAll();
            //var _items = new List<CoursesDTO>();
            //var item = new CoursesDTO();
            //var teacher = new TeacherGetDTO();
            //foreach (Courses i in items)
            //{
            //    item = _mapper.Map<CoursesDTO>(i);
            //    teacher.TeacherId = i.Teacher.TeacherId;
            //    teacher.TeacherName = i.Teacher.UserModel.FullName;
            //    item.Lecturer = teacher;
            //    _items.Add(item);
            //}
            return items;
        }

        [HttpGet]
        public async Task<List<Department>> GetDepartments()
        {
            var result = await _repoDepartment.FindAll();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<Courses> GetCoursesById(int id)
        {
            var item = await _repoCourses.FindByIdAsync(id);
            return item;
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> TeacherCourses(List<int> listCourses, int id)
        {
            var teacher = await _repoTeacher.FindByIdAsync(id);
            foreach(int i in listCourses)
            {
                var course = await _repoCourses.FindByIdAsync(i);
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
            var _course = new List<CoursesShow>();

            foreach (Teacher t in Teacher)
            {
                var _teacher = _mapper.Map<TeacherDTO>(t);
                _teacher.Courses = _course;
                foreach(Courses C in t.Courses)
                {
                    var c = new CoursesShow()
                    {
                        CoursesId = C.CoursesId,
                        CoursesName = C.CoursesName
                    };
                    _teacher.Courses.Add(c);
                }
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
        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(DepartmentPostDTO departmentPostDTO)
        {
            //ICollection<Teacher> _teacher;
            var department = await _repoContext.Departments.Include(x => x.Teachers).
                FirstAsync(x => x.DepartmentId == departmentPostDTO.DepartmentId);
            foreach(int i in departmentPostDTO.TeacherId)
            {
                departmentPostDTO.Teachers.Add(await _repoTeacher.FindByIdAsync(i));
            }
            _mapper.Map(departmentPostDTO, department);
            _repoContext.Departments.Update(department);
           await _repoContext.SaveChangesAsync();
            return Ok();
        }
    }
}
