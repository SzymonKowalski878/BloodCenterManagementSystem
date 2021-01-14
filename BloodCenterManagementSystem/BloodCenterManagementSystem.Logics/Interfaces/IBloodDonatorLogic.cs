using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IBloodDonatorLogic:ILogic
    {
        Result<BloodDonatorModel> RegisterBloodDonator(BloodDonatorModel data);
    }
}
