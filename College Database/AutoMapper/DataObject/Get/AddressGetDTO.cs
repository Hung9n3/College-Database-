using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject.Get
{
    public class AddressGetDTO
    {
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
    }
}
