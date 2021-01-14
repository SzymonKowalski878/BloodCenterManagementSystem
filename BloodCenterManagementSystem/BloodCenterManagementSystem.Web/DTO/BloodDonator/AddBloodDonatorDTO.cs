using BloodCenterManagementSystem.Web.DTO.BloodType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO
{
    public class AddBloodDonatorDTO
    {
        public string Pesel { get; set; }
        public string HomeAdress { get; set; }
        public string PhoneNumber { get; set; }
        public BloodTypeNameDTO BloodType { get; set; }
        public AddUserDTO User { get; set; }
    }
}
