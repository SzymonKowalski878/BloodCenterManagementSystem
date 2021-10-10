﻿using AutoMapper;
using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO;
using BloodCenterManagementSystem.Web.DTO.BloodDonator;
using BloodCenterManagementSystem.Web.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:Controller
    {
        private readonly Lazy<IBloodDonatorLogic> _bloodDonatorLogic;
        protected IBloodDonatorLogic BloodDonatorLogic => _bloodDonatorLogic.Value;

        private readonly Lazy<IUserLogic> _userLogic;
        protected IUserLogic UserLogic => _userLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        private readonly Lazy<IEmailConfirmationService> _emailConfirmationService;
        protected IEmailConfirmationService EmailConfirmationService => _emailConfirmationService.Value;

        public UserController(Lazy<IBloodDonatorLogic> bloodDonatorLogic,
            Lazy<IMapper> mapper,
            Lazy<IUserLogic>userLogic,
            Lazy<IEmailConfirmationService> emailConfirmationService)
        {
            _bloodDonatorLogic = bloodDonatorLogic;
            _mapper = mapper;
            _userLogic = userLogic;
            _emailConfirmationService = emailConfirmationService;
        }

        [Authorize(Policy = "Authenticated")]
        [HttpPost,Route("UpdateUserdata")]
        [ProducesResponseType(typeof(UpdateUserData), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Post(UpdateUserData data)
        {
            if(data == null)
            {
                return BadRequest("UpdateUserData was null");
            }

            var result = BloodDonatorLogic.UpdateUserData(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }

        [Authorize(Policy = "Worker")]
        [HttpPost("/worker")]
        [ProducesResponseType(typeof(AddWorkerDTO), 400)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult RegisterWorker(AddWorkerDTO data)
        {
            var userToAdd = Mapper.Map<AddWorkerDTO, UserModel>(data);

            var result = UserLogic.RegisterWokrer(userToAdd);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }
    }
}

