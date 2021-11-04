using BloodCenterManagementSystem.Web.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.BloodDonator
{
    public class ReturnDonatorInQueueDTO
    {
        public string Pesel { get; set; }
        public ReturnUserShortDTO User { get; set; }
    }
}
