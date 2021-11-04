using BloodCenterManagementSystem.Logics.Users.DataHolders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IEmailConfirmationService:ILogic
    {
        Result<UserToken> GenerateUserConfirmationToken(string login);
        Result<bool> ValidateConfirmationToken(string authToken);
    }
}
