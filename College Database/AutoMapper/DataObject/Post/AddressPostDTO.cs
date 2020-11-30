using Entity.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject.Post
{
    public class AddressPostDTO
    {
        public int AddressId { get; set; }
        public City City { get; set; }
        public Districts Districts { get; set; }
    }
}
