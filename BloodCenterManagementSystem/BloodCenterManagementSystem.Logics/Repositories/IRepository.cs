using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Repositories
{
    public interface IRepository<T> where T:BaseModel
    {
        void Add(T model);
        void SaveChanges();
    }
}
