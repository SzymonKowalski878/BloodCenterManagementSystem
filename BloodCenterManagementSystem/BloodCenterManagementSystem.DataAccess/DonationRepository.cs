using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<DonationModel> ReturnDonatorsAllDonations(int userId)
        {
            return DataContext.Donations.Where(m => m.BloodDonator.User.Id == userId);
        }

        public DonationModel ReturnDonationDetails(int donationId)
        {
            return DataContext.Donations.Include(m => m.ResultOfExamination).FirstOrDefault(m => m.Id == donationId);
        }

        public override DonationModel GetById(int id)
        {
            return DataContext.Donations.Include(m => m.ResultOfExamination).FirstOrDefault(m => m.Id == id);
        }
    }
}
