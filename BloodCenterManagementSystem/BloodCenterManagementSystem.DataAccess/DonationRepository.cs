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
            return DataContext.Donations.Include(m => m.ResultOfExamination).Include(m=>m.BloodDonator.User).FirstOrDefault(m => m.Id == donationId);
        }

        public override DonationModel GetById(int id)
        {
            return DataContext.Donations.Include(m => m.BloodDonator).Include(m => m.BloodDonator.User).Include(m=>m.BloodDonator.BloodType).Include(m=>m.BloodStorage).FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<DonationModel> GetDonationsInQueue()
        {
            var list = new List<DonationModel>();
            
            var registered = DataContext.Donations.Include(m => m.BloodDonator).Include(m => m.BloodDonator.User).Where(m => m.Stage == "registered");

            foreach(var x in registered)
            {
                list.Add(x);
            }

            var qualified = DataContext.Donations.Include(m => m.BloodDonator).Include(m => m.BloodDonator.User).Where(m => m.Stage == "qualified");

            foreach (var x in qualified)
            {
                list.Add(x);
            }

            var examinated = DataContext.Donations.Include(m => m.BloodDonator).Include(m => m.BloodDonator.User).Where(m => m.Stage == "blood examined");

            foreach (var x in examinated)
            {
                list.Add(x);
            }

            return list;
        }

        public IEnumerable<DonationModel> GetDonationInQueue(string stage)
        {
            return DataContext.Donations.Include(m => m.BloodDonator).Include(m => m.BloodDonator.User).Where(m => m.Stage == stage);
        }

        public override IEnumerable<DonationModel> GetAll()
        {
            return DataContext.Donations.Include(m => m.BloodDonator).Include(m => m.BloodDonator.User);
        }
    }
}
