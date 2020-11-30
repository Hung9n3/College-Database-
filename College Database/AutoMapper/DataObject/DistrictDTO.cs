﻿using Entity.BaseModel;
using Entity.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College_Database.AutoMapper.DataObject
{
    public class DistrictDTO 
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public City City { get; set; }
    }
}
