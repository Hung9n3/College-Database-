using Entity.BaseModel;
using Entity.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject
{
    public class CityDTO 
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public ICollection<Districts> Districts { get; set; }
    }
}
