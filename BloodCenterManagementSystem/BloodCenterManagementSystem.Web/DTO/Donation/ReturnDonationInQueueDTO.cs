using BloodCenterManagementSystem.Web.DTO.BloodDonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.Donation
{
    public class ReturnDonationInQueueDTO
    {
        public string Stage { get; set; }
        public DateTime DonationDate { get; set; }
        public ReturnDonatorInQueueDTO BloodDonator { get; set; }
    }
}
