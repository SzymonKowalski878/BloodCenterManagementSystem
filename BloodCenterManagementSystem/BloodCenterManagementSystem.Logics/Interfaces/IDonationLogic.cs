using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Interfaces
{
    public interface IDonationLogic:ILogic
    {
        Result<DonationModel> AddDonation(IdHolder data);
        Result<IEnumerable<DonationModel>> ReturnAllDonatorDonations(IdHolder userId);
        Result<DonationModel> ReturnDonationDetails(IdHolder donationId);
        Result<DonationModel> UpdateDonationStage(UpdateDonationStage data);
    }
}
