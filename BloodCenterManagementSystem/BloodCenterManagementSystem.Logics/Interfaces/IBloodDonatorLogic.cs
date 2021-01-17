using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IBloodDonatorLogic:ILogic
    {
        Result<BloodDonatorModel> RegisterBloodDonator(BloodDonatorModel data);
        Result<BloodDonatorModel> ReturnDonatorInformation(int id);
        Result<BloodDonatorModel> UpdateUserData(UpdateUserData data);
    }
}
