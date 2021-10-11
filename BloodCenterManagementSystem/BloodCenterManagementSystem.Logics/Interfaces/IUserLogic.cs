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
        Result<UserToken> Login(UserEmailAndPassword data);
        Result<UserToken> RenewToken(string email);
        Result<UserModel> RegisterWokrer(UserModel data);
    }
}
