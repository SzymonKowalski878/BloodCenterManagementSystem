using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IDonationLogic:ILogic
    {
        Result<DonationModel> AddDonation(int userId);
        Result<IEnumerable<DonationModel>> ReturnAllDonatorDonations(int userId);
        Result<DonationModel> ReturnDonationDetails(int donationId);
        Result<DonationModel> UpdateDonationStage(UpdateDonationStage data);
        Result<IEnumerable<DonationModel>> ReturnQueue(string stage);
        Result<IEnumerable<DonationModel>> ReturnAll();
        Result<DonationModel> GetById(int donationId);
    }
}
