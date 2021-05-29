using AutoMapper;
using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.Donation;
using BloodCenterManagementSystem.Web.DTO.ResultOfExamination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationController:Controller
    {
        private readonly Lazy<IDonationLogic> _donationLogic;
        protected IDonationLogic DonationLogic => _donationLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        public DonationController(Lazy<IDonationLogic> donationLogic,
            Lazy<IMapper> mapper)
        {
            _donationLogic = donationLogic;
            _mapper = mapper;
        }

        [Authorize(Policy = "Worker")]
        [HttpPost,Route("AddDonation")]
        [ProducesResponseType(typeof(ReturnDonationDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Post(IdHolder data)
        {
            var result = DonationLogic.AddDonation(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var donationToReturn = Mapper.Map<DonationModel, ReturnDonationDTO>(result.Value);

            if (donationToReturn == null)
            {
                return BadRequest("Error during mapping with automapper");
            }

            return Ok(donationToReturn);
        }

        [Authorize(Policy = "Authenticated")]
        [HttpPost,Route("ReturnDonatorsAllDonations")]
        [ProducesResponseType(typeof(List<ReturnDonationSmallDTO>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult ReturnAllDonatorDonations(IdHolder userId)
        {
            var result = DonationLogic.ReturnAllDonatorDonations(userId);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var list = new List<ReturnDonationSmallDTO>();

            foreach(var x in result.Value)
            {
                list.Add(Mapper.Map<DonationModel, ReturnDonationSmallDTO>(x));
            }

            return Ok(list);
        }

        [Authorize(Policy = "Authenticated")]
        [HttpPost,Route("ReturnDonationDetails")]
        [ProducesResponseType(typeof(ReturnDonationDetailsDTO), 200)]
        [ProducesResponseType(204)] 
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult ReturnDonationDetails(IdHolder donationId)
        {
            var result = DonationLogic.ReturnDonationDetails(donationId);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            //returns 204 if donation exists but there are no details results of examination

            var toReturn = Mapper.Map<DonationModel, ReturnDonationDetailsDTO>(result.Value);

            return Ok(toReturn);
        }

        [Authorize(Policy = "Worker")]
        [HttpPost,Route("UpdateDonationStage")]
        [ProducesResponseType(typeof(UpdateDonationStage), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult UpdateDonationStage(UpdateDonationStage data)
        {
            var result = DonationLogic.UpdateDonationStage(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }

        [Authorize(Policy = "Worker")]
        [HttpPost,Route("ReturnQueue")]
        [ProducesResponseType(typeof(IEnumerable<ReturnDonationInQueueDTO>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult ReturnQueue(string stage)
        {
            var result = DonationLogic.ReturnQueue(stage);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var list = new List<ReturnDonationInQueueDTO>();

            foreach (var x in result.Value)
            {
                list.Add(Mapper.Map<DonationModel, ReturnDonationInQueueDTO>(x));
            }

            return Ok(list);
        }
    }
}
