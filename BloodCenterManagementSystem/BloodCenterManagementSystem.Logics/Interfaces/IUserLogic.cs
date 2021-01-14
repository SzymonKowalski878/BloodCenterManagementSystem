using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IUserLogic:ILogic
    {
        Result<UserModel> RegisterAccount(RegisterUserrData data);
    }
}
