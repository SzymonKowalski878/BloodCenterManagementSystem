using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.ResultsOfExaminations
{
    public class ResultOfExaminationLogic:IResultOfExaminationLogic
    {
        private readonly Lazy<IResultOfExaminationRepository> _resultOfExaminationRepository;
        protected IResultOfExaminationRepository ResultOfExaminationRepository => _resultOfExaminationRepository.Value;

        private readonly Lazy<IDonationRepository> _donationRepository;
        protected IDonationRepository DonationRepository => _donationRepository.Value;

        public ResultOfExaminationLogic(Lazy<IResultOfExaminationRepository> resultOfExaminationRepository,
            Lazy<IDonationRepository> donationRepository)
        {
            _resultOfExaminationRepository = resultOfExaminationRepository;
            _donationRepository = donationRepository;
        }

        public Result<ResultOfExaminationModel> AddResults(ResultOfExaminationModel examination)
        {
            if (examination == null)
            {
                return Result.Error<ResultOfExaminationModel>("IdHolder containing donation id was null");
            }

            var donation = DonationRepository.GetById(examination.DonationId);

            if(donation == null)
            {
                return Result.Error<ResultOfExaminationModel>("Unable to find donation with passed id");
            }

            if (donation.ResultOfExamination != null)
            {
                return Result.Error<ResultOfExaminationModel>("Result already exists");
            }

            donation.ResultOfExamination = examination;
            DonationRepository.SaveChanges();

            return Result.Ok(examination);
        }

        public Result<ResultOfExaminationModel> UpdateResults(ResultOfExaminationModel examination)
        {
            if (examination == null)
            {
                return Result.Error<ResultOfExaminationModel>("IdHolder containing donation id was null");
            }

            var resultOfExamination = ResultOfExaminationRepository.GetById(examination.DonationId);

            if (resultOfExamination == null)
            {
                return Result.Error<ResultOfExaminationModel>("Unable to find donation with passed id");
            }

            resultOfExamination.BloodPressureLower = examination.BloodPressureLower;
            resultOfExamination.BloodPressureUpper = examination.BloodPressureUpper;
            resultOfExamination.Height = examination.Height;
            resultOfExamination.Weight = examination.Weight;

            ResultOfExaminationRepository.SaveChanges();

            return Result.Ok(examination);
        }
    }
}
