using AutoMapper;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.BloodType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Mappers
{
    public class BloodTypeProfile:Profile
    {
        public BloodTypeProfile()
        {
            CreateMap<BloodTypeNameDTO, BloodTypeModel>();
        }
    }
}
