using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Models
{
    public class DonationModel:BaseModel
    {
        public string Stage { get; set; }
        public DateTime DonationDate { get; set; }
        public string RejectionReason { get; set; }
        public virtual int BloodDonatorId { get; set; }
        public virtual BloodDonatorModel BloodDonator { get; set; }
        public BloodStorageModel BloodStorage { get; set; }
        public ResultOfExaminationModel ResultOfExamination { get; set; }
    }
}
