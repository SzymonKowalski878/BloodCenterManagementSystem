using AutoMapper;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.BloodStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Mappers
{
    public class BloodStorageProfile:Profile
    {
        public BloodStorageProfile()
        {
            CreateMap<BloodStorageModel, ReturnBloodUnitInStorage>();
        }
    }
}
