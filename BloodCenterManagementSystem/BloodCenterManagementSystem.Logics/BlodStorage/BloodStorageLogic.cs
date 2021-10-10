using BloodCenterManagementSystem.Logics.BlodStorage.DataHolders;
using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.Logics.BlodStorage
{
    public class BloodStorageLogic:IBloodStorageLogic
    {
        private readonly Lazy<IBloodStorageRepository> _bloodStorageRepository;
        protected IBloodStorageRepository BloodStorageRepository => _bloodStorageRepository.Value;

        private readonly Lazy<IDonationRepository> _donationRepository;
        protected IDonationRepository DonationRepository => _donationRepository.Value;

        private readonly Lazy<IBloodTypeRepository> _bloodTypeRepository;
        protected IBloodTypeRepository BloodTypeRepository => _bloodTypeRepository.Value;

        public BloodStorageLogic(Lazy<IBloodStorageRepository> bloodStorageRepository,
            Lazy<IDonationRepository> donationRepository,
            Lazy<IBloodTypeRepository> bloodTypeRepository)
        {
            _bloodStorageRepository = bloodStorageRepository;
            _donationRepository = donationRepository;
            _bloodTypeRepository = bloodTypeRepository;
        }

        public Result<BloodStorageModel> AddBloodUnit(AddBloodUnitToStorage data)
        {
            if (data == null)
            {
                return Result.Error<BloodStorageModel>("Data containing donation id was null");
            }

            var donation = DonationRepository.GetById(data.DonationId);

            if(donation==null || donation.BloodDonator == null)
            {
                return Result.Error<BloodStorageModel>("Unable to find donation with such id or blood donator was null");
            }

            if (donation.BloodStorage != null)
            {
                return Result.Error<BloodStorageModel>("Blood unit already in database");
            }

            if (donation.Stage != "qualified")
            {
                return Result.Error<BloodStorageModel>("Wrong donation stage");
            }

            var unit = new BloodStorageModel
            {
                Id = 0,
                ForeignBloodUnitId = null,
                BloodUnitLocation=null,
                IsAvailable=true,
                IsAfterCovid=data.IsAfterCovid,
                BloodTypeId=donation.BloodDonator.BloodTypeId,
                DonationId = donation.Id,
            };

            var bloodType = BloodTypeRepository.GetById(donation.BloodDonator.BloodTypeId);

            if (bloodType == null)
            {
                return Result.Error<BloodStorageModel>("Unable to find blood type");
            }

            bloodType.AmmountOfBloodInBank += 450;

            donation.BloodDonator.AmmountOfBloodDonated += 450;
            donation.Stage = "donation finished";

            BloodStorageRepository.Add(unit);
            BloodStorageRepository.SaveChanges();

            return Result.Ok(unit);
        }

        public Result<BloodStorageModel> AddForeignBloodUnit(AddForeignBloodUnitToStorage data)
        {
            if (data == null)
            {
                return Result.Error<BloodStorageModel>("Data passed in the function was null");
            }

            if (string.IsNullOrEmpty(data.BloodUnitLocation))
            {
                return Result.Error<BloodStorageModel>("Blood unit location was null or empty");
            }

            var bloodType = BloodTypeRepository.GetByBloodTypeName(data.BloodTypeName);

            if (bloodType == null)
            {
                return Result.Error<BloodStorageModel>("Unable to find blood type");
            }

            var unit = new BloodStorageModel
            {
                Id = 0,
                ForeignBloodUnitId = data.ForeignBloodUnitId,
                BloodUnitLocation = data.BloodUnitLocation,
                IsAvailable = true,
                IsAfterCovid = data.IsAfterCovid,
                DonationId = null,
                BloodType = bloodType
            };

            bloodType.AmmountOfBloodInBank += 450;

            BloodStorageRepository.Add(unit);
            BloodStorageRepository.SaveChanges();

            return Result.Ok(unit);
        }

        public Result<IEnumerable<BloodStorageModel>> ReturnAllAvailableBloodUnits()
        {
            var units = BloodStorageRepository.GetAll();

            if (units.Count() == 0)
            {
                return Result.Error<IEnumerable<BloodStorageModel>>("No available blood units");
            }

            return Result.Ok(units);
        }

        public Result<BloodStorageModel> ChangeBloodUnitToUnavailable(int bloodUnitId)
        {

            var unit = BloodStorageRepository.GetById(bloodUnitId);

            if(unit == null)
            {
                return Result.Error<BloodStorageModel>("Unable to find blood unit");
            }

            unit.IsAvailable = false;

            var bloodType = BloodTypeRepository.GetById(unit.BloodTypeId);

            bloodType.AmmountOfBloodInBank -= 450;

            BloodStorageRepository.SaveChanges();

            return Result.Ok(unit);
        }
    }
}
