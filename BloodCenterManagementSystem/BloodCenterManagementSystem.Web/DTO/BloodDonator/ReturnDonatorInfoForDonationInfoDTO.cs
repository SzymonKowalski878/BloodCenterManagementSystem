﻿using BloodCenterManagementSystem.Web.DTO.BloodType;
using BloodCenterManagementSystem.Web.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.BloodDonator
{
    public class ReturnDonatorInfoForDonationInfoDTO
    {
        public string Pesel { get; set; }
        public BloodTypeNameDTO BloodType { get; set; }
        public ReturnUserShortDTO User { get; set; }
    }
}