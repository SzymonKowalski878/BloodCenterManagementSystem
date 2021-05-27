using BloodCenterManagementSystem.Logics.BloodTypes;
using BloodCenterManagementSystem.Logics.BloodTypes.DataHolders;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IBloodTypeLogic:ILogic
    {
        Result<IEnumerable<BloodTypeModel>> GetAllBloodTypes();
        Result<BloodTypeModel> GetById(int id);
        Result<BloodTypeModel> GetByName(BloodTypeName bloodTypeName);
    }
}
