using BloodCenterManagementSystem.Web.DTO.ResultOfExamination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.Donation
{
    public class ReturnDonationDetailsDTO
    {
        public int Id { get; set; }
        public string Stage { get; set; }
        public DateTime DonationDate { get; set; }
        public ResultOfExaminationWithoutDonatorDTO ResultOfExamination { get; set; }
    }
}
