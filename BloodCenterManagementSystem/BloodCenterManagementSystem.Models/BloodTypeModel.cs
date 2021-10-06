using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Models
{
    public class BloodTypeModel:BaseModel
    {
        public string BloodTypeName { get; set; }
        public int AmmountOfBloodInBank { get; set; }
    }
}
