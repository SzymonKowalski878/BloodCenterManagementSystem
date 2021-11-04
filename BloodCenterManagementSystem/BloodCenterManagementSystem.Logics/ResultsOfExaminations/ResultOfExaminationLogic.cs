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

            try
            {
                donation.ResultOfExamination = examination;
                DonationRepository.SaveChanges();
            }
            catch(Exception ex)
            {
                return Result.Error<ResultOfExaminationModel>(ex.Message);
            }

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

            try
            {
                ResultOfExaminationRepository.SaveChanges();
            }
            catch(Exception ex)
            {
                return Result.Error<ResultOfExaminationModel>(ex.Message);
            }

            return Result.Ok(examination);
        }

        public Result<ResultOfExaminationModel> UpdateBloodExaminationResult(ResultOfExaminationModel examination)
        {
            if (examination == null)
            {
                return Result.Error<ResultOfExaminationModel>("IdHolder containing donation id was null");
            }

            var donation = DonationRepository.GetById(examination.DonationId);

            if (donation == null)
            {
                return Result.Error<ResultOfExaminationModel>("Unable to find donation with passed id");
            }

            donation.ResultOfExamination.HB = examination.HB;
            donation.ResultOfExamination.HT = examination.HT;
            donation.ResultOfExamination.RBC = examination.RBC;
            donation.ResultOfExamination.WBC = examination.WBC;
            donation.ResultOfExamination.PLT = examination.PLT;
            donation.ResultOfExamination.MCH = examination.MCH;
            donation.ResultOfExamination.MCHC = examination.MCHC;
            donation.ResultOfExamination.MCV = examination.MCV;
            donation.ResultOfExamination.NE = examination.NE;
            donation.ResultOfExamination.EO = examination.EO;
            donation.ResultOfExamination.BA = examination.LY;
            donation.ResultOfExamination.MO = examination.MO;

            try
            {
                ResultOfExaminationRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Error<ResultOfExaminationModel>(ex.Message);
            }

            return Result.Ok(examination);
        }
    }
}
