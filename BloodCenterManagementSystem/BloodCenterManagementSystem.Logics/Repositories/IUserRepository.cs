using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Repositories
{
    public interface IUserRepository:IRepository<UserModel>
    {
        UserModel GetByEmail(string email);
        string GetUserPassword(string  email);
        IEnumerable<UserModel> ReturnAllWorkers();
        void Delete(UserModel data);
    }
}
