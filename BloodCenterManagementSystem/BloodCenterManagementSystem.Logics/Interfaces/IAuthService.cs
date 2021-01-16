using BloodCenterManagementSystem.Logics.Users.DataHolders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IAuthService:ILogic
    {
        string HashPassword(string password);
        bool VerifyPassword(int id, string password);
        UserToken GenerateToken(int id, string role);
    }
}
