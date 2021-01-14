using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class BloodTypeRepository:Repository<BloodTypeModel>,IBloodTypeRepository
    {
        public BloodTypeRepository(Lazy<DataContext> dataContext)
            : base(dataContext)
        {

        }

        public BloodTypeModel GetByBloodTypeName(string bloodTypeName)
        {
            return DataContext.BloodTypes.FirstOrDefault(m => m.BloodTypeName == bloodTypeName);
        }
    }
}
