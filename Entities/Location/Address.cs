
using Entity.Location;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Entity.BaseModel;

namespace Entity.Location
{
    public class Address
    {
        [Key]
        public int AddressId {get;set;}
        public City City { get; set; }
        public Districts Districts { get; set; }
    }
}
