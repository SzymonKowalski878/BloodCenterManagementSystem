using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IEmailSender:ILogic
    {
        Result<bool> SendEmail(MessageModel message);
    }
}
