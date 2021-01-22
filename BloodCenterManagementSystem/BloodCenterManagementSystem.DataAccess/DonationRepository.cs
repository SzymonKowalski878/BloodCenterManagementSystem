using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class DonationRepository: Repository<DonationModel>, IDonationRepository
    {
        public DonationRepository(Lazy<DataContext> dataContext)
            :base(dataContext)
        {

        }


    }
}
