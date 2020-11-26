using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using College_Database.AutoMapper.DataObject;
using Contracts;
using Entities.User;
using Entity.Context;
using Entity.Course;
using Entity.Location;
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
        private IRepoCity _repoCity;
        private IRepoDistrict _repoDistrict;
        private IRepoTeacher _repoTeacher;
        private IRepoStudent _repoStudent;
        private IRepoAddress _repoAddress;
        private IRepoCourses _repoCourses;
        public CollegeController(IMapper mapper, IRepoStudent repoStudent, IRepoTeacher repoTeacher, IRepoAddress repoAddress, 
                                IRepoCity repoCity, IRepoDistrict repoDistrict, RepoContext repoContext, UserManager<UserModel> userManager,
                                SignInManager<UserModel> signInManager, IOptions<ApplicationSettings> appSettings, IRepoCourses repoCourses)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _repoContext = repoContext;
            _repoAddress = repoAddress;
            _repoCity = repoCity;
            _repoCourses = repoCourses;
            _repoDistrict = repoDistrict;
            _repoTeacher = repoTeacher;
            _repoStudent = repoStudent;
            _mapper = mapper;
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
        public async Task<IActionResult> AddCourses(CoursesDTO _course, int id)
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
            return items;
        }
        [HttpGet("{id}")]
        public async Task<Courses> GetCoursesById(int id)
        {
            var item = await _repoCourses.FindByIdAsync(id);
            return item;
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> TeacherCourses(ListCourses listCourses, int id)
        {
            var teacher = await _repoTeacher.FindByIdAsync(id);
            foreach(int i in listCourses.id)
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
            applicationUserModel.Address.City = await _repoCity.FindByIdAsync(cityid);
            applicationUserModel.Address.Districts = await _repoDistrict.FindByIdAsync(districtid);
            var userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _repoContext.UserModel.Include(x => x.Address).Include(x => x.Address.City)
                .Include(x => x.Address.Districts).FirstOrDefaultAsync(x => x.Id == userId);
            _mapper.Map(applicationUserModel, user);
            _repoAddress.Update(user.Address);
            await _repoContext.SaveChangesAsync();
            return Ok();
        }
    }
}
