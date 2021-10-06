using AutoMapper;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.ResultOfExamination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Mappers
{
    public class ResultOfExaminationProfile:Profile
    {
        public ResultOfExaminationProfile()
        {
            CreateMap<ResultOfExaminationModel, ResultOfExaminationDTO>();
            CreateMap<AddResultOfExaminationDTO, ResultOfExaminationModel>();
            CreateMap<UpdateResultOfExaminationDTO, ResultOfExaminationModel>();
            CreateMap<ResultOfExaminationModel, ResultOfExaminationWithoutDonatorDTO>();
        }
    }
}
