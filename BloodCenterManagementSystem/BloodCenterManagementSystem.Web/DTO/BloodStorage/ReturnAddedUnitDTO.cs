using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.BloodStorage
{
    public class ReturnAddedUnitDTO
    {
        public int Id { get; set; }
        public int? ForeignBloodUnitId { get; set; }
        public string BloodUnitLocation { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsAfterCovid { get; set; }
        public int BloodTypeId { get; set; }
        public int? DonationId { get; set; }
    }
}
