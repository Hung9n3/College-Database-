using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using College_Database.AutoMapper.DataObject;
using College_Database.AutoMapper.DataObject.Get;
using College_Database.AutoMapper.DataObject.Post;
using Entity.Course;
using Entity.Location;
using Entity.User;

namespace College_Database.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUserModel, UserModel>().ForMember(x => x.Id, opt => opt.Ignore());
                
            CreateMap<CityDTO, City>().ForMember(x => x.CityId, opt => opt.Ignore());
            CreateMap<DistrictDTO, Districts>().ForMember(x => x.DistrictId, opt => opt.Ignore());
            CreateMap<UserModel, UserDTO>().ForMember(x => x.Address, opt => opt.Ignore());
            CreateMap<DepartmentDTO, Department>().ForMember(x => x.DepartmentId, opt => opt.Ignore());
            CreateMap<CoursesPostDTO, Courses>().ForMember(x => x.CoursesId, opt => opt.Ignore());
            CreateMap<Courses, CoursesDTO>();
            CreateMap<Student, UserDTO>()
                .ForMember(x => x.Courses, c => c.MapFrom(c => c.StudentCourses.Select(cs => cs.Courses)));
            CreateMap<Teacher, UserDTO>().ForMember(x => x.Department, x => x.Ignore());
            CreateMap<Teacher, TeacherDTO>().ForMember(x => x.UserModel, c => c.MapFrom(c => c.UserModel))
                .ForMember(x => x.Courses, opt => opt.Ignore());
            CreateMap<Student, StudentGetDTO>().ForMember(x => x.StudentCourses, c => c.MapFrom(c => c.StudentCourses.Select(cs => cs.Courses)))
                .ForMember(x => x.UserModel, c => c.MapFrom(c => c.UserModel));
            CreateMap<DepartmentPostDTO, Department>().ForMember(x => x.DepartmentId, opt => opt.Ignore());
        }
    }
}
