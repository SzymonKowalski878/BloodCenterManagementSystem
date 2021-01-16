using BloodCenterManagementSystem.Web.DTO.BloodDonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.User
{
    public class ReturnUserInformationDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
