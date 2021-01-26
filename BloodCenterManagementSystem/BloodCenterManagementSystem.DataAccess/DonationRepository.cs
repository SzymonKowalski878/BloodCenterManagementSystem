using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class DonationRepository: Repository<DonationModel>, IDonationRepository
    {
        public DonationRepository(Lazy<DataContext> dataContext)
            :base(dataContext)
        {

        }

        public DonationModel ReturnDonatorNewestDonation(int Donatorid)
        {
            return DataContext.Donations.Where(m => m.BloodDonatorId == Donatorid).OrderByDescending(m => m.DonationDate).FirstOrDefault();
        }

    }
}
