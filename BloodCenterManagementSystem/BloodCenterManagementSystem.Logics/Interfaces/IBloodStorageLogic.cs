using BloodCenterManagementSystem.Logics.BlodStorage.DataHolders;
using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IBloodStorageLogic:ILogic
    {
        Result<BloodStorageModel> AddBloodUnit(AddBloodUnitToStorage data);
        Result<BloodStorageModel> AddForeignBloodUnit(AddForeignBloodUnitToStorage data);
        Result<IEnumerable<BloodStorageModel>> ReturnAllAvailableBloodUnits();
        Result<BloodStorageModel> ChangeBloodUnitToUnavailable(int bloodUnitId);
    }
}
