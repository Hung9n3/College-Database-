
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Entity.BaseModel;

namespace Entity.Location
{
    public class Districts 
    {
        [Key]
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public City City { get; set; }
    }
}