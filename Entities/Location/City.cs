using Entity.BaseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Location
{
    public class City 
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }
        public ICollection<Districts> Districts { get; set; }
    }
}
