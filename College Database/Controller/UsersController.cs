﻿
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entities.User;
using Entity.Context;
using Entity.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Contracts;
using Entity.Location;
using College_Database.AutoMapper.DataObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Entities.StudentCourses;
using Entity.Course;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
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
        public UserController(IMapper mapper,IRepoStudent repoStudent,IRepoTeacher repoTeacher,IRepoAddress repoAddress,
            IRepoCity repoCity, IRepoDistrict repoDistrict,  RepoContext repoContext,UserManager<UserModel> userManager, 
            SignInManager<UserModel> signInManager, IOptions<ApplicationSettings> appSettings, IRepoCourses repoCourses)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _repoContext = repoContext;
            _repoAddress = repoAddress;
            _repoCity = repoCity;
            _repoDistrict = repoDistrict;
            _repoTeacher = repoTeacher;
            _repoCourses = repoCourses;
            _repoStudent = repoStudent;
            _mapper = mapper;
        }
        
       
        [HttpPost("{cityid}/{districtid}")]
        public async Task<Object> Register(ApplicationUserModel model, int cityid, int districtid)
        {
            var address = new Address()
            {
                City = await _repoCity.FindByIdAsync(cityid),
                Districts = await _repoDistrict.FindByIdAsync(districtid)
        };
            _repoAddress.Create(address);
            model.Address = address;
            var applicationUser = _mapper.Map<UserModel>(model);
            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            var user = _repoContext.UserModel.Where(x => x.UserName.Contains(applicationUser.UserName)).FirstOrDefault();
            switch (applicationUser.Role)
            {
                case "teacher":
                    {
                        try
                        {
                            var department = await _repoContext.Departments.FindAsync(model.DepartmentId);
                            var teacher = new Teacher();
                            teacher.Department = department;
                            teacher.UserModel = user;
                            _repoContext.Teachers.Add(teacher);
                            _repoContext.SaveChanges();
                            return Ok(result);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                case "student":
                    {
                        try
                        {                          
                            var student = new Student();
                            student.Id = model.StudentId;
                            student.UserModel = user;
                            _repoContext.Students.Add(student);
                            _repoContext.SaveChanges();
                            return Ok(result);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                case "admin":
                    {
                        try
                        {                          
                            return Ok(result);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                default:
                    return BadRequest("Lack of role !!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Role", user.Role.ToString()),
                        new Claim("Phone",user.Phone.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserInfo()
        {
            string userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _repoContext.UserModel.Include(x => x.Address).Include(x => x.Address.Districts.City).Include(x => x.Address.Districts)
                .Where(x => x.Id == userId).FirstOrDefaultAsync();
             var _user = _mapper.Map<UserDTO>(user);
            if (user.Role == "student")
            {
                var student = await _repoContext.Students.Include(x => x.StudentCourses).ThenInclude(x => x.Courses).Where(x => x.UserModel.Id == userId)
                    .AsNoTracking().FirstOrDefaultAsync();
                _mapper.Map(student, _user);
                
            }
            if(user.Role == "teacher")
            {
                var teacher = await _repoContext.Teachers.Include(x => x.Department).Include(x => x.Courses).
                    Include(x => x.UserModel).Where(x => x.UserModel.Id == userId).AsNoTracking().FirstOrDefaultAsync();
                _mapper.Map(teacher, _user);
            }
            _user.City = user.Address.Districts.City;
            _user.District = user.Address.Districts;
            return Ok(_user);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ApplyCourse(ListCourses listCourses)
        {
            string userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var student = await _repoContext.Students.Include(x => x.UserModel).Where(x => x.UserModel.Id == userId).FirstOrDefaultAsync();
            foreach (int i in listCourses.id)
            {
                var courses = await _repoCourses.FindByIdAsync(i);
                var studentcourses = new StudentCourses()
                {
                    Courses = courses,
                    Student = student
                };
                _repoContext.StudentCourses.Add(studentcourses);
                student.Bill = student.Bill + courses.Credits * 55;
            }
            _repoStudent.Update(student);
           await _repoContext.SaveChangesAsync();
            return Ok();
        }
    }
}