using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Models
{
    public class BloodStorageModel:BaseModel
    {
        public int ForeignBloodUnitId { get; set; }
        public string BloodUnitLocation { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsAfterCovid { get; set; }
        public int BloodTypeId { get; set; }
        public BloodTypeModel BloodType { get; set; }
        public int DonationId { get; set; }
        public DonationModel Donation { get; set; }
    }
}
