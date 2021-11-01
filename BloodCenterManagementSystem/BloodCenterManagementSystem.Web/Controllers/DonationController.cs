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
using System.Security.Claims;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/donations")]
    public class DonationController : Controller
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
      
        [HttpPost("{userId}")]
        [Authorize(Policy = "Worker")]
        [ProducesResponseType(typeof(ReturnDonationDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult AddDonation([FromRoute]int userId)
        {
            var result = DonationLogic.AddDonation(userId);

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



        [HttpGet("queue")]
        [Authorize(Policy = "Worker")]
        [ProducesResponseType(typeof(IEnumerable<ReturnDonationInQueueDTO>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Queue([FromQuery] string donationStage)
        {
            var result = DonationLogic.ReturnQueue(donationStage);

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

        [HttpGet("{donationId}/details")]
        [Authorize(Policy = "Authenticated")]
        [ProducesResponseType(typeof(ReturnDonationDetailsDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult GetDonationDetails([FromRoute] int donationId)
        {
            var result = DonationLogic.ReturnDonationDetails(donationId);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var loggedInUserId = identity.FindFirst("UserId")?.Value;
            var loggedInUserRole = identity.FindFirst("Role")?.Value;
            if (!string.IsNullOrEmpty(loggedInUserRole) && loggedInUserRole == "Donator")
            {
                if (!string.IsNullOrEmpty(loggedInUserId) && loggedInUserId != result.Value.BloodDonator.User.Id.ToString())
                {
                    return BadRequest(Result.Error<DonationModel>("Wypierdalaj").ErrorMessages);
                }
            }

            var toReturn = Mapper.Map<DonationModel, ReturnDonationDetailsDTO>(result.Value);

            return Ok(toReturn);
        }

        [HttpGet("{donationId}")]
        [Authorize(Policy = "Authenticated")]
        [ProducesResponseType(typeof(ReturnDonationDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult GetDonationById([FromRoute]int donationId)
        {
            var result = DonationLogic.GetById(donationId);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var loggedInUserId = identity.FindFirst("UserId")?.Value;
            var loggedInUserRole = identity.FindFirst("Role")?.Value;
            if (!string.IsNullOrEmpty(loggedInUserRole) && loggedInUserRole == "Donator")
            {
                if (!string.IsNullOrEmpty(loggedInUserId) && loggedInUserId != result.Value.BloodDonator.User.Id.ToString())
                {
                    return BadRequest(Result.Error<DonationModel>("Wypierdalaj").ErrorMessages);
                }
            }

            var toReturn = Mapper.Map<DonationModel, ReturnDonationDTO>(result.Value);

            return Ok(toReturn);
        }

        [HttpGet]
        [Authorize(Policy = "Worker")]
        [ProducesResponseType(typeof(IEnumerable<ReturnDonationInQueueDTO>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult GetAllDonations()
        {
            var result = DonationLogic.ReturnAll();

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            var toReturn = new List<ReturnDonationInQueueDTO>();

            foreach(var donation in result.Value)
            {
                toReturn.Add(Mapper.Map<DonationModel, ReturnDonationInQueueDTO>(donation));
            }

            return Ok(toReturn);
        }


        [HttpPatch]
        [Authorize(Policy ="Worker")]
        [ProducesResponseType(typeof(UpdateDonationStage), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult Patch (UpdateDonationStage data)
        {
            var result = DonationLogic.UpdateDonationStage(data);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }
    }
}
