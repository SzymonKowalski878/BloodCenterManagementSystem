using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.BlodStorage.DataHolders
{
    public class AddBloodUnitToStorage
    {
        public int DonationId { get; set; }
        public bool IsAfterCovid { get; set; }
    }
}
