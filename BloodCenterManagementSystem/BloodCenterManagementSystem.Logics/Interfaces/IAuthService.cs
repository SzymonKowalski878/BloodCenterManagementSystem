using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IAuthService:ILogic
    {
        string HashPassword(string password);
    }
}
