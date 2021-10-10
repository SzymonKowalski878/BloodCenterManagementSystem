using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IUserLogic:ILogic
    {
        Result<string> RegisterAccount(string email, string authToken, string password);
        Result<UserToken> Login(UserIdAndPassword data);
        Result<UserToken> RenewToken(int id);
        Result<UserModel> RegisterWokrer(UserModel data);
    }
}
