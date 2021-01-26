using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.ResultOfExamination
{
    public class UpdateResultOfExaminationDTO
    {
        public int DonationId { get; set; }
        public int BloodPressureUpper { get; set; }
        public int BloodPressureLower { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
    }
}
