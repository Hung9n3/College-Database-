using Entity.BaseModel;
using Entity.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject
{
    public class AddressDTO 
    {
        public int AddressId { get; set; }
        public City City { get; set; }
        public Districts Districts { get; set; }
    }
}
