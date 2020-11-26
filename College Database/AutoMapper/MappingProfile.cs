using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using College_Database.AutoMapper.DataObject;
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
            CreateMap<UserModel, UserDTO>();
            CreateMap<DepartmentDTO, Department>().ForMember(x => x.DepartmentId, opt => opt.Ignore());
            CreateMap<CoursesDTO, Courses>().ForMember(x => x.CoursesId, opt => opt.Ignore());
            CreateMap<Student, UserDTO>().ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Courses, c => c.MapFrom(c => c.StudentCourses.Select(cs => cs.Courses)));
            CreateMap<Teacher,UserDTO>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
