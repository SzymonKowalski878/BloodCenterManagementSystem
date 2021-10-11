using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class UserRepository: Repository<UserModel>,IUserRepository
    {
        public UserRepository(Lazy<DataContext> dataContext)
            :base(dataContext)
        {
            
        }

        public UserModel GetByEmail(string email)
        {
            return DataContext.Set<UserModel>().FirstOrDefault(m => m.Email == email);
        }

        public string GetUserPassword(string email)
        {
            var result = DataContext.Users.FirstOrDefault(m => m.Email == email);

            return result?.Password;
        }

        public override UserModel GetById(int id)
        {
            return DataContext.Users.FirstOrDefault(m => m.Id == id);
        }
    }
}
