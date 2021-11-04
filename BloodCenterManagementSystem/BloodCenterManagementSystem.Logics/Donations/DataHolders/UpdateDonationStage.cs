using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Donations.DataHolders
{
    public class UpdateDonationStage
    {
        public int Id { get; set; }
        public string Stage { get; set; }
        public string RejectionReason { get; set; }
    }
}
