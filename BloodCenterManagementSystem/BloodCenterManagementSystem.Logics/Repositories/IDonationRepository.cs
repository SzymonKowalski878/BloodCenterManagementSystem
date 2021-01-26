using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Repositories
{
    public interface IDonationRepository:IRepository<DonationModel>
    {
        DonationModel ReturnDonatorNewestDonation(int Donatorid);
        IEnumerable<DonationModel> ReturnDonatorsAllDonations(int userId);
        DonationModel ReturnDonationDetails(int donationId);
    }
}
