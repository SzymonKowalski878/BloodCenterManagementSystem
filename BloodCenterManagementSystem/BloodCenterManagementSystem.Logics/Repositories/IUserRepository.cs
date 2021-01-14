using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Repositories
{
    public interface IUserRepository:IRepository<UserModel>
    {
        UserModel GetByEmail(string email);
    }
}
