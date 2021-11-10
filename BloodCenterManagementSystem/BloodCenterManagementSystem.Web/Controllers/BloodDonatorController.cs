﻿using AutoMapper;
using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Filters;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.Controllers.DataHolders;
using BloodCenterManagementSystem.Web.Controllers.Responses;
using BloodCenterManagementSystem.Web.DTO;
using BloodCenterManagementSystem.Web.DTO.BloodDonator;
using BloodCenterManagementSystem.Web.DTO.BloodType;
using BloodCenterManagementSystem.Web.DTO.Donation;
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
    [Route("api/blooddonators")]
    public class BloodDonatorController:Controller
    {
        private readonly Lazy<IBloodDonatorLogic> _bloodDonatorLogic;
        protected IBloodDonatorLogic BloodDonatorLogic => _bloodDonatorLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        private readonly Lazy<IDonationLogic> _donationLogic;
        protected IDonationLogic DonationLogic => _donationLogic.Value;


        public BloodDonatorController(Lazy<IBloodDonatorLogic> bloodDonatorLogic,
            Lazy<IMapper> mapper,
            Lazy<IDonationLogic> donationLogic)
        {
            _bloodDonatorLogic = bloodDonatorLogic;
            _mapper = mapper;
            _donationLogic = donationLogic;
        }

        [Authorize(Policy ="Authenticated")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReturnDonatorInformationDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Get([FromRoute]int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var loggedInUserId = identity.FindFirst("UserId")?.Value;
            var loggedInUserRole = identity.FindFirst("Role")?.Value;
            if (!string.IsNullOrEmpty(loggedInUserRole) && loggedInUserRole == "Donator")
            {
                if (!string.IsNullOrEmpty(loggedInUserId) && loggedInUserId != id.ToString())
                {
                    return BadRequest(Result.Error<BloodDonatorModel>("Id passed in route isn't equal to id in token").ErrorMessages);
                }
            }

            var result = BloodDonatorLogic.ReturnDonatorInformation(id);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var donatorToReturn = Mapper.Map<BloodDonatorModel, ReturnDonatorInformationDTO>(result.Value);
            return Ok(donatorToReturn);
        }

        [Authorize(Policy = "Worker")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReturnDonatorShortDTO>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Get()
        {
            var result = BloodDonatorLogic.GetAll(); 

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var listToReturn = new List<ReturnDonatorShortDTO>();

            foreach(var x in result.Value)
            {
                listToReturn.Add(Mapper.Map<BloodDonatorModel,ReturnDonatorShortDTO>(x));
            }

            return Ok(listToReturn);
        }

        [Authorize(Policy = "Worker")]
        [HttpPost]
        [ProducesResponseType(typeof(ReturnOk), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Post(AddBloodDonatorDTO data)
        {
            var donatorToAdd = Mapper.Map<AddBloodDonatorDTO, BloodDonatorModel>(data);

            var result = BloodDonatorLogic.RegisterBloodDonator(donatorToAdd);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(new ReturnOk { Status = "ok" });
        }

        [Authorize(Policy = "Authenticated")]
        [HttpGet("{userId}/donations")]
        [ProducesResponseType(typeof(IEnumerable<ReturnDonationSmallDTO>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult ReturnDonatorsAllDonations([FromRoute] int userId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var loggedInUserId = identity.FindFirst("UserId")?.Value;
            var loggedInUserRole = identity.FindFirst("Role")?.Value;
            if (!string.IsNullOrEmpty(loggedInUserRole) && loggedInUserRole == "Donator")
            {
                if (!string.IsNullOrEmpty(loggedInUserId) && loggedInUserId != userId.ToString())
                {
                    return BadRequest(Result.Error<IEnumerable<DonationModel>>("Wypierdalaj").ErrorMessages);
                }
            }

            var result = DonationLogic.ReturnAllDonatorDonations(userId);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var list = new List<ReturnDonationSmallDTO>();

            foreach (var x in result.Value)
            {
                list.Add(Mapper.Map<DonationModel, ReturnDonationSmallDTO>(x));
            }

            return Ok(list);
        }
    }
}
