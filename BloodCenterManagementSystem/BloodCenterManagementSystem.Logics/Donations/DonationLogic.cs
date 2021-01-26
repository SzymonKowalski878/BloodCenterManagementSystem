using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Donations
{
    public class DonationLogic:IDonationLogic
    {
        private readonly Lazy<IBloodDonatorRepository> _bloodDonatorRepository;
        protected IBloodDonatorRepository BloodDonatorRepository => _bloodDonatorRepository.Value;

        private readonly Lazy<IDonationRepository> _donationRepository;
        protected IDonationRepository DonationRepository => _donationRepository.Value;

        public DonationLogic(Lazy<IBloodDonatorRepository> bloodDonatorRepository,
            Lazy<IDonationRepository> donationRepository)
        {
            _bloodDonatorRepository = bloodDonatorRepository;
            _donationRepository = donationRepository;
        }

        public Result<DonationModel> AddDonation(IdHolder data)
        {
            if(data == null)
            {
                return Result.Error<DonationModel>("Id was null");
            }

            var donator = BloodDonatorRepository.ReturnDonatorInfo(data.Id);

            if(donator == null || donator.User==null)
            {
                return Result.Error<DonationModel>("Unable to find user with that id");
            }

            var latestDonation = DonationRepository.ReturnDonatorNewestDonation(donator.Id);

            if (latestDonation != null && (latestDonation.Stage == "registered" || latestDonation.Stage=="blood examined" || latestDonation.Stage=="qualified"))
            {
                return Result.Error<DonationModel>("Donation in progress and the stage is " + latestDonation.Stage);
            }

            var donation = new DonationModel
            {
                Id = 0,
                BloodDonatorId = donator.Id,
                Stage = "registered",
                DonationDate = DateTime.Now,
                RejectionReason = null
            };

            DonationRepository.Add(donation);
            DonationRepository.SaveChanges();

            

            return Result.Ok(donation);
        }
    }
}
