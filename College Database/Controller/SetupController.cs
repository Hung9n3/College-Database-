using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using College_Database.AutoMapper.DataObject;
using Entity.Location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Entity.Context;
using Contracts;

namespace College_Database.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private RepoContext _repoContext;
        private IRepoCity _repoCity;
        private IRepoDistrict _repoDistrict;
        private IMapper _mapper;
        public SetupController(RepoContext repoContext, IMapper mapper, IRepoCity repoCity, IRepoDistrict repoDistrict)
        {
            _repoCity = repoCity;
            _repoDistrict = repoDistrict;
            _repoContext = repoContext;
            _mapper = mapper;
        }
        [HttpPost]
        public async  Task<IActionResult> AddCity(CityDTO _city)
        {
            var city = _mapper.Map<City>(_city);
            _repoContext.City.Add(city);
           await _repoContext.SaveChangesAsync();
             var cities = await _repoCity.FindAll();
            return Ok(cities);
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> AddDistrict(DistrictDTO districtDTO, int id)
        {
            var district = _mapper.Map<Districts>(districtDTO);
            district.City = await _repoCity.FindByIdAsync(id);
            _repoDistrict.Create(district);
            await _repoDistrict.SaveChangesAsync();
            return Ok();
        }
    }
}
