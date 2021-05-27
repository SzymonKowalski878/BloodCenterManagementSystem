using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class BloodDonatorRepository:Repository<BloodDonatorModel>,IBloodDonatorRepository
    {
        public BloodDonatorRepository(Lazy<DataContext> dataContext)
            :base(dataContext)
        {

        }

        public BloodDonatorModel ReturnDonatorInfo(int id)
        {
            return DataContext.BloodDonators.Include(m => m.User).Include(m=>m.BloodType).FirstOrDefault(m => m.User.Id == id);
        }

        public BloodDonatorModel ReturnDoantorInfoByPesel(string pesel)
        {
            return DataContext.BloodDonators.Include(m => m.User).Include(m => m.BloodType).FirstOrDefault(m => m.Pesel == pesel);
        }
    }
}
