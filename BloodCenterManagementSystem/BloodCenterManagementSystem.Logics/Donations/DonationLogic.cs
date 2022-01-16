using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Result<DonationModel> AddDonation(int userId)
        {
            var donator = BloodDonatorRepository.ReturnDonatorInfo(userId);

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

            try
            {
                DonationRepository.Add(donation);
                DonationRepository.SaveChanges();
            }
            catch(Exception ex)
            {
                return Result.Error<DonationModel>(ex.Message);
            }

            return Result.Ok(donation);
        }

        public Result<IEnumerable<DonationModel>>ReturnAllDonatorDonations(int userId)
        {
            var donations = DonationRepository.ReturnDonatorsAllDonations(userId);

            return Result.Ok(donations);
        }


        public Result<DonationModel> ReturnDonationDetails(int donationId)
        {

            var donation = DonationRepository.ReturnDonationDetails(donationId);

            if(donation == null)
            {
                return Result.Error<DonationModel>("Unable to find donation with such id");
            }

            return Result.Ok(donation);
        }

        public Result<DonationModel> UpdateDonationStage(UpdateDonationStage data)
        {
            if(data == null)
            {
                return Result.Error<DonationModel>("Data containg donation stage and donation id was null");
            }

            if(!(data.Stage=="qualified"|| data.Stage == "not qualified" || data.Stage == "blood examined" || data.Stage == "abandoned" || data.Stage=="donation finished"))
            {
                return Result.Error<DonationModel>("Wrong donation stage name passed in the function");
            }

            var donation = DonationRepository.GetById(data.Id);

            if (donation == null)
            {
                return Result.Error<DonationModel>("Unable to find donation with passed id");
            }

            if(data.Stage== "abandoned")
            {
                donation.RejectionReason = donation.Stage;
                donation.Stage = "abandoned";
            }

            if(data.Stage == "not qualified")
            {
                if (string.IsNullOrEmpty(data.RejectionReason))
                {
                    return Result.Error<DonationModel>("Rejection reason was null or empty while trying to set stage to not qualified");
                }

                donation.RejectionReason = data.RejectionReason;
            }


            try
            {

                donation.Stage = data.Stage;
                DonationRepository.SaveChanges();
            }
            catch(Exception ex)
            {
                return Result.Error<DonationModel>(ex.Message);
            }

            return Result.Ok(donation);
        }

        public Result<IEnumerable<DonationModel>> ReturnQueue(string stage)
        {
            if (string.IsNullOrEmpty(stage))
            {
                var results = DonationRepository.GetDonationsInQueue();

                return Result.Ok(results);
            }
            else
            {
                if (!(stage == "qualified" || stage == "blood examined" || stage == "registered"))
                {
                    return Result.Error<IEnumerable<DonationModel>>("Wrong stage passed in the function");
                }
                var results = DonationRepository.GetDonationInQueue(stage);

                return Result.Ok(results);
            }
        }

        public Result<IEnumerable<DonationModel>> ReturnAll()
        {
            var result = DonationRepository.GetAll();

            return Result.Ok(result);
        }

        public Result<DonationModel> GetById(int donationId)
        {
            var result = DonationRepository.GetById(donationId);

            if(result == null)
            {
                return Result.Error<DonationModel>("No donation found");
            }

            return Result.Ok(result);
        }
    }
}
