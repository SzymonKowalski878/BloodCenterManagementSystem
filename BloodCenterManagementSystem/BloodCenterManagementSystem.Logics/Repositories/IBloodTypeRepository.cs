using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Repositories
{
    public interface IBloodTypeRepository:IRepository<BloodTypeModel>
    {
        BloodTypeModel GetByBloodTypeName(string bloodTypeName);
    }
}
