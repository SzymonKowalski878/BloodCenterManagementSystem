using BloodCenterManagementSystem.Web.DTO.BloodType;
using BloodCenterManagementSystem.Web.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.BloodDonator
{
    public class ReturnDonatorInformationDTO
    {
        public string Pesel { get; set; }
        public string HomeAdress { get; set; }
        public string PhoneNumber { get; set; }
        public int AmountOfBloodDonated { get; set; }
        public BloodTypeNameDTO BloodType { get; set; }
        public ReturnUserInformationDTO User { get; set; }
    }
}
