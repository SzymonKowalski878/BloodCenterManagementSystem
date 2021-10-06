using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.BlodStorage.DataHolders
{
    public class AddForeignBloodUnitToStorage
    {
        public string BloodTypeName { get; set; }
        public int ForeignBloodUnitId { get; set; }
        public string BloodUnitLocation { get; set; }
        public bool IsAfterCovid { get; set; }
    }
}
