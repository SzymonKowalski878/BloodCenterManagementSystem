using BloodCenterManagementSystem.Web.DTO.BloodDonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.Donation
{
    public class ReturnDonationDTO
    {
        public int Id { get; set; }
        public DateTime DonationDate { get; set; }
        public string RejectionReason { get; set; }
        public ReturnDonatorInformationDTO BloodDonator { get; set; }
    }
}
