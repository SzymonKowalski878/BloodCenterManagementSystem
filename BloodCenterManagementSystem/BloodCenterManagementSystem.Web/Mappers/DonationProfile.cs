using AutoMapper;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.Donation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Mappers
{
    public class DonationProfile:Profile
    {
        public DonationProfile()
        {
            CreateMap<DonationModel, ReturnDonationDTO>();
            CreateMap<DonationModel, ReturnDonationInQueueDTO>();
        }
    }
}
