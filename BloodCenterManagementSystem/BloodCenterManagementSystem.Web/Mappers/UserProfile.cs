using AutoMapper;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO;
using BloodCenterManagementSystem.Web.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Mappers
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<AddUserDTO, UserModel>();
            CreateMap<UserModel, ReturnUserInformationDTO>();
            CreateMap<AddWorkerDTO, UserModel>();
        }
    }
}
