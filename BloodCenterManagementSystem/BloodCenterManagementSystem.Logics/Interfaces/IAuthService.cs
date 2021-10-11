using BloodCenterManagementSystem.Logics.Users.DataHolders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IAuthService:ILogic
    {
        string HashPassword(string password);
        bool VerifyPassword(string email, string password);
        UserToken GenerateToken(string email, string role);
    }
}
