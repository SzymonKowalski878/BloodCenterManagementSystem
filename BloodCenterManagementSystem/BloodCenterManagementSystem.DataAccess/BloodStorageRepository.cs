using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class BloodStorageRepository:Repository<BloodStorageModel>,IBloodStorageRepository
    {
        public BloodStorageRepository(Lazy<DataContext> dataContext)
            :base(dataContext)
        {

        }

        public override IEnumerable<BloodStorageModel> GetAll()
        {
            return DataContext.BloodStorage.Include(m => m.BloodType).Include(m => m.Donation).Where(m => m.IsAvailable == true);
        }
    }
}
