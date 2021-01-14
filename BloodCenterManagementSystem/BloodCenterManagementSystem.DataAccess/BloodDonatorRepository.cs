using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class BloodDonatorRepository:Repository<BloodDonatorModel>,IBloodDonatorRepository
    {
        public BloodDonatorRepository(Lazy<DataContext> dataContext)
            :base(dataContext)
        {

        }
    }
}
