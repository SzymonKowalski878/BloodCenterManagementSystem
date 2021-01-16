using AutoMapper;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO;
using BloodCenterManagementSystem.Web.DTO.BloodDonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Mappers
{
    public class BloodDonatorProfile:Profile
    {
        public BloodDonatorProfile()
        {
            CreateMap<AddBloodDonatorDTO, BloodDonatorModel>();
            CreateMap<BloodDonatorModel, ReturnDonatorInformationDTO>();
        }
    }
}
